using Microsoft.AspNetCore.Mvc;
using Services.GageRR.Core;

namespace Services.GageRR.WebApp.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class GageRRController : ControllerBase
    {
        private readonly GageService _gageService = new GageService();

        [HttpPost]
        [Route("Calculate")]
        public IActionResult Calcuate([FromBody] Input input)
        {
            var output = _gageService.Calculate(input);
            return Ok(output);
        }
    }
}
