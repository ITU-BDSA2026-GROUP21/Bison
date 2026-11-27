using NUnit.Framework;
using Microsoft.AspNetCore.Builder;

namespace Bison.Tests.ApplicationTests;

[SetUpFixture]
public class BisonFixture
{
    private WebApplication? _app;
    private string? _testDirectory;

    public static string BaseUrl { get; } = "http://localhost:5274";

    [OneTimeSetUp]
    public async Task StartApplication()
    {
        // Create a temporary directory for test data, with a random name.
        _testDirectory = Path.Combine(
            Path.GetTempPath(),
            $"BisonTests-{Guid.NewGuid()}");

        Directory.CreateDirectory(_testDirectory);

        // Define paths for the db file and image directory
        var databasePath = Path.Combine(_testDirectory, "Bison.db");
        var imagePath = Path.Combine(_testDirectory, "uploads");

        // Find the path to Bison.Razor, since we could possibly need the static files located within in some E2E tests.
        var razorProjectPath = FindRazorProjectPath();

        // Define testing configuration
        var options = new WebApplicationOptions
        {
            EnvironmentName = "Testing",
            ApplicationName = typeof(Program).Assembly.GetName().Name,
            ContentRootPath = razorProjectPath
        };

        // Override the standard configuration with new values for testing.
        var configuration = new Dictionary<string, string?>
        {
            // Explicitly disable connection pooling so the db file can be properly deleted afterwards
            ["ConnectionStrings:DefaultConnection"] = $"Data Source={databasePath};Pooling=False",
            ["ConnectionStrings:ImagePath"] = imagePath,
            ["urls"] = BaseUrl
        };

        // Create the actual application with the new options and configuration
        _app = await Program.CreateApplication(
            Array.Empty<string>(),
            options,
            configuration
        );

        // Start the application:
        await _app.StartAsync();
    }

    [OneTimeTearDown]
    public async Task StopApplication()
    {
        // If the app exists, stop it and dispose it.
        if (_app != null)
        {
            await _app.StopAsync();
            await _app.DisposeAsync();
        }

        // If the test directory is not null, and it exists at the expected location, delete it.
        if (_testDirectory != null &&
            Directory.Exists(_testDirectory))
        {
            Directory.Delete(_testDirectory, recursive: true);
        }
    }

    // Private method for getting the path for Bison.Razor
    private static string FindRazorProjectPath()
    {
        // Get current base directory
        DirectoryInfo? directory = new DirectoryInfo(AppContext.BaseDirectory);

        // Check if current directory contains "src/Bison.Razor"
        // If not, go one step up, and try again, until we can't anymore
        while (directory != null)
        {
            var projectCandidate = Path.Combine(directory.FullName, "src", "Bison.Razor");

            if (File.Exists(Path.Combine(projectCandidate, "Bison.Razor.csproj")))
            {
                return projectCandidate;
            }

            directory = directory.Parent;
        }

        throw new DirectoryNotFoundException("Bison.Razor could not be found!");
    }

}