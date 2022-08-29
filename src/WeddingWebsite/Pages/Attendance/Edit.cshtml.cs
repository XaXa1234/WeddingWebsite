using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace WeddingWebsite.Pages.Attendance
{
    public class EditModel : PageModel
    {
        public EditModel(IRsvc)
        [BindProperty]
        public InputGuestEdit Input { get; set; }
        public void OnGet(string u)
        {
            var emailDecoded = 
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
        public bool? IsComing { get; set; }
        public string Comment { get; set; }
        public bool CanHaveGuest { get; set; }
        public bool? HasGuest { get; set; }
        public string GuestFirstName { get; set; }
        public string GuestLastName { get; set; }
    }
}
