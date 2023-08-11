namespace Services.GageRR.Core
{
    /// <summary>
    /// Gage R&R 계산 상수
    /// </summary>
    public interface IConstants
    {
        /// <summary>
        /// K1 상수. 시행횟수
        /// </summary>
        IReadOnlyDictionary<int, decimal> K1 { get; }

        /// <summary>
        /// K2상수. 평가자수
        /// </summary>
        IReadOnlyDictionary<int, decimal> K2 { get; }

        /// <summary>
        /// K3상수. 파트수
        /// </summary>
        IReadOnlyDictionary<int, decimal> K3 { get; }
    }

    /// <summary>
    /// 밀리미터 상수
    /// </summary>
    public class MMConstants : IConstants
    {
        /// <summary>
        /// 시행횟수
        /// </summary>
        private static readonly Dictionary<int, decimal> _k1 = new()
        {
            { 2, 4.56M },
            { 3, 3.05M }
        };

        /// <summary>
        /// 평가자수
        /// </summary>
        private static readonly Dictionary<int, decimal> _k2 = new()
        {
            { 2, 3.65M },
            { 3, 2.7M }
        };

        /// <summary>
        /// 파트수
        /// </summary>
        private static readonly Dictionary<int, decimal> _k3 = new()
        {
            { 2, 3.65M },
            { 3, 2.70M },
            { 4, 2.30M },
            { 5, 2.08M },
            { 6, 1.93M },
            { 7, 1.82M },
            { 8, 1.74M },
            { 9, 1.67M },
            { 10, 1.62M }
        };

        public IReadOnlyDictionary<int, decimal> K1 => _k1;

        public IReadOnlyDictionary<int, decimal> K2 => _k2;

        public IReadOnlyDictionary<int, decimal> K3 => _k3;
    }

    /// <summary>
    /// 인치 상수
    /// </summary>
    public class InchConstants : IConstants
    {
        /// <summary>
        /// 시행횟수
        /// </summary>
        private static readonly Dictionary<int, decimal> _k1 = new()
        {
            { 2, 0.8862M },
            { 3, 0.5908M }
        };

        /// <summary>
        /// 평가자수
        /// </summary>
        private static readonly Dictionary<int, decimal> _k2 = new()
        {
            { 2, 0.7071M },
            { 3, 0.5231M }
        };

        /// <summary>
        /// 파트수
        /// </summary>
        private static readonly Dictionary<int, decimal> _k3 = new()
        {
            { 2, 0.7071M },
            { 3, 0.5231M },
            { 4, 0.4467M },
            { 5, 0.4030M },
            { 6, 0.3742M },
            { 7, 0.3534M },
            { 8, 0.3375M },
            { 9, 0.3249M },
            { 10,0.3146M }
        };

        public IReadOnlyDictionary<int, decimal> K1 => _k1;

        public IReadOnlyDictionary<int, decimal> K2 => _k2;

        public IReadOnlyDictionary<int, decimal> K3 => _k3;
    }

}
