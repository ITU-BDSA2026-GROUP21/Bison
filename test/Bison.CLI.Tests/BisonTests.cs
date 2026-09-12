using SimpleDB;

namespace Bison.CLI.Tests;

// It may be necessary later to tell xUnit to not run these tests in parallel, because of Console.SetOut()
public class BisonTests
{
    string observationPath = Path.Combine(AppContext.BaseDirectory, "bison_observe_cli_db_test.csv");

    string commentPath = Path.Combine(AppContext.BaseDirectory, "bison_comment_cli_db_test.csv");

    [Fact]
    public void NonExistentObservationID()
    {
        // Arrange
        CSVDatabase<ObservationRecord> observationDatabase = CSVDatabase<ObservationRecord>.getInstance(observationPath);

        CSVDatabase<CommentRecord> commentDatabase = CSVDatabase<CommentRecord>.getInstance(commentPath);

        bool argumentThrown = false;

        // Act
        try
        {
            UserInterface.comment(observationDatabase, commentDatabase, 9999999, "Test Comment");
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
            UserInterface.PrintObservations(observationDatabase.Read(1));

            string output = writer.ToString();

            // Assert
            Assert.Equal("ropf @ 08/01/23 12:09:20: A bird at DR Byen" + Environment.NewLine, output);
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
            Assert.Equal("ropf @ 08/01/23 12:09:20: A bird at DR Byen" + Environment.NewLine +
                         "- raap @ 08/01/23 12:09:20: A bird indeed" + Environment.NewLine, output);
        }
        finally
        {
            Console.SetOut(originalOut);
        }
    }


    [Fact]
    public void EndToEndRead()
    {
        var originalOut = Console.Out;
        using var writer = new StringWriter();
        try
        {
            IDatabaseRepository<ObservationRecord> observationDatabase = CSVDatabase<ObservationRecord>.getInstance(observationPath);
            IDatabaseRepository<CommentRecord> commentDatabase = CSVDatabase<CommentRecord>.getInstance(commentPath);

            string[] args = ["read"];

            Console.SetOut(writer);

            UserInterface.run(args, observationDatabase, commentDatabase);

            string output = writer.ToString();

            Assert.Equal("ropf @ 08/01/23 12:09:20: A bird at DR Byen" + Environment.NewLine, output);
        }
        finally
        {
            Console.SetOut(originalOut);
        }
    }

    [Fact]
    public void EndToEndObserve()
    {
        IDatabaseRepository<ObservationRecord> observationDatabase = CSVDatabase<ObservationRecord>.getInstance(observationPath);
        IDatabaseRepository<CommentRecord> commentDatabase = CSVDatabase<CommentRecord>.getInstance(commentPath);

        string[] args = ["observe", "Penguin"];

        ObservationRecord last = observationDatabase.Read().Last();
        ObservationRecord expected = new ObservationRecord { Author = Environment.UserName, Observation = "Penguin", Timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds(), ID = ++last.ID };

        UserInterface.run(args, observationDatabase, commentDatabase);

        var actual = observationDatabase.Read().Last();
        Assert.Equal(expected.ToString(), actual.ToString());
    }
}
