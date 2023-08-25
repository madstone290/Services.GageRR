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

    }
}
