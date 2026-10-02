using System.Net;
using System.Net.Http.Json;
using Bison.Testing;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace Bison.Razor.Testing;

public class BisonRazorTesting : IClassFixture<ApiFixture>
{
    private readonly HttpClient client;

    public BisonRazorTesting(ApiFixture fixture)
    {
        client = fixture.CreateClient();
    }

    [Theory]
    [InlineData("/")]
    public async Task EndpointReturnSuccess(string url)
    {
        var response = await client.GetAsync(url);
        var body = await response.Content.ReadAsStringAsync();

        Assert.True(response.IsSuccessStatusCode, body);
    }

}