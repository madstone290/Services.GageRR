using Services.GageRR.Core;

namespace Services.GageRR.Tests
{
    public class GageServiceTests
    {
        [Fact]
        public void MM_Appraiser3_Trial3_Part10()
        {
            var testData = new List<List<List<double>>>()
            {
                // 평가자1
                new List<List<double>>()
                {
                    // 시행1
                    new List<double>(){ 24.130,  24.140  ,24.130  ,24.140  ,24.140  ,24.130  ,24.130  ,24.120  ,24.140  ,24.130 },
                    // 시행2
                    new List<double>(){ 24.130,  24.140  ,24.130  ,24.140  ,24.140  ,24.130  ,24.130  ,24.120  ,24.140  ,24.130 },
                    // 시행3
                    new List<double>(){ 24.130,  24.140  ,24.130  ,24.140  ,24.140  ,24.130  ,24.130  ,24.120  ,24.140  ,24.130 }
                },
                new List<List<double>>()
                {
                    new List<double>(){ 24.130,   24.140  ,24.130  ,24.140  ,24.140  ,24.130  ,24.130  ,24.120  ,24.140  ,24.130 },
                    new List<double>(){ 24.130,   24.140  ,24.140  ,24.140  ,24.140  ,24.130  ,24.130  ,24.120  ,24.140  ,24.130 },
                    new List<double>(){ 24.130,   24.140  ,24.130  ,24.140  ,24.140  ,24.130  ,24.130  ,24.120  ,24.140  ,24.130 }
                },
                new List<List<double>>
                {
                    new List<double>(){ 24.130  ,24.140  ,24.130  ,24.140  ,24.140  ,24.130  ,24.130  ,24.120  ,24.140  ,24.130 },
                    new List<double>(){ 24.130  ,24.140  ,24.130  ,24.140  ,24.140  ,24.130  ,24.130  ,24.130  ,24.140  ,24.130 },
                    new List<double>(){ 24.130  ,24.140  ,24.130  ,24.140  ,24.140  ,24.130  ,24.130  ,24.120  ,24.140  ,24.130 }
                }
            };


            var input = new Input()
            {
                AppraiserCount = 3,
                TrialCount = 3,
                PartCount = 10,
                SpecUpper = 24.2M,
                SpecLower = 24.0M,
                Unit = Unit.MM,
                Records = InputRecordFactory.Build(testData)
            };

            Output output = new GageService().Calculate(input);

            Assert.Equal(0.002M, output.EV_SD, 3);
            Assert.Equal(0.001M, output.AV_SD, 3);
            Assert.Equal(0.002M, output.GRR_SD, 3);
            Assert.Equal(0.031M, output.PV_SD, 3);
            Assert.Equal(0.031M, output.TV_SD, 3);

            Assert.Equal(6.63M, output.EV_SV, 2);
            Assert.Equal(2.67M, output.AV_SV, 2);
            Assert.Equal(7.15M, output.GRR_SV, 2);
            Assert.Equal(99.74M, output.PV_SV, 2);

            Assert.Equal(1.02M, output.EV_T, 2);
            Assert.Equal(0.41M, output.AV_T, 2);
            Assert.Equal(1.10M, output.GRR_T, 2);
            Assert.Equal(15.30M, output.PV_T, 2);

            Assert.Equal(19.680M, output.NDC, 3);
        }

        [Fact]
        public void Inch_Appraiser3_Trial3_Part5()
        {
            var testData = new List<List<List<double>>>()
            {
                // 평가자1
                new List<List<double>>()
                {
                    // 시행1
                    new List<double>(){ 3.29, 2.44, 4.34, 3.47, 2.20 },
                    // 시행2
                    new List<double>(){ 3.41, 2.32, 4.17, 3.50, 2.08 },
                    // 시행3
                    new List<double>(){ 3.64, 2.42, 4.27, 3.64, 2.16 }
                },
                new List<List<double>>()
                {
                    new List<double>(){ 3.08, 2.53, 4.19, 3.01, 2.44 },
                    new List<double>(){ 3.25, 1.78, 3.94, 4.03, 1.80 },
                    new List<double>(){ 3.07, 2.32, 4.34, 3.20, 1.72 }
                },
                new List<List<double>>
                {
                    new List<double>(){ 3.04, 1.62, 3.88, 3.14, 1.54 },
                    new List<double>(){ 2.89, 1.87, 4.09, 3.20, 1.93 },
                    new List<double>(){ 2.85, 2.04, 3.67, 3.11, 1.55 },
                }
            };


            var input = new Input()
            {
                AppraiserCount = 3,
                TrialCount = 3,
                PartCount = 5,
                SpecUpper = 5,
                SpecLower = 2,
                Unit = Unit.Inch,
                Records = InputRecordFactory.Build(testData)
            };

            Output output = new GageService().Calculate(input);

            Assert.Equal(0.217M, output.EV_SD, 3);
            Assert.Equal(0.235M, output.AV_SD, 3);
            Assert.Equal(0.320M, output.GRR_SD, 3);
            Assert.Equal(0.872M, output.PV_SD, 3);
            Assert.Equal(0.929M, output.TV_SD, 3);

            Assert.Equal(23.4M, output.EV_SV, 1);
            Assert.Equal(25.3M, output.AV_SV, 1);
            Assert.Equal(34.5M, output.GRR_SV, 1);
            Assert.Equal(93.9M, output.PV_SV, 1);

            // Tolerance data is not provided

            Assert.Equal(3.8M, output.NDC, 1);
        }
    }
}