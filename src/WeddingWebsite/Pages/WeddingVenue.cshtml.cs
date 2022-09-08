using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WeddingWebsite.Db;
using WeddingWebsite.Models;

namespace WeddingWebsite.Pages
{
    public class WeddingVenueModel : PageModel
    {
        private readonly IApplicationDbContext applicationDbContext;

        public WeddingVenueModel(IApplicationDbContext applicationDbContext)
        {
            this.applicationDbContext = applicationDbContext;
        }

        public IEnumerable<PlaceToSleep> PlaceToSleeps {  get;set;}

        public void OnGet()
        {
            PlaceToSleeps = this.applicationDbContext.PlaceToSleeps.ToList();
        }
    }
}
