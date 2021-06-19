namespace HotelListing
{
    using AspNetCoreRateLimit;
    using HotelListing.Configurations;
    using HotelListing.Contexts;
    using HotelListing.Interfaces.Managers;
    using HotelListing.Interfaces.Repositories;
    using HotelListing.Managers;
    using HotelListing.Repositories;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.OpenApi.Models;
    using Newtonsoft.Json;
    public class Startup
    {
        const int CACHE_DURATION_IN_SECONDS = 120;
        const string CORS_POLICY = "AllowAll";
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration) => this.Configuration = configuration;

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers(config =>
            {
                config.CacheProfiles.Add("CACHE_DURATION",
                    new CacheProfile
                    {
                        Duration = CACHE_DURATION_IN_SECONDS
                    });
            }).AddNewtonsoftJson(json =>
            {
                json.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            });
            services.AddDbContext<HotelListingContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("Default"));
            });
            services.AddCors(policy =>
            {
                policy.AddPolicy(CORS_POLICY, builder =>
                {
                    builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
                });
            });
            services.AddAuthentication();
            services.AddMemoryCache();

            services.AddRateLimiting();
            services.AddHttpContextAccessor();

            services.ConfigureHttpCacheHeaders();
            services.ConfigureIdentity();
            services.ConfigureJwt(Configuration);
            services.ConfigureVersioning();

            services.AddAutoMapper(typeof(MapperInitializationConfiguration));

            services.AddScoped<IAuthManager, AuthManager>();
            services.AddTransient<IUnitOfWorkRepository, UnitOfWorkRepository>();

            services.AddSwaggerGen(swagger =>
            {
                swagger.SwaggerDoc("v1", new OpenApiInfo { Title = "HotelListing", Version = "v1" });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            string SWAGGER_NAME = "HotelListing v1";

            app.UseSwagger();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwaggerUI(swagger => swagger.SwaggerEndpoint("/swagger/v1/swagger.json", SWAGGER_NAME));
            }
            else
            {
                app.UseSwaggerUI(swagger =>
                {
                    string path = string.IsNullOrWhiteSpace(swagger.RoutePrefix) ? "." : "..";
                    swagger.SwaggerEndpoint($"{path}/swagger/v1/swagger.json", SWAGGER_NAME);
                });
            }

            app.AddGlobalErrorHandling();
            app.UseHttpsRedirection();
            app.UseCors(CORS_POLICY);
            app.UseRouting();
            app.UseResponseCaching();
            app.UseHttpCacheHeaders();

            app.UseIpRateLimiting();

            app.UseAuthorization();
            app.UseAuthentication();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
