using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WeddingWebsite.Services;

namespace WeddingWebsite.Pages.Attendance
{
    public class FinishedModel : PageModel
    {
        private readonly IRsvpService rsvpService;

        public FinishedModel(IRsvpService rsvpService)
        {
            this.rsvpService = rsvpService;
        }

        public void OnGet()
        {
        }
    }
}
