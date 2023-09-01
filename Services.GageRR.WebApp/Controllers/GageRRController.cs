using Microsoft.AspNetCore.Mvc;
using Services.GageRR.Core;
using Services.GageRR.Core.Data;

namespace Services.GageRR.WebApp.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class GageRRController : ControllerBase
    {
        private readonly GageService _gageService = new GageService();

        [HttpPost]
        [Route("Range")]
        public IActionResult Calcuate([FromBody] RangeInput input)
        {
            var output = _gageService.RangeMethod(input);
            return Ok(output);
        }

        [HttpPost]
        [Route("AverageRange")]
        public IActionResult Calcuate([FromBody] AverageRangeInput input)
        {
            var output = _gageService.AverageRangeMethod(input);
            return Ok(output);
        }

        [HttpPost]
        [Route("Anova")]
        public IActionResult Anova([FromBody] AnovaInput input)
        {
            var output = _gageService.AnovaMethod(input);
            return Ok(output);
        }
    }
}
