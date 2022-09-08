using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Localization.Routing;
using Microsoft.Extensions.Localization;
using System.Globalization;

namespace WeddingWebsite.Extensions
{
    public static class LocalizationExtensions
    {
        public static void AddMyLocalization(this IServiceCollection services, IConfiguration config)
        {
            services.AddLocalization(opt => { opt.ResourcesPath = "Resources"; });
            services.Configure<RequestLocalizationOptions>(options =>
            {
                List<CultureInfo> supportedCultures = new List<CultureInfo>
                                    {
                                        new CultureInfo("fr"),
                                        //new CultureInfo("fr-FR"),
                                        new CultureInfo("en-GB")
                                    };
                options.DefaultRequestCulture = new RequestCulture("en-GB");
                options.SupportedCultures = new List<CultureInfo>() { new CultureInfo("en-GB") };
                options.SupportedUICultures = supportedCultures;
                options.FallBackToParentUICultures = true;
                options.RequestCultureProviders.Insert(0, new RouteDataRequestCultureProvider { Options = options });
                //var requestProvider = options.RequestCultureProviders.OfType<AcceptLanguageHeaderRequestCultureProvider>().First();
                //options.RequestCultureProviders.Remove(requestProvider);
            });
        }
    }
}
