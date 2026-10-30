using Microsoft.EntityFrameworkCore;
using Bison.Core.Models;
using Bison.Core;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

string connectionString;

// If the application is running on our Azure site, the connectionString will be set to be in /home/data,
// so we can keep data permanently in a predictable directory on the server.
// If running locally on our machine, it will just follow the DefaultConnection value from appsettings.json,
// which will create it in the directory where the application is running.
if (builder.Environment.IsProduction() && !string.IsNullOrEmpty(Environment.GetEnvironmentVariable("WEBSITE_SITE_NAME")))
{
    var dbDirectory = "/home/data";

    Directory.CreateDirectory(dbDirectory);

    connectionString = $"Data Source={Path.Combine(dbDirectory, "Bison.db")}";
}
else
{
    connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
        ?? throw new InvalidOperationException("DefaultConnection is not configured.");
}

// Add BisonDBContext to the container.
builder.Services.AddDbContext<BisonDBContext>(options => options.UseSqlite(connectionString));

// Add Identity services to the container
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

var app = builder.Build();

// DB Migrating and seeding, so a fresh copy is created if no Bison.db exists. Bison.db is no longer tracked by git, 
// so for a fresh clone on a macbine, this will ensure that the file exists.
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<BisonDBContext>();
    context.Database.Migrate();
    DbInitializer.SeedDatabase(context);
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();
//Redirecting the load up page to observations page (Public.cshtml)
app.MapGet("/", () => Results.Redirect("/obs"));

app.Run();
