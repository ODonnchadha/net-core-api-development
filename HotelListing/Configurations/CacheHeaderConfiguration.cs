using Marvin.Cache.Headers;
using Microsoft.Extensions.DependencyInjection;

namespace HotelListing.Configurations
{
    public static class CacheHeaderConfiguration
    {
        public static void ConfigureHttpCacheHeaders(this IServiceCollection services)
        {
            services.AddResponseCaching();
            services.AddHttpCacheHeaders((expiration) =>
            {
                expiration.MaxAge = 65;
                expiration.CacheLocation = CacheLocation.Private;
            }, (validation) =>
            {
                validation.MustRevalidate = true;
            });
        }
    }
}
