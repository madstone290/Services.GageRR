using Services.GageRR.Core.Data;

namespace Services.GageRR.Core
{
    /// <summary>
    /// Gage R&R 계산기
    /// </summary>
    public class GageService
    {

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
            var appraiserAvg = input.Records.GroupBy(x => x.Appraiser)
                .ToDictionary(
                    appraiserGroup => appraiserGroup.Key,
                    appraiserGroup => appraiserGroup.Average(x => x.Value));

            var partRange = input.Records.GroupBy(x => x.Part)
                .ToDictionary(
                    partGroup => partGroup.Key,
                    partGroup => partGroup.Max(x => x.Value) - partGroup.Min(x => x.Value));

            var r_ = partRange.Values.Average();
            var grr = r_.M(RANGE_CONSTANT).D(RANGE_D2_CONSTANT);
            var grr_t = grr.M(100).D(input.Tolerance);

            RangeOutput output = new()
            {
                AppraiserAvg = appraiserAvg,
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

        public AnovaOutput AnovaMethod(AverageRangeInput input)
        {
            var records = input.Records;

            decimal x__ = records.Average(x => x.Value);
            var x_o = records.GroupBy(x => x.Appraiser)
                .ToDictionary(
                    @operator => @operator.Key,
                    @operator => @operator.Average(x => x.Value));

            var x_p = records.GroupBy(x => x.Part)
                .ToDictionary(
                    part => part.Key,
                    part => part.Average(x => x.Value));

            var x_op = records.GroupBy(x => new
            {
                x.Appraiser,
                x.Part
            })
                .ToDictionary(
                    op => op.Key,
                    op => op.Average(x => x.Value));

            int o = input.AppraiserCount;
            int p = input.PartCount;
            int r = input.TrialCount;

            int df_o = input.AppraiserCount - 1;
            int df_p = input.PartCount - 1;
            int df_op = df_o * df_p;
            int df_r = input.AppraiserCount * input.PartCount * (input.TrialCount - 1);
            int df_total = input.AppraiserCount * input.PartCount * input.TrialCount - 1;

            decimal ss_o = p * r * x_o.Sum(x_o_each => (x_o_each.Value - x__).P(2));
            decimal ss_p = o * r * x_p.Sum(x_p_each => (x_p_each.Value - x__).P(2));
            decimal ss_total = records.Sum(r_each => (r_each.Value - x__).P(2));
            decimal ss_r = records.Sum(r_each =>
            {
                var opId = new
                {
                    r_each.Appraiser,
                    r_each.Part
                };
                return (r_each.Value - x_op[opId]).P(2);
            });
            decimal ss_op = ss_total - ss_o - ss_p - ss_r;

            decimal ms_o = ss_o / df_o;
            decimal ms_p = ss_p / df_p;
            decimal ms_op = ss_op / df_op;
            decimal ms_r = ss_r / df_r;

            decimal f_o = ms_o / ms_op;
            decimal f_p = ms_p / ms_op;
            decimal f_op = ms_op / ms_r;

            decimal p_o = 1 - (decimal)alglib.fdistribution(df_o, df_op, (double)f_o);
            decimal p_p = 1 - (decimal)alglib.fdistribution(df_p, df_op, (double)f_p);
            decimal p_op = 1 - (decimal)alglib.fdistribution(df_op, df_r, (double)f_op);


            var output = new AnovaOutput()
            {
                DF_Operator = df_o,
                DF_Part = df_p,
                DF_Operator_Part = df_op,
                DF_Repeatability = df_r,
                DF_Total = df_total,

                SS_Operator = (double)ss_o,
                SS_Part = (double)ss_p,
                SS_Operator_Part = (double)ss_op,
                SS_Repeatability = (double)ss_r,
                SS_Total = (double)ss_total,

                MS_Operator = (double)ms_o,
                MS_Part = (double)ms_p,
                MS_Operator_Part = (double)ms_op,
                MS_Repeatability = (double)ms_r,

                F_Operator = (double)f_o,
                F_Part = (double)f_p,
                F_Operator_Part = (double)f_op,

                P_Operator = (double)p_o,
                P_Part = (double)p_p,
                P_Operator_Part = (double)p_op
            };

            output.Round();
            return output;
        }
    }

}

