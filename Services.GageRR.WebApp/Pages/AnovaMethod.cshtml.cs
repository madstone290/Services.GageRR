using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Services.GageRR.WebApp.Pages
{
    public class AnovaMethodModel : PageModel
    {
        [FromQuery]
        public int Trial { get; set; } = 3;

        [FromQuery]
        public int Appraiser { get; set; } = 3;

        [FromQuery]
        public int Part { get; set; } = 10;

        public void OnGet()
        {

        }
    }
}