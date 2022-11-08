using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Serilog.Context;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using WeddingWebsite.Models;
using WeddingWebsite.Services;

namespace WeddingWebsite.Pages.Attendance
{
    [IgnoreAntiforgeryToken(Order = 1001)]
    public class EditModel : PageModel
    {
        private readonly IRsvpService rsvpService;
        private readonly ILogger<EditModel> logger;

        public EditModel(IRsvpService rsvpService, ILogger<EditModel> logger)
        {
            this.rsvpService = rsvpService;
            this.logger = logger;
        }

        [BindProperty]
        public InputGuestEdit Input { get; set; }
        public async Task<IActionResult> OnGet(string u)
        {
            using (LogContext.PushProperty("u", u))
            {
                var emailDecoded = rsvpService.DecodeRsvpEmail(u);
                using (LogContext.PushProperty("emailDecoded", emailDecoded))
                {
                    var guest = await rsvpService.FindRsvp(emailDecoded);
                    if (guest == null)
                    {
                        logger.LogInformation("####### Cannot find email");
                        var cul = HttpContext.Request.RouteValues.GetValueOrDefault("culture");
                        return RedirectToPage("/Attendance/Identify", new { culture = cul });
                    }
                    Input = InputGuestEdit.From(guest);
                    return Page();
                }
            }
        }
        
        public async Task<IActionResult> OnPost(string u)
        {

            using (LogContext.PushProperty("u", u))
            {
                var emailDecoded = rsvpService.DecodeRsvpEmail(u);
                using (LogContext.PushProperty("emailDecoded", emailDecoded))
                {
                    logger.LogInformation("####### EDIT: {@EditInformations}", Input);
                    var cul = HttpContext.Request.RouteValues.GetValueOrDefault("culture");
                    var guest = await rsvpService.FindRsvp(emailDecoded);
                    if (guest == null)
                    {
                        logger.LogInformation("####### Cannot find email");
                        return RedirectToPage("/Attendance/Identify", new { culture = cul });
                    }
                    await rsvpService.UpdateRsvp(emailDecoded, Input.FirsName, Input.LastName, Input.Address,
                                                    Input.PhoneNumber,  Input.Comment,
                                                    Input.HasGuest, Input.GuestFirstName, Input.GuestLastName, Input.CommentGuest);
                    logger.LogInformation("####### infos updated");
                    return RedirectToPage("/Attendance/Finished", new { culture = cul });
                }
            }
        }
    }
    public class InputGuestEdit
    {
        [Display(Name = "First Name"), Required(ErrorMessage = "First Name Required")]
        public string FirsName { get; set; }
        [Display(Name = "Last Name"), Required(ErrorMessage = "Last Name Required")]
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        [Display(Name = "Is Coming"), Required(ErrorMessage = "Is Coming Required")]
        public bool IsComing { get; set; }
        public string Comment { get; set; }
        public bool CanHaveGuest { get ;private  set; }
        public bool HasGuest { get; set; }
        public string GuestFirstName { get; set; }
        public string GuestLastName { get; set; }
        public string CommentGuest { get; set; }
        public static InputGuestEdit From(RsvpGuest guest)
        {
            InputGuestEdit result = new InputGuestEdit();
            result.FirsName = guest.FirsName;
            result.LastName = guest.LastName;
            if(guest.IsComing.HasValue)
                result.IsComing = guest.IsComing.Value;
            result.PhoneNumber = guest.PhoneNumber;
            result.Address = guest.Address;
            result.Comment = guest.Comment;
            result.CanHaveGuest = guest.CanHaveGuest;
            if(guest.HasGuest.HasValue)
                result.HasGuest = guest.HasGuest.Value;
            result.GuestFirstName = guest.GuestFirstName;
            result.GuestLastName = guest.GuestLastName;
            result.CommentGuest = guest.CommentGuest;
            return result;

        }
    }
}
