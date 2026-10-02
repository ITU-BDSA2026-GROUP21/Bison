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
builder.Services.AddSingleton<IDBFacade>(new DBFacade(dbPath));
builder.Services.AddSingleton<IObservationService, ObservationService>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<BisonDBContext>();
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
