using SimpleDB;
using System.Net;
using System.Net.Http.Json;

namespace Bison.CSVDBService.Tests;

public class CSVDBServiceTests : IClassFixture<ApiFixture>
{

    private readonly HttpClient client;

    public CSVDBServiceTests(ApiFixture fixture)
    {
        client = fixture.CreateClient();
    }

    [Fact]
    public async Task ObservationsGet()
    {
        HttpResponseMessage response = await client.GetAsync("/observations");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        Assert.NotNull(response.Content);
    }

    [Fact]
    public async Task ObservationsPost()
    {
        //Arrange
        List<ObservationRecord> observations1 = await client.GetFromJsonAsync<List<ObservationRecord>>("/observations");
        var size1 = observations1.Count;

        //Act
        var observation = new Observation("Test", "ITU");
        HttpResponseMessage response = await client.PostAsJsonAsync("/observation", observation);

        //Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        List<ObservationRecord> observations2 = await client.GetFromJsonAsync<List<ObservationRecord>>("/observations");
        var size2 = observations2.Count;

        //Assert
        Assert.NotEqual(size1, size2);
    }
}