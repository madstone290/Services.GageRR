namespace Services.GageRR.Core.Data
{
    /// <summary>
    /// Gage R&R (Average & Range method) 계산 결과
    /// </summary>
    public class AverageRangeOutput
    {
        /// <summary>
        /// 평가자/시행횟수 기준 평균 ex) 측정자1번, 시행1번에 대한 총 파트의 평균
        /// </summary>
        public Dictionary<int, Dictionary<int, decimal>> AppraiserTrialAvg { get; set; } = new();

        /// <summary>
        /// 평가자/파트 기준 평균 ex) 측정자1번, 파트1번에 대한 총 시행의 평균
        /// </summary>
        public Dictionary<int, Dictionary<int, decimal>> AppraiserPartAvg { get; set; } = new();

        /// <summary>
        /// 평가자/파트기준 범위 ex) 측정자1번, 파트1번에 대한 총 시행의 범위(최대-최소)
        /// </summary>
        public Dictionary<int, Dictionary<int, decimal>> AppraiserPartRange { get; set; } = new();

        /// <summary>
        /// 평가자별 평균(X_)
        /// </summary>
        public Dictionary<int, decimal> AppraiserAvg { get; set; } = new();

        /// <summary>
        /// 평가자별 파트별 범위의 평균(R_)
        /// </summary>
        public Dictionary<int, decimal> AppraiserPartRangeAvg { get; set; } = new();

        /// <summary>
        /// 파트별 평균 ex) 1번파트의 모든평가자/모든시행에 대한 평균
        /// </summary>
        public Dictionary<int, decimal> PartAvg { get; set; } = new();

        /// <summary>
        /// 파트평균의 평균(전체 평균, X_)
        /// </summary>
        public decimal PartAvgAvg { get; set; }

        /// <summary>
        /// 파트별 평균의 범위(Rp). 파트평균최대 - 파트평균최소
        /// </summary>
        public decimal Rp { get; set; }

        /// <summary>
        /// 평가자별 범위의 평균의 평균(R__). R_의 평균
        /// </summary>
        public decimal R__ { get; set; }

        /// <summary>
        /// 평가자별 평균의 범위. 평가자별 평균최대 - 평가자별 평균최소
        /// </summary>
        public decimal X_Diff { get; set; }

        // SD: Standard Deviation

        /// <summary>
        /// 반복성. Repeatibility. Equipment Variation
        /// </summary>
        public decimal EV_SD { get; set; }

        /// <summary>
        /// 재현성. Reproducibility. Appraiser Variation
        /// </summary>
        public decimal AV_SD { get; set; }

        /// <summary>
        /// 반복성&재현성. Repeatability & Reproducibility. Gage R&R
        /// </summary>
        public decimal GRR_SD { get; set; }

        /// <summary>
        /// 파트변동. Part Variation
        /// </summary>
        public decimal PV_SD { get; set; }

        /// <summary>
        /// 총 변동. Total Variation
        /// </summary>
        public decimal TV_SD { get; set; }

        // SV: Study Variation(%)
        public decimal? EV_SV { get; set; }
        public decimal? AV_SV { get; set; }
        public decimal? GRR_SV { get; set; }
        public decimal? PV_SV { get; set; }

        // T: Tolerance(%)
        public decimal? EV_T { get; set; }
        public decimal? AV_T { get; set; }
        public decimal? GRR_T { get; set; }
        public decimal? PV_T { get; set; }

        public decimal? NDC { get; set; }

        public string PrintSD()
        {
            return $"EV: {EV_SD:F3}, AV: {AV_SD:F3}, GRR: {GRR_SD:F3}, PV: {PV_SD:F3}, TV: {TV_SD:F3}";
        }

        public string PrintSVPercent()
        {
            return $"EV%: {EV_SV:F3}, AV%: {AV_SV:F3}, GRR%: {GRR_SV:F3}, PV%: {PV_SV:F3}";
        }

        public string PrintTPercent()
        {
            return $"EV%: {EV_T:F3}, AV%: {AV_T:F3}, GRR%: {GRR_T:F3}, PV%: {PV_T:F3}";
        }

        public void Round(int digit = 3)
        {
            foreach (var key in AppraiserTrialAvg.Keys)
            {
                foreach (var subKey in AppraiserTrialAvg[key].Keys)
                {
                    AppraiserTrialAvg[key][subKey] = Math.Round(AppraiserTrialAvg[key][subKey], digit);
                }
            }

            foreach (var key in AppraiserPartAvg.Keys)
            {
                foreach (var subkey in AppraiserPartAvg[key].Keys)
                {
                    AppraiserPartAvg[key][subkey] = Math.Round(AppraiserPartAvg[key][subkey], digit);
                }
            }

            foreach (var key in AppraiserPartRange.Keys)
            {
                foreach (var subkey in AppraiserPartRange[key].Keys)
                {
                    AppraiserPartRange[key][subkey] = Math.Round(AppraiserPartRange[key][subkey], digit);
                }
            }

            foreach (var key in AppraiserAvg.Keys)
            {
                AppraiserAvg[key] = Math.Round(AppraiserAvg[key], digit);
            }

            foreach (var key in AppraiserPartRangeAvg.Keys)
            {
                AppraiserPartRangeAvg[key] = Math.Round(AppraiserPartRangeAvg[key], digit);
            }

            foreach (var key in PartAvg.Keys)
            {
                PartAvg[key] = Math.Round(PartAvg[key], digit);
            }
            PartAvgAvg = Math.Round(PartAvgAvg, digit);
            Rp = Math.Round(Rp, digit);
            R__ = Math.Round(R__, digit);
            X_Diff = Math.Round(X_Diff, digit);

            EV_SD = Math.Round(EV_SD, digit);
            AV_SD = Math.Round(AV_SD, digit);
            GRR_SD = Math.Round(GRR_SD, digit);
            PV_SD = Math.Round(PV_SD, digit);
            TV_SD = Math.Round(TV_SD, digit);

            if (EV_SV.HasValue)
                EV_SV = Math.Round(EV_SV.Value, digit);
            if (AV_SV.HasValue)
                AV_SV = Math.Round(AV_SV.Value, digit);
            if (GRR_SV.HasValue)
                GRR_SV = Math.Round(GRR_SV.Value, digit);
            if (PV_SV.HasValue)
                PV_SV = Math.Round(PV_SV.Value, digit);

            if (EV_T.HasValue)
                EV_T = Math.Round(EV_T.Value, digit);
            if (AV_T.HasValue)
                AV_T = Math.Round(AV_T.Value, digit);
            if (GRR_T.HasValue)
                GRR_T = Math.Round(GRR_T.Value, digit);
            if (PV_T.HasValue)
                PV_T = Math.Round(PV_T.Value, digit);

            if (NDC.HasValue)
                NDC = Math.Round(NDC.Value, digit);
        }

    }
}
