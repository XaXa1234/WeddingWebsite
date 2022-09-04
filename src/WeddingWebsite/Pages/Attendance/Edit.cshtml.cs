using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
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

        public EditModel(IRsvpService rsvpService)
        {
            this.rsvpService = rsvpService;
        }

        [BindProperty]
        public InputGuestEdit Input { get; set; }
        public async Task<IActionResult> OnGet(string u)
        {
            var emailDecoded =  rsvpService.DecodeRsvpEmail(u);
            var guest = await rsvpService.FindRsvp(emailDecoded);
            if (guest == null)
                return RedirectToPage("/Attendance/Identify");
            Input = InputGuestEdit.From(guest);
            return Page();
        }
        
        public async Task<IActionResult> OnPost(string u)
        {
            var emailDecoded = rsvpService.DecodeRsvpEmail(u);
            var guest = await rsvpService.FindRsvp(emailDecoded);
            if (guest == null)
                return RedirectToPage("/Attendance/Identify");
            await rsvpService.UpdateRsvp(emailDecoded, Input.FirsName, Input.LastName, 
                                            Input.PhoneNumber, Input.IsComing, Input.Comment,
                                            Input.HasGuest, Input.GuestFirstName, Input.GuestLastName);
            return RedirectToPage("/Attendance/Finished");
        }
    }
    public class InputGuestEdit
    {
        [Required]
        public string FirsName { get; set; }
        [Required]
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        [Required]
        public bool IsComing { get; set; }
        public string Comment { get; set; }
        public bool CanHaveGuest { get ;private  set; }
        public bool HasGuest { get; set; }
        public string GuestFirstName { get; set; }
        public string GuestLastName { get; set; }
        public static InputGuestEdit From(RsvpGuest guest)
        {
            InputGuestEdit result = new InputGuestEdit();
            result.FirsName = guest.FirsName;
            result.LastName = guest.LastName;
            if(guest.IsComing.HasValue)
                result.IsComing = guest.IsComing.Value;
            result.PhoneNumber = guest.PhoneNumber;
            result.Comment = guest.Comment;
            result.CanHaveGuest = guest.CanHaveGuest;
            if(guest.HasGuest.HasValue)
                result.HasGuest = guest.HasGuest.Value;
            result.GuestFirstName = guest.GuestFirstName;
            result.GuestLastName = guest.GuestLastName;
            return result;

        }
    }
}
