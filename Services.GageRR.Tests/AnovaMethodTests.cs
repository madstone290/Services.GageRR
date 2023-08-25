using Services.GageRR.Core;
using Services.GageRR.Core.Data;

namespace Services.GageRR.Tests
{
    public class AnovaMethodTests
    {
        [Fact]
        public void Case1()
        {
            var testData = new List<List<List<double>>>()
            {
                new List<List<double>>()
                {
new List<double>(){ 2.78, 2.36, 2.22, 4.56, 3.56, 2.11, 2.24, 5.47, 2.44, 4.10 },
new List<double>(){ 1.87, 2.36, 2.45, 4.21, 3.47, 2.23, 2.24, 5.89, 1.80, 3.88 },
new List<double>(){ 1.87, 2.21, 2.33, 4.13, 3.69, 2.12, 2.27, 5.31, 1.72, 3.56 },
                },
                new List<List<double>>()
                {
new List<double>(){ 2.56, 2.13, 2.41, 4.12, 3.47, 2.11, 2.22, 5.36, 2.44, 4.65 },
new List<double>(){ 2.22, 2.36, 2.36, 4.36, 3.89, 2.23, 2.23, 5.47, 1.80, 3.89 },
new List<double>(){ 2.14, 2.34, 2.33, 4.56, 3.78, 2.12, 2.24, 5.87, 1.72, 4.00 },
                },
                new List<List<double>>
                {
new List<double>(){ 2.56, 2.56, 2.12, 4.25, 3.47, 2.11, 2.27, 5.21, 2.44, 4.12 },
new List<double>(){ 2.22, 2.12, 2.45, 4.36, 3.89, 2.23, 2.26, 5.36, 1.80, 3.25 },
new List<double>(){ 2.15, 2.32, 2.33, 4.22, 3.75, 2.12, 2.22, 5.54, 1.72, 3.69 },
                }
            };

            var input = new AverageRangeInput()
            {
                AppraiserCount = 3,
                TrialCount = 3,
                PartCount = 10,
                Records = InputRecordFactory.Build(testData)
            };

            AnovaOutput output = new GageService().AnovaMethod(input);

            Assert.Equal(2, output.DF_Operator);
            Assert.Equal(9, output.DF_Part);
            Assert.Equal(18, output.DF_Operator_Part);
            Assert.Equal(60, output.DF_Repeatability);
            Assert.Equal(89, output.DF_Total);


            Assert.Equal(0.0999, output.SS_Operator, 4);
            Assert.Equal(116.5294, output.SS_Part, 4);
            Assert.Equal(0.4468, output.SS_Operator_Part, 4);
            Assert.Equal(3.6058, output.SS_Repeatability, 4);
            Assert.Equal(120.6820, output.SS_Total, 4);

            Assert.Equal(0.0500, output.MS_Operator, 4);
            Assert.Equal(12.9477, output.MS_Part, 4);
            Assert.Equal(0.0248, output.MS_Operator_Part, 4);
            Assert.Equal(0.0601, output.MS_Repeatability, 4);

            Assert.Equal(2.0128, output.F_Operator, 4);
            Assert.Equal(521.6101, output.F_Part, 4);
            Assert.Equal(0.4130, output.F_Operator_Part, 4);

            Assert.Equal(0.1626, output.P_Operator, 4);
            Assert.Equal(0.0000, output.P_Part, 4);
            Assert.Equal(0.9799, output.P_Operator_Part, 4);

        }
    }
}
