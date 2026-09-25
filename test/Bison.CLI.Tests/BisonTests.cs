using SimpleDB;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace Bison.CLI.Tests;

// It may be necessary later to tell xUnit to not run these tests in parallel, because of Console.SetOut()
public class BisonTests
{
    string observationPath = Path.Combine(AppContext.BaseDirectory, "bison_observe_cli_db_test.csv");

    string commentPath = Path.Combine(AppContext.BaseDirectory, "bison_comment_cli_db_test.csv");

    [Fact]
    public async Task NonExistentObservationID()
    {
        // Arrange
        var baseURL = "http://localhost:5004";
        using HttpClient client = new();

        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        client.BaseAddress = new Uri(baseURL);

        bool argumentThrown = false;

        // Act
        try
        {
            await UserInterface.comment(client, 9999999, "Test Comment");
        }
        catch (ArgumentException e)
        {
            Console.WriteLine(e.Message);
            argumentThrown = true;
        }

        // Assert
        Assert.True(argumentThrown, "Should throw an ArgumentException");
    }

    [Fact]
    public void ObservationPrinting()
    {
        var originalOut = Console.Out;
        using var writer = new StringWriter();
        try
        {
            // Arrange
            CSVDatabase<ObservationRecord> observationDatabase = CSVDatabase<ObservationRecord>.getInstance(observationPath);
            Console.SetOut(writer);

            // Act
            UserInterface.PrintObservations(observationDatabase.Read(1), "DR Byen");

            string output = writer.ToString();

            // Assert
            Assert.Equal("Location: DR Byen" + Environment.NewLine + "ropf @ 08/01/23 12:09:20: A bird at DR Byen" + Environment.NewLine, output);
        }
        finally
        {
            Console.SetOut(originalOut);
        }
    }

    [Fact]
    public void CommentPrinting()
    {
        var originalOut = Console.Out;
        using var writer = new StringWriter();
        try
        {
            // Arrange
            CSVDatabase<ObservationRecord> observationDatabase = CSVDatabase<ObservationRecord>.getInstance(observationPath);
            CSVDatabase<CommentRecord> commentDatabase = CSVDatabase<CommentRecord>.getInstance(commentPath);
            Console.SetOut(writer);

            // Act
            UserInterface.PrintComments(commentDatabase.Read(), observationDatabase.Read(), 0);

            string output = writer.ToString();

            // Assert
            Assert.Equal("Location: DR Byen" + Environment.NewLine +
                         "ropf @ 08/01/23 12:09:20: A bird at DR Byen" + Environment.NewLine +
                         "- raap @ 08/01/23 12:09:20: A bird indeed" + Environment.NewLine, output);
        }
        finally
        {
            Console.SetOut(originalOut);
        }
    }

    [Fact]
    public async Task EndToEndRead()
    {
        var originalOut = Console.Out;
        using var writer = new StringWriter();

        var baseURL = "http://localhost:5004";
        using HttpClient client = new();

        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        client.BaseAddress = new Uri(baseURL);

        try
        {
            string[] args = ["read", "DR Byen"];

            Console.SetOut(writer);
            await UserInterface.run(args, client);

            string output = writer.ToString();

            //Asserts that the observation posted by ropf is present
            Assert.Contains("ropf @ 08/01/23 12:09:20: A bird at DR Byen", output);

            var locationLines = output.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries).Where(l => l.StartsWith("Location: "));

            //Checks that the list of observation at the location is not empty and that all locations is DR Byen
            Assert.NotEmpty(locationLines);
            foreach (var l in locationLines)
            {
                Assert.StartsWith("Location: DR Byen", l, StringComparison.OrdinalIgnoreCase);
            }

            //This one compares the expected if only one observation har been done at the location
            //Where as one more than one observation is done in the same place the output will not be equal to the expected
            /*Console.WriteLine(output);
            Assert.Equal(
                "Location: DR Byen" + Environment.NewLine
                + "ropf @ 08/01/23 12:09:20: A bird at DR Byen" + Environment.NewLine,
                output);*/
        }
        finally
        {
            Console.SetOut(originalOut);
        }
    }

    [Fact]
    public async Task EndToEndObserve()
    {
        //Arrange
        var baseURL = "http://localhost:5004";
        using HttpClient client = new();

        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        client.BaseAddress = new Uri(baseURL);

        var beforeAct = await client.GetFromJsonAsync<List<ObservationRecord>>("observations");
        int beforeActID = beforeAct.Last().ID;

        string[] args = ["observe", "Penguin", "Copenhagen"];

        //ObservationRecord last = observationDatabase.Read().Last();
        ObservationRecord expected = new ObservationRecord
        {
            Author = Environment.UserName,
            Observation = "Penguin",
            Timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
            ID = ++beforeActID,
            Location = "Copenhagen"
        };

        //Act
        await UserInterface.run(args, client);

        //Assert
        var afterAct = await client.GetFromJsonAsync<List<ObservationRecord>>("observations");
        int afterActID = afterAct.Last().ID;

        //Need compare every aspect as the expected is a record and afterAct is json
        Assert.Equal(afterAct.Last().Author, expected.Author);
        Assert.Equal(afterAct.Last().Observation, expected.Observation);
        Assert.Equal(afterAct.Last().Timestamp, expected.Timestamp);
        Assert.Equal(afterAct.Last().ID, expected.ID);
        Assert.Equal(afterAct.Last().Location, expected.Location);
    }

}
