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
        [Route("Calculate")]
        public IActionResult Calcuate([FromBody] AverageRangeInput input)
        {
            var output = _gageService.AverageRangeMethod(input);
            return Ok(output);
        }
    }
}
