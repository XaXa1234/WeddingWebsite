using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using WeddingWebsite.Models;
using WeddingWebsite.Services;

namespace WeddingWebsite.Pages.Attendance
{
    [IgnoreAntiforgeryToken(Order = 1001)]
    public class IdentifyModel : PageModel
    {
        private readonly IRsvpService rsvpService;
        private readonly CommonLocalizationService commonLocalizationService;

        [BindProperty]
        public InputGuestIdentify Input { get;set;}

        public IdentifyModel(IRsvpService rsvpService, CommonLocalizationService commonLocalizationService)
        {
            this.rsvpService = rsvpService;
            this.commonLocalizationService = commonLocalizationService;
        }
        public void OnGet()
        {
            Input = new InputGuestIdentify();
        }
        
        public async Task<IActionResult> OnPostComing()
        {
            //throw new Exception("fdwefwlefj");
            RsvpGuest guest = null;
            if (!string.IsNullOrEmpty(Input.Email))
            {
                guest = await rsvpService.FindRsvp(Input?.Email);
                if (guest == null)
                    ModelState.AddModelError(string.Empty, commonLocalizationService.Get("Cannot find the email"));
            }
            if (!ModelState.IsValid)
                return Page();
            await rsvpService.IsComing(guest);
            var cul = HttpContext.Request.RouteValues.GetValueOrDefault("culture");
            return RedirectToPage("/Attendance/Edit", new { u=rsvpService.EncodeRsvpEmail(Input.Email), culture = cul });
        }
        public async Task<IActionResult> OnPostNotComing()
        {
            RsvpGuest guest = null;
            if (!string.IsNullOrEmpty(Input.Email))
            {
                guest = await rsvpService.FindRsvp(Input?.Email);
                if (guest == null)
                    ModelState.AddModelError(string.Empty, commonLocalizationService.Get("Cannot find the email"));
            }
            if (!ModelState.IsValid)
                return Page();
            await rsvpService.IsNotComing(guest);
            var cul = HttpContext.Request.RouteValues.GetValueOrDefault("culture");
            return RedirectToPage("/Attendance/Finished", new { culture = cul });
        }
    }
    public class InputGuestIdentify
    {
        [Display(Name = "Email"), Required(ErrorMessage = "Email Required"), DataType(DataType.EmailAddress)]
        public string Email { get; set; }
    }
}
