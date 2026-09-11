using SimpleDB;

namespace Bison.CLI.Tests;

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
        // Arrange
        CSVDatabase<ObservationRecord> observationDatabase = CSVDatabase<ObservationRecord>.getInstance(observationPath);
        using var writer = new StringWriter();
        Console.SetOut(writer);

        // Act
        UserInterface.PrintObservations(observationDatabase.Read(1));

        string output = writer.ToString();

        // Assert
        Assert.Equal("ropf @ 08/01/23 12:09:20: A bird at DR Byen" + Environment.NewLine, output);
    }

    [Fact]
    public void CommentPrinting()
    {
        // Arrange


        // Act

        // Assert
    }


    // Below here may be useful for integration tests
    /*
        public CSVDatabase<ObservationRecord> CreateTestObservationDatabase()
        {
            //Copy base CSV, and create temporary test database and CSV file

            CSVDatabase<ObservationRecord> old = new CSVDatabase<ObservationRecord>("../../testdata/bison_observe_cli_db_test.csv");

            File.Create("../../testdata/test.csv");



            //Return new Database
        }

        public CSVDatabase<ObservationRecord> CreateTestCommentDatabase()
        {
            //Copy base CSV, and create temporary test database and CSV file

            //Return new Database
        }

        public void DeleteCSV(string filePath1)
        {
            try
            {
                // Try to delete CSV
            }
            catch
            {
                // Catch exception, then continue
            }
        }

        public void Dispose()
        {
            DeleteCSV("../../testdata/test.csv");
            DeleteCSV("../../testdata/test2.csv");
        }*/
}
