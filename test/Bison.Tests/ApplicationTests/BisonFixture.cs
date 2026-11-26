using NUnit.Framework;
using Microsoft.AspNetCore.Builder;

namespace Bison.Tests.ApplicationTests;

[SetUpFixture]
public class BisonFixture
{
    private WebApplication? _app;

    public static string BaseUrl { get; } = "http://localhost:5274";

    [OneTimeSetUp]
    public async Task StartApplication()
    {

    }

    [OneTimeTearDown]
    public async Task StopApplication()
    {
        if (_app != null)
        {
            await _app.StopAsync();
            await _app.DisposeAsync();
        }
    }

}