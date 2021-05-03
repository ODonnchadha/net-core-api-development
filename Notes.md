## Ultimate ASP.NET Core 5 Web API Development Guide
- INTRODUCTION:
  - Software interface that acts as an intermediary allowing two application to talk to each other.
  - REST and associated best practices. Use .NET Core 5. Use Enterprise Patterns.

- ENVIRONMENT CONFIGURATION:
  - Installation of: Visual Studio Community.
    - https://dotnet.microsoft.com/download/dotnet/5.0
    - (1) .NET 5.x: SDK 5.0.202 and the (2) ASP.NET Core Runtime 5.0.5 hosting bundle.
  - GitHub account.
  - Postman.

- PROJECT SETUP & CONFIGURATIONS:
  - Swagger: Via package Swashbuckle.AspNetCore
  ```csharp
    services.AddSwaggerGen(c =>
    {
      c.SwaggerDoc("v1", new OpenApiInfo { Title = "HotelListing", Version = "v1" });
    });
    if (env.IsDevelopment())
    {
      app.UseSwagger();
      app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "HotelListing v1"));
    }
  ```
  - CORS: Cross-origin resource sharing.
  ```csharp
    services.AddCors(policy =>
    {
      policy.AddPolicy(CORS_POLICY, builder =>
      {
        builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
      });
    });
    app.UseCors(CORS_POLICY);
  ```
  - GITHUB: