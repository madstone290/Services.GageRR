namespace Services.GageRR.Core.Data
{
    public class AnovaOutput
    {
        public int DF_Operator { get; set; }
        public int DF_Part { get; set; }
        public int DF_Operator_Part { get; set; }
        public int DF_Repeatability { get; set; }
        public int DF_Total { get; set; }
        public double SS_Operator { get; set; }
        public double SS_Part { get; set; }
        public double SS_Operator_Part { get; set; }
        public double SS_Repeatability { get; set; }
        public double SS_Total { get; set; }
        public double MS_Operator { get; set; }
        public double MS_Part { get; set; }
        public double MS_Operator_Part { get; set; }
        public double MS_Repeatability { get; set; }
        public double F_Operator { get; set; }
        public double F_Part { get; set; }
        public double F_Operator_Part { get; set; }
        public double P_Operator { get; set; }
        public double P_Part { get; set; }
        public double P_Operator_Part { get; set; }


        public void Round(int digit = 4)
        {
            SS_Operator = Math.Round(SS_Operator, digit);
            SS_Part = Math.Round(SS_Part, digit);
            SS_Operator_Part = Math.Round(SS_Operator_Part, digit);
            SS_Repeatability = Math.Round(SS_Repeatability, digit);
            SS_Total = Math.Round(SS_Total, digit);
            MS_Operator = Math.Round(MS_Operator, digit);
            MS_Part = Math.Round(MS_Part, digit);
            MS_Operator_Part = Math.Round(MS_Operator_Part, digit);
            MS_Repeatability = Math.Round(MS_Repeatability, digit);
            F_Operator = Math.Round(F_Operator, digit);
            F_Part = Math.Round(F_Part, digit);
            F_Operator_Part = Math.Round(F_Operator_Part, digit);
            P_Operator = Math.Round(P_Operator, digit);
            P_Part = Math.Round(P_Part, digit);
            P_Operator_Part = Math.Round(P_Operator_Part, digit);
        }
    }
}
