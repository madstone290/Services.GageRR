using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Services.GageRR.WebApp.Pages
{
    public class RangeMethodModel : PageModel
    {
        [FromQuery]
        public int Appraiser { get; set; } = 2;

        [FromQuery]
        public int Part { get; set; } = 5;

        public void OnGet()
        {
        }
    }
}
