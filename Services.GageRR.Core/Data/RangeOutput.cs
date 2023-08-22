namespace Services.GageRR.Core.Data
{
    /// <summary>
    /// Range method에 사용되는 출력값
    /// </summary>
    public record RangeOutput
    {
        /// <summary>
        /// 측정자별 평균
        /// </summary>
        public Dictionary<int, decimal> AppraiserAvg { get; set; } = new Dictionary<int, decimal>();

        /// <summary>
        /// 부품별 범위
        /// </summary>
        public Dictionary<int, decimal> PartRange { get; set; } = new Dictionary<int, decimal>();

        /// <summary>
        /// 범위평균(R_, R bar)
        /// </summary>
        public decimal R_ { get; set; }

        /// <summary>
        /// Gage R&R
        /// </summary>
        public decimal GRR { get; set; }

        /// <summary>
        /// GRR 공차(%)
        /// </summary>
        public decimal GRR_T { get; set; }

        public void Round(int digit = 3)
        {
            foreach (var key in AppraiserAvg.Keys.ToList())
            {
                AppraiserAvg[key] = Math.Round(AppraiserAvg[key], digit);
            }

            foreach (var key in PartRange.Keys.ToList())
            {
                PartRange[key] = Math.Round(PartRange[key], digit);
            }

            R_ = Math.Round(R_, digit);
            GRR = Math.Round(GRR, digit);
            GRR_T = Math.Round(GRR_T, digit);

        }
    }
}
