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
        builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
      });
    });
    app.UseCors(CORS_POLICY);
  ```
  - GITHUB:

- DATABASE MODELING & REPOSITORY PATTERN
  - NOTE:
    ```csharp
      public class Hotel
      {
          [ForeignKey(nameof(Country))]
          public int CountryId { get; set; }
          public Country Country { get; set; }
      }
    ```
    ```javascript
      Add-Migration INIT
      Update-Database
    ```
  - Seeding:
    ```csharp
      protected override void OnModelCreating(ModelBuilder builder)
      {
          builder.Entity<Country>().HasData(
              new Country { Id = 1, Name = "Jamaica", ShortName = "JM" },
              new Country { Id = 2, Name = "Bahamas", ShortName = "BS" },
              new Country { Id = 3, Name = "Cayman Island", ShortName = "CI" }
          );
      }
    ```
    ```javascript
      Add-Migration SEED
      Update-Database
    ```
  - Service Repositories & Dependency Injection:
    ```csharp
      public interface IGenericRepository<T> where T: class {}
      public interface IUnitOfWorkRepository : IDisposable {}
      public class GenericRepository<T> : IGenericRepository<T> where T : class
      {
          private readonly HotelListingContext context;
          private readonly DbSet<T> db;

          public GenericRepository(HotelListingContext context)
          {
              this.context = context;
              this.db = context.Set<T>();
          }
      }
      public class UnitOfWorkRepository : IUnitOfWorkRepository
      {
          private IGenericRepository<Country> countries;
          private IGenericRepository<Hotel> hotels;
          private readonly HotelListingContext context;

          public UnitOfWorkRepository(HotelListingContext context) => this.context = context;
          public IGenericRepository<Country> Countries => countries ?? new GenericRepository<Country>(context);
          public IGenericRepository<Hotel> Hotels => hotels ?? new GenericRepository<Hotel>(context);
      }
    ```
  - Automapper & DTOs:
    - Data transfer objects with the associated mapping. Enforcing front-end validation, etc...
      - We'll be creating DTOs per operation. e.g.: Read, Create, etc...
      ```csharp
          services.AddAutoMapper(typeof(MapperInitializationConfiguration));
      ```

- GET REQUESTS
  - Configure the 'Get' endpoints. We'll be using attribute routing. e.g.:
    ```csharp
      [Route("api/[controller]")]
      [ApiController()]
      public class CountryController : ControllerBase { }
    ```
  - NOTE:
    ```csharp
      services.AddControllers().AddNewtonsoftJson(json =>
      {
          json.SerializerSettings.ReferenceLoopHandling = 
          Newtonsoft.Json.ReferenceLoopHandling.Ignore;
      });

- SECURING THE API:
  - Security. How do we make an API secure?
    - SSL. TLS. Standard for HTTP communication as they enforce encryption across the wire.
    - Identity Core. Power access system. Authentication and authorization.
    - What is exposed within a URL? Defensive design. Hash. Encrypt. JWT.
  ```csharp
    public class HotelListingContext : IdentityDbContext<User>
    {
      protected override void OnModelCreating(ModelBuilder builder)
      {
        base.OnModelCreating(builder);
      }
    }
  ```
  ```javascript
    Add-Migration IDENTITY
    Update-Database
  ```
  - User roles: We'll need to seed them.
  
  - JWT:
  - https://jwt.io/a
    ```csharp
      services.ConfigureJwt(Configuration);
      
      app.UseAuthorization();
      app.UseAuthentication();
    ```
