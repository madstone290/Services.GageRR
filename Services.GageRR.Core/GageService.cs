﻿using Services.GageRR.Core.Data;

namespace Services.GageRR.Core
{
    /// <summary>
    /// Gage R&R 계산기
    /// </summary>
    public class GageService
    {
        /**
         * n: part count
         * r: trial count
         * x_diff: 평가자평균max - 평가자평균min
         * EV = R__ * K1
         * AV = Sqrt((X_diff * K2)^2 - EV^2/nr)
         * GRR = Sqrt(EV^2 + AV^2)
         * PV = Rp * K3
         * TV = Sqrt( GRR^2 + PV^2 )
         * */

        /// <summary>
        /// AverageRange method 계산 상수
        /// </summary>
        private const decimal NDC_CONSTANT = 1.41m;

        /// <summary>
        /// Range method 계산 상수
        /// </summary>
        private const decimal RANGE_CONSTANT = 5.15m;

        /// <summary>
        /// Range method 계산 상수
        /// </summary>
        private const decimal RANGE_D2_CONSTANT = 1.19m;

        private readonly InputValidator _inputValidator = new InputValidator();


        /// <summary>
        /// 범위법 계산.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public RangeOutput RangeMethod(RangeInput input)
        {
            var partRange = input.Records.GroupBy(x => x.Part)
                .ToDictionary(
                    partGroup => partGroup.Key,
                    partGroup => partGroup.Max(x => x.Value) - partGroup.Min(x => x.Value));

            var r_ = partRange.Values.Average();
            var grr = r_.M(RANGE_CONSTANT).D(RANGE_D2_CONSTANT);
            var grr_t = grr.M(100).D(input.Tolerance);

            RangeOutput output = new ()
            {
                PartRange = partRange,
                R_ = r_,
                GRR = grr,
                GRR_T = grr_t
            };
            output.Round();
            return output;
        }

        /// <summary>
        /// 평균 및 범위법(Xbar-R) 계산. 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public AverageRangeOutput AverageRangeMethod(AverageRangeInput input)
        {
            _inputValidator.Validate(input);

            var records = input.Records;

            // 평가자별 시행별 평균
            var appraiserTrialAvg = records.GroupBy(x => x.Appraiser)
                .ToDictionary(
                    appraiserGroup => appraiserGroup.Key,
                    appraiserGroup => appraiserGroup.GroupBy(x => x.Trial)
                        .ToDictionary(
                            trialGroup => trialGroup.Key,
                            trialGroup => trialGroup.Average(x => x.Value)
                        )
                    );

            // 평가자별 파트별 평균
            var appraiserPartAvg = records.GroupBy(x => x.Appraiser)
                .ToDictionary(
                    appraiserGroup => appraiserGroup.Key,
                    appraiserGroup => appraiserGroup.GroupBy(x => x.Part)
                        .ToDictionary(
                            partGroup => partGroup.Key,
                            partGroup => partGroup.Average(x => x.Value)
                        )
                    );

            // 평가자별 파트별 범위
            var appraiserPartRange = records.GroupBy(x => x.Appraiser)
                .ToDictionary(
                    appraiserGroup => appraiserGroup.Key,
                    appraiserGroup => appraiserGroup.GroupBy(x => x.Part)
                        .ToDictionary(
                            partGroup => partGroup.Key,
                            partGroup => partGroup.Max(x => x.Value) - partGroup.Min(x => x.Value)
                        )
                    );

            // 평가자별 파트별 범위의 평균(R_)
            var appraiserPartRangeAvg = appraiserPartRange.GroupBy(x => x.Key)
                .ToDictionary(
                    appraiserGroup => appraiserGroup.Key,
                    appraiserGroup => appraiserGroup.Average(x => x.Value.Values.Average())
                );

            // 평가자별 평균(X_)
            var appraiserAvg = records.GroupBy(x => x.Appraiser)
                .ToDictionary(
                    appraiserGroup => appraiserGroup.Key,
                    appraiserGroup => appraiserGroup.Average(x => x.Value)
                );

            // 파트별 평균
            var partAvg = records.GroupBy(x => x.Part)
                .ToDictionary(
                    partGroup => partGroup.Key,
                    partGroup => partGroup.Average(x => x.Value)
                );

            IConstants constants = input.Unit == Unit.MM ? new MMConstants() : new InchConstants();
            var k1 = constants.K1[input.TrialCount];
            var k2 = constants.K2[input.AppraiserCount];
            var k3 = constants.K3[input.PartCount];

            // 전체 평가자의 파트범위 평균의 평균(R__)
            var r__ = appraiserPartRangeAvg.Values.Average();
            var ev = k1 * r__;

            // 평가자 평균의 범위(X_diff)
            var x_diff = appraiserAvg.Values.Max() - appraiserAvg.Values.Min();
            var av = (k2.M(x_diff).P(2) - ev.P(2).D(input.PartCount * input.TrialCount)).Sqrt();
            var grr = (ev.P(2) + av.P(2)).Sqrt();

            var rp = partAvg.Values.Max() - partAvg.Values.Min();
            var pv = k3 * rp;

            var tv = (grr.P(2) + pv.P(2)).Sqrt();

            AverageRangeOutput output = new()
            {
                AppraiserTrialAvg = appraiserTrialAvg,
                AppraiserPartAvg = appraiserPartAvg,
                AppraiserPartRange = appraiserPartRange,
                AppraiserPartRangeAvg = appraiserPartRangeAvg,
                AppraiserAvg = appraiserAvg,
                PartAvg = partAvg,
                PartAvgAvg = partAvg.Values.Average(),

                Rp = rp,
                R__ = r__,
                X_Diff = x_diff,

                EV_SD = ev,
                AV_SD = av,
                GRR_SD = grr,
                PV_SD = pv,
                TV_SD = tv,

                EV_SV = tv == 0 ? null : 100 * ev / tv,
                AV_SV = tv == 0 ? null : 100 * av / tv,
                GRR_SV = tv == 0 ? null : 100 * grr / tv,
                PV_SV = tv == 0 ? null : 100 * pv / tv,

                EV_T = input.Tolerance == 0 ? null : 100 * ev / input.Tolerance,
                AV_T = input.Tolerance == 0 ? null : 100 * av / input.Tolerance,
                GRR_T = input.Tolerance == 0 ? null : 100 * grr / input.Tolerance,
                PV_T = input.Tolerance == 0 ? null : 100 * pv / input.Tolerance,

                NDC = grr == 0 ? null : NDC_CONSTANT * pv / grr
            };
            output.Round();

            return output;
        }
    }
}