namespace Services.GageRR.Core
{
    /// <summary>
    /// Gage R&R 계산 결과
    /// </summary>
    public class Output
    {
        public Input Input { get; set; } = null!;

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
        public decimal EV_SV { get; set; }
        public decimal AV_SV { get; set; }
        public decimal GRR_SV { get; set; }
        public decimal PV_SV { get; set; }

        // T: Tolerance(%)
        public decimal EV_T { get; set; }
        public decimal AV_T { get; set; }
        public decimal GRR_T { get; set; }
        public decimal PV_T { get; set; }

        public decimal NDC { get; set; }

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
    }
}
