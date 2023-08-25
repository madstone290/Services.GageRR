using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Services.GageRR.WebApp.Pages
{
    public class IndexModel : PageModel
    {
        public IActionResult OnGet()
        {
            return LocalRedirect("/AverageRangeMethod");
        }
    }
}
