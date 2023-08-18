using Services.GageRR.Core;
using Services.GageRR.Core.Data;

namespace Services.GageRR.Tests
{
    public class RangeMethodTests
    {
        [Fact]
        public void Case1()
        {
            RangeInput input = new()
            {
                AppraiserCount = 2,
                PartCount = 5,
                SpecLower = 79.7m,
                SpecUpper = 80.3m,
                Records = new List<RangeInput.Record>()
                {
                    new RangeInput.Record(1, 1, 79.980m),
                    new RangeInput.Record(1, 2, 80.020m),
                    new RangeInput.Record(1, 3, 80.070m),
                    new RangeInput.Record(1, 4, 79.990m),
                    new RangeInput.Record(1, 5, 80.050m),

                    new RangeInput.Record(2, 1, 79.990m),
                    new RangeInput.Record(2, 2, 80.020m),
                    new RangeInput.Record(2, 3, 80.050m),
                    new RangeInput.Record(2, 4, 80.010m),
                    new RangeInput.Record(2, 5, 80.060m),
                }
            };

            GageService gageService = new();
            RangeOutput output = gageService.RangeMethod(input);


            Assert.Equal(0.010M, output.PartRange[1], 3);
            Assert.Equal(0.000M, output.PartRange[2], 3);
            Assert.Equal(0.020M, output.PartRange[3], 3);
            Assert.Equal(0.020M, output.PartRange[4], 3);
            Assert.Equal(0.010M, output.PartRange[5], 3);

            Assert.Equal(0.012M, output.R_, 3);
            Assert.Equal(0.052M, output.GRR, 3);
            Assert.Equal(8.66M, output.GRR_T, 2);

        }
    }
}
