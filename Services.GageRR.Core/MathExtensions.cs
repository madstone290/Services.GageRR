namespace Services.GageRR.Core
{
    /// <summary>
    /// 수학 확장 메소드. 클린코드용.
    /// </summary>
    public static class MathExtensions
    {
        public static double P(this double left, double right)
        {
            return Math.Pow(left, right);
        }

        public static double M(this double left, double right)
        {
            return left * right;
        }

        public static double D(this double left, double right)
        {
            return left / right;
        }

        public static double Sqrt(this double value)
        {
            return Math.Sqrt(value);
        }

        public static decimal P(this decimal left, decimal right)
        {
            return (decimal)Math.Pow((double)left, (double)right);
        }

        public static decimal M(this decimal left, decimal right)
        {
            return left * right;
        }

        public static decimal D(this decimal left, decimal right)
        {
            return left / right;
        }

        public static decimal Sqrt(this decimal value)
        {
            return (decimal)Math.Sqrt((double)value);
        }
    }
}
