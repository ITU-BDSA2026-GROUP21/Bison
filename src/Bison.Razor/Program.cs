using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

string? connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<BisonDBContext>(options => options.UseSqlite(connectionString));

//Commandline for running program after introducing enviromental varible: 
//BISONDBPATH=../../data/sqlite/tmp/bison.db dotnet run 
//(This upholds from Bison.Razor directory, it changes depending on which diretory user is in)
var dbPath = Environment.GetEnvironmentVariable("BISONDBPATH")
             ?? Path.Combine(Path.GetTempPath(), "bison.db");

// Add services to the container.
builder.Services.AddRazorPages();

// The two services below are the old ones and should not be added when the pages are ready to move over to EF Core
builder.Services.AddSingleton<IDBFacade>(new DBFacade(dbPath));
builder.Services.AddSingleton<OldIObservationService, OldObservationService>();

// These services are the ones that should be used in the future. They are the ones that rely on EF Core.
builder.Services.AddScoped<IObservationRepository, ObservationRepository>();

var app = builder.Build();

// DB Migrating and seeding, so a fresh copy is created if no Bison.db exists. Bison.db is no longer tracked by git, so for new
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

app.MapRazorPages();
//Redirecting the load up page to observations page (Public.cshtml)
app.MapGet("/", () => Results.Redirect("/obs"));

app.Run();
