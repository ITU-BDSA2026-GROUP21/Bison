using SimpleDB;

namespace Bison.CLI.Tests;

public class BisonTests
{
    [Fact]
    public void NonExistentObservationID()
    {
        // Arrange

        // Act

        // Assert

    }


    // Below here may be useful for integration tests

    /*public CSVDatabase<ObservationRecord> CreateTestObservationDatabase()
    {
        //Copy base CSV, and create temporary test database and CSV file

        CSVDatabase<ObservationRecord> old = new CSVDatabase<ObservationRecord>("../../testdata/bison_comment_cli_db_test.csv");

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
    } */
}