using AspNetCoreRateLimit;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;

namespace HotelListing.Configurations
{
    public static class RateLimitMemoryConfiguration
    {
        public static void AddRateLimiting(this IServiceCollection services)
        {
            services.Configure<IpRateLimitOptions>(options =>
            {
                options.GeneralRules = new List<RateLimitRule> 
                { 
                    new RateLimitRule 
                    { 
                        Endpoint = "*", 
                        Limit = 1, 
                        Period = "1s" 
                    } 
                };
            });

            services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
            services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
            services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
        }
    }
}
