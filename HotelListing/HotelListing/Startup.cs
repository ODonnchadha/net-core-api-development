using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace HotelListing
{
    public class Startup
    {
        const string CORS_POLICY = "CORS";
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration) => this.Configuration = configuration;

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddCors(policy =>
            {
                policy.AddPolicy(CORS_POLICY, builder =>
                {
                    builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
                });
            });

            services.AddSwaggerGen(swagger =>
            {
                swagger.SwaggerDoc("v1", new OpenApiInfo { Title = "HotelListing", Version = "v1" });
            });
         }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "HotelListing v1"));

            app.UseHttpsRedirection();
            app.UseCors(CORS_POLICY);
            app.UseRouting();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
