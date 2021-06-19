using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.DependencyInjection;

namespace HotelListing.Configurations
{
    public static class VersioningConfiguration
    {
        public static void ConfigureVersioning(this IServiceCollection services)
        {
            services.AddApiVersioning(versioning =>
            {
                versioning.ReportApiVersions = true;
                versioning.AssumeDefaultVersionWhenUnspecified = true;
                versioning.DefaultApiVersion = new ApiVersion(1, 0);
                versioning.ApiVersionReader = new HeaderApiVersionReader("api-version");
            });
        }
    }
}
