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

}
