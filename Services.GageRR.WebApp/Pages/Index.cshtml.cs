using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Services.GageRR.WebApp.Pages
{
    public class IndexModel : PageModel
    {
        [FromQuery]
        public int Trial { get; set; } = 3;

        [FromQuery]
        public int Appraiser { get; set; } = 3;

        [FromQuery]
        public int Part { get; set; } = 10;

        public List<string> AppraiserNames = new List<string> { "송병길", "이연하", "Bruce wayne" };


        public void OnGet()
        {

        }
    }
}