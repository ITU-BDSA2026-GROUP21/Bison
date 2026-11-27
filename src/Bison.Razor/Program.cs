using Microsoft.EntityFrameworkCore;
using Bison.Core.Models;
using Bison.Core;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.FileProviders;

public class Program
{
    public static async Task Main(string[] args)
    {
        var app = await CreateApplication(args);
        await app.RunAsync();
    }

    public static async Task<WebApplication> CreateApplication(
        string[] args,
        WebApplicationOptions? options = null,
        Dictionary<string, string?>? configuration = null)
    {
        var builder = WebApplication.CreateBuilder(options ?? new WebApplicationOptions { Args = args });

        if (configuration != null)
        {
            builder.Configuration.AddInMemoryCollection(configuration);
        }

        string connectionString;
        string imageDirectoryString;

        // If the application is running on our Azure site, the connectionString will be set to be in /home/data,
        // so we can keep data permanently in a predictable directory on the server.
        // If running locally on our machine, it will just follow the DefaultConnection value from appsettings.json,
        // which will create it in the directory where the application is running.
        // TODO: Change this so it's primarily handled in appsettings.json!
        if (builder.Environment.IsProduction() && !string.IsNullOrEmpty(Environment.GetEnvironmentVariable("WEBSITE_SITE_NAME")))
        {
            var dbDirectory = "/home/data";
            imageDirectoryString = "/home/data/uploads";

            Directory.CreateDirectory(dbDirectory);

            connectionString = $"Data Source={Path.Combine(dbDirectory, "Bison.db")}";
        }
        else
        {
            connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("DefaultConnection is not configured.");

            imageDirectoryString = builder.Configuration.GetConnectionString("ImagePath")
                ?? throw new InvalidOperationException("Image path is not configured");

        }

        // Normalize imageDirectoryString so the static file middleware gets the entire path on both local machines and on azure.
        imageDirectoryString = Path.GetFullPath(imageDirectoryString);

        // Then create the actual directory where the uploaded images will be stored.
        Directory.CreateDirectory(imageDirectoryString);

        // Add BisonDBContext to the container.
        builder.Services.AddDbContext<BisonDBContext>(options => options.UseSqlite(connectionString));

        // Add Identity services to the container and specify their options.
        builder.Services.AddDefaultIdentity<Author>(options =>
        {
            options.SignIn.RequireConfirmedAccount = true;
            options.Password.RequiredLength = 6;
            options.Password.RequireDigit = false;
            options.Password.RequireUppercase = false;
            options.Password.RequireLowercase = false;
            options.Password.RequireNonAlphanumeric = false;
            options.User.RequireUniqueEmail = true;
        }).AddEntityFrameworkStores<BisonDBContext>();

        if (!builder.Environment.IsEnvironment("Testing")) // TODO: If we decide to test github logins, this if-statement should be removed.
        {
            builder.Services.AddAuthentication()
                .AddGitHub(options =>
                {
                    options.ClientId = builder.Configuration["authentication:github:clientId"];
                    options.ClientSecret = builder.Configuration["authentication:github:clientSecret"];
                    options.CallbackPath = "/signin-github";
                });
        }

        builder.Services.ConfigureApplicationCookie(options =>
        {
            options.AccessDeniedPath = "/Identity/Account/Login";
            options.ExpireTimeSpan = TimeSpan.FromHours(24);
            options.SlidingExpiration = true;
        });

        // Add pages to the container.
        builder.Services.AddRazorPages();

        // Add custom services to the container.
        // The old DBFacade and old observation service have been removed and replaced with the services below.
        builder.Services.AddScoped<IObservationRepository, ObservationRepository>();
        builder.Services.AddScoped<IObservationService, ObservationService>();
        builder.Services.AddScoped<ITaxonRepository, TaxonRepository>();
        builder.Services.AddScoped<ITaxonFilterService, TaxonFilterService>();
        builder.Services.AddScoped<IAuthorRepository, AuthorRepository>();
        builder.Services.AddScoped<IAuthorService, AuthorService>();
        builder.Services.AddScoped<IPhotoService>(sp => new PhotoService(imageDirectoryString)); // Using a lambda function to explicitly tell the service provicer what service to use.

        builder.Services.AddDistributedMemoryCache(); // where session data is stored
        builder.Services.AddSession();

        var app = builder.Build();

        // DB Migrating and seeding, so a fresh copy is created if no Bison.db exists. Bison.db is no longer tracked by git, 
        // so for a fresh clone on a macbine, this will ensure that the file exists. The seeding will only actually happen when 
        // the database is empty of both Authors and Posts, and Taxons. 
        // Eduard and Debasmita's users are added separately with a check, as if we used the same procedure as in DbInitializer, they wouldn't be created properly, and their
        // passwords would be stored without hashing.
        using (var scope = app.Services.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<BisonDBContext>();
            context.Database.Migrate();
            DbInitializer.SeedDatabase(context);

            // The rest of the scope is for creating Eduard and Debasmita's users, if they don't already exist in the database.
            // Not done during testing.
            // TODO: Should maybe be extracted to somewhere else, maybe the DbInitializer
            // BUG: If secrets aren't set properly, will throw an ArgumentNullException
            if (!app.Environment.IsEnvironment("Testing"))
            {
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<Author>>();

                // Get Eduard's email secret and check if his user exists
                var eduardEmailString = app.Configuration["Seeding:Eduard:Email"];
                var eduardUser = await userManager.FindByEmailAsync(eduardEmailString);

                // If it doesn't create it in this if-block
                if (eduardUser == null)
                {
                    // Get Eduard's password secret, and attempt to create his user
                    var eduardPasswordString = app.Configuration["Seeding:Eduard:Password"];
                    var result = await userManager.CreateAsync(new Author { Email = eduardEmailString, UserName = "Eduard", EmailConfirmed = true }, eduardPasswordString);

                    // If user creation failed, throw an exception.
                    // TODO: Error logging and handling should be more explicit
                    if (!result.Succeeded)
                    {
                        throw new Exception("Something went wrong when creating Eduard's user!");
                    }
                }

                // Same procedure for Debasmita's user
                var debasmitaEmailString = app.Configuration["Seeding:Debasmita:Email"];
                var debasmitaUser = await userManager.FindByEmailAsync(debasmitaEmailString);

                if (debasmitaUser == null)
                {
                    var debasmitaPasswordString = app.Configuration["Seeding:Debasmita:Password"];
                    var result = await userManager.CreateAsync(new Author { Email = debasmitaEmailString, UserName = "Debasmita", EmailConfirmed = true }, debasmitaPasswordString);

                    if (!result.Succeeded)
                    {
                        throw new Exception("Something went wrong when creating Debasmita's user!");
                    }
                }
            }
        }

        if (!app.Environment.IsEnvironment("Testing"))
        {
            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
        }

        app.UseStaticFiles();

        // Add an additional static file middleware for serving the uploaded observation images. This essentialy handles what an image serving method in PhotoService would do.
        app.UseStaticFiles(new StaticFileOptions
        {
            FileProvider = new PhysicalFileProvider(imageDirectoryString), // Define the actual file path for finding images
            RequestPath = "/photos", // Define the endpoint we'll actually use on the pages to access the images. 
                                     // Doing it this way makes the pages not care whether it's running locally, using wwwroot/uploads, or the path we use in Azure.
            OnPrepareResponse = context =>
            {
                context.Context.Response.Headers.XContentTypeOptions = "nosniff"; // Add a security header that tells browsers not to guess the file type, 
                                                                                  // but to respect the declared MIME type. In our case that's "image/<the specific image type>"
            }
        });

        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseSession();

        app.MapRazorPages();
        //Redirecting the load up page to observations page (Public.cshtml)
        app.MapGet("/", () => Results.Redirect("/obs"));

        return app;
    }
}