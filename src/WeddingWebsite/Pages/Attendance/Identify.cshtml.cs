using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Serilog.Context;
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
        private readonly ILogger<IdentifyModel> logger;

        [BindProperty]
        public InputGuestIdentify Input { get;set;}

        public IdentifyModel(IRsvpService rsvpService, CommonLocalizationService commonLocalizationService, ILogger<IdentifyModel> logger)
        {
            this.rsvpService = rsvpService;
            this.commonLocalizationService = commonLocalizationService;
            this.logger = logger;
        }
        public void OnGet()
        {
            Input = new InputGuestIdentify();
        }
        
        public async Task<IActionResult> OnPostComing()
        {
            using (LogContext.PushProperty("{@Input}", Input))
            {
                logger.LogInformation("####### OnPostComing start: {@Input}", Input);
                //throw new Exception("fdwefwlefj");
                RsvpGuest guest = null;
                if (!string.IsNullOrEmpty(Input?.Email))
                {
                    guest = await rsvpService.FindRsvp(Input?.Email.Trim());
                    if (guest == null)
                        ModelState.AddModelError(string.Empty, commonLocalizationService.Get("Cannot find the email"));
                }
                if (!ModelState.IsValid)
                {
                    logger.LogInformation("####### OnPostComing not valid: {@Input}", Input);
                    return Page();
                }
                await rsvpService.IsComing(guest);
                var cul = HttpContext.Request.RouteValues.GetValueOrDefault("culture");
                logger.LogInformation("####### OnPostComing is valid: {@Input}", Input);
                return RedirectToPage("/Attendance/Edit", new { u = rsvpService.EncodeRsvpEmail(Input.Email), culture = cul });
            }
         

        }
        public async Task<IActionResult> OnPostNotComing()
        {
            using (LogContext.PushProperty("{@Input}", Input))
            {
                logger.LogInformation("####### OnPostNotComing start: {@Input}", Input);
                RsvpGuest guest = null;
                if (!string.IsNullOrEmpty(Input.Email))
                {
                    guest = await rsvpService.FindRsvp(Input?.Email);
                    if (guest == null)
                        ModelState.AddModelError(string.Empty, commonLocalizationService.Get("Cannot find the email"));
                }
                if (!ModelState.IsValid)
                {
                    logger.LogInformation("####### OnPostNotComing not valid: {@Input}", Input);
                    return Page();
                }
                await rsvpService.IsNotComing(guest);
                var cul = HttpContext.Request.RouteValues.GetValueOrDefault("culture");
                logger.LogInformation("####### OnPostNotComing is valid: {@Input}", Input);
                return RedirectToPage("/Attendance/Finished", new { culture = cul });
            }
        }
    }
    public class InputGuestIdentify
    {
        [Display(Name = "Email"), Required(ErrorMessage = "Email Required"), DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        //public override string ToString()
        //{
        //    return base.ToString() + " - Email: " + this.Email;
        //}
    }
}
