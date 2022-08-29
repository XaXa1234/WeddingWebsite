using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using WeddingWebsite.Services;

namespace WeddingWebsite.Pages.Attendance
{
    public class IdentifyModel : PageModel
    {
        private readonly IRsvpService rsvpService;


        [BindProperty]
        public InputGuestIdentify Input { get;set;}

        public IdentifyModel(IRsvpService rsvpService)
        {
            this.rsvpService = rsvpService;
        }
        public void OnGet()
        {
            Input = new InputGuestIdentify();
        }
        public async Task<IActionResult> OnPostComing()
        {
            var guest = await rsvpService.FindRsvp(Input?.Email);
            if(guest == null)
                ModelState.AddModelError(string.Empty, "Cannot find the email");
            if (!ModelState.IsValid)
                return Page();
            await rsvpService.IsComing(guest);
            return RedirectToPage("/Attendance/Edit", new { u=rsvpService.EncodeRsvpEmail(Input.Email) });
        }
        public async Task<IActionResult> OnPostNotComing()
        {
            var guest = await rsvpService.FindRsvp(Input?.Email);
            if (guest == null)
                ModelState.AddModelError(string.Empty, "Cannot find the email");
            if (!ModelState.IsValid)
                return Page();
            await rsvpService.IsNotComing(guest);
            return RedirectToPage("/Attendance/Finished");
        }
    }
    public class InputGuestIdentify
    {
        [Required]
        [Display(Name = "Email you received the invitation to")]
        public string Email { get; set; }
    }
}
