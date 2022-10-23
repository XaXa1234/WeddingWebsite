using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WeddingWebsite.Db;
using WeddingWebsite.Models;

namespace WeddingWebsite.Pages
{
    public class WeddingVenueModel : PageModel
    {
        private readonly IApplicationDbContext applicationDbContext;
        private readonly IHttpContextAccessor httpContextAccessor;

        public string RequestedCulture { get; set; }
        public WeddingVenueModel(IApplicationDbContext applicationDbContext,IHttpContextAccessor httpContextAccessor)
        {
            this.applicationDbContext = applicationDbContext;
            this.httpContextAccessor = httpContextAccessor;
            
        }

        public IEnumerable<PlaceToSleep> PlaceToSleeps {  get;set;}

        public void OnGet()
        {
            var requestCulture = httpContextAccessor.HttpContext.Features.Get<IRequestCultureFeature>();
            RequestedCulture = requestCulture.RequestCulture.UICulture.Name;
            PlaceToSleeps = this.applicationDbContext.PlaceToSleeps.ToList();
        }
    }
}
