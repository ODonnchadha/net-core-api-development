## Ultimate ASP.NET Core 5 Web API Development Guide
- Udemy: Learn how to create a maintainable Web API using ASP.NET Core 5, Entity Framework and Enterprise Level Design Patterns.
- Trevoir Williams

- INTRODUCTION:
  - API: A software interface that acts as an intermediary allowing two applications to talk to each other.
  - REST and associated best practices using .NET Core 5 and Enterprise Patterns.

- ENVIRONMENT CONFIGURATION:
  - e.g.: Installation of: Visual Studio Community.
    - [.NET 5.x: SDK 5.0.202](https://dotnet.microsoft.com/download/dotnet/5.0).
    - [ASP.NET Core Runtime 5.0.5 hosting bundle](https://dotnet.microsoft.com/download/dotnet/5.0).
  - GitHub account.
  - Postman.

- PROJECT SETUP & CONFIGURATIONS:
  - Swagger: Via package 'Swashbuckle.AspNetCore.'
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
  ```javascript
    setx KEY "f5df0c58-931b-47EB-A853-5818890f863c" /M
  ```
  ```csharp
    var key = Environment.GetEnvironmentVariable("KEY");
  ```
  ```csharp
      services.ConfigureJwt(Configuration);
      
      app.UseAuthorization();
      app.UseAuthentication();
  ```

- VALUE-ADDED FEATURES:
  - Paging: Segmenting the data that is to be returned. e.g.: ?pageSize=1&pageNumber=3
  ```csharp
    using X.PagedList;
  ```
  - Implement Global exceptions:

  - Implement API versioning:
    - Ensure same route:
      - [ApiController(), Route("api/[controller]")]
      - [ApiController(), Route("api/Countries")]

      - Within header: api-supported-versions: 1.0
        - 1.)
          - https://localhost/api/Countries/?api-version=2.0
        - 2.)
          - [ApiController(), Route("api/{v:apiversion}/Countries")]
          - https://localhost/api/2.0/Countries
        - 3.)
          ```csharp
            services.AddApiVersioning(versioning =>
            {
                versioning.ApiVersionReader = new HeaderApiVersionReader("api-version");
            });
          ```
    - Depricated:
      - [ApiVersion("2.0", Deprecated = true)]
      - e.g.: Within header: api-deprecated-versions: 2.0
  
  - Caching:
    - Client Cache. Lives on the browser.
    - Gateway Cache. On the server. And shared.
    - Proxy cache. On the network. Shared.

    - [ResponseCache(Duration = 60)]
    - Client setting: Ensure no-cache header is off.
    - Upon response: 'Cache-Control: public, max-age=60.' The client will know that the data is cached.
    ```csharp
      services.AddResponseCaching();
      app.UseResponseCaching();
    ```
    - Header: age: 9, e.g.:

    ```csharp
      services.AddControllers(config => 
      {
        config.CacheProfiles.Add("CACHE_DURATION_CONFIGURATION",
          new CacheProfile
          {
            Duration = CACHE_DURATION_IN_SECONDS
          });
    ```
    - [ResponseCache(CacheProfileName = "CACHE_DURATION_CONFIGURATION")]
    - What about "stale" data? Add validation.
    - Addition of library: Marvin.Cache.Headers
    - Adding to headers: Expires, Last-Modified, ETag, etc...
    ```csharp
      public static void ConfigureHttpCacheHeaders(this IServiceCollection services)
    ```
    - This now becomes global. Can be overridden:
    - [HttpCacheExpiration(CacheLocation = CacheLocation.Public, MaxAge = 60)]
    - [HttpCacheValidation(MustValidate = false)]

  - Rate Limiting and Throttling:
    - Headers: X-Rate-Limit, et al.

- HOSTING THE API ON IIS (Internet Information Services:)
  - Set up the environment for a "local" deployment. i.e. Personal machine.
  - DOWNLOAD: ASP.NET Core Runtime 5.0.2 Windows Hosting Bundle.
  - DOWNLOAD: Recommended db Express. Server name: .\SQLEXPRESS or SQL Developer. Server name: localhost.
  - Roadblocks: Permission and authorization. Administrative rights.

  - Publish the site to IIS.
  - Considerations. Swagger endpoint.
  - Publish application to local folder and then manually move. Use settings to modify ConnectionString.
  - Obtain the migration script.