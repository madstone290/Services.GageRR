namespace Services.GageRR.Core.Data
{
    /// <summary>
    /// Gage R&R (Average & Range method) 계산 입력값
    /// </summary>
    public class AverageRangeInput
    {
        /// <summary>
        /// 측정값
        /// </summary>
        public class Record
        {
            /// <summary>
            /// 평가자번호
            /// </summary>
            public int Appraiser { get; set; }

            /// <summary>
            /// 시행번호
            /// </summary>
            public int Trial { get; set; }

            /// <summary>
            /// 파트번호
            /// </summary>
            public int Part { get; set; }

            /// <summary>
            /// 값
            /// </summary>
            public decimal Value { get; set; }
        }

        /// <summary>
        /// 총 평가자수 [2,3]
        /// </summary>
        public int AppraiserCount { get; set; }

        /// <summary>
        /// 총 시행횟수 [2,3]
        /// </summary>
        public int TrialCount { get; set; }

        /// <summary>
        /// 총 파트수 [2,10]
        /// </summary>
        public int PartCount { get; set; }

        /// <summary>
        /// 측정단위
        /// </summary>
        public Unit Unit { get; set; }

        /// <summary>
        /// 규격상한
        /// </summary>
        public decimal SpecUpper { get; set; }

        /// <summary>
        /// 규격하한
        /// </summary>
        public decimal SpecLower { get; set; }

        /// <summary>
        /// 공차
        /// </summary>
        public decimal Tolerance => SpecUpper - SpecLower;

        /// <summary>
        /// 측정값
        /// </summary>
        public List<Record> Records { get; set; } = new();
    }


}

