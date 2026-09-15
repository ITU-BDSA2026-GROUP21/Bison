namespace SimpleDB.Tests;

public class SimpleDBTests
{

    string observationPath = Path.Combine(AppContext.BaseDirectory, "bison_observe_cli_db_test.csv");

    string commentPath = Path.Combine(AppContext.BaseDirectory, "bison_comment_cli_db_test.csv");


    [Fact]
    public void StoringObservationInCSV()
    {
        // Arrange
        CSVDatabase<ObservationRecord> observationDatabase = CSVDatabase<ObservationRecord>.getInstance(observationPath);

        Assert.Single(observationDatabase.Read()); // Precondition check

        ObservationRecord o = new ObservationRecord { Author = "test", Observation = "This is a test", Timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds(), ID = 1 };

        // Act
        observationDatabase.Store(o);

        // Assert
        Assert.Equal(2, observationDatabase.Read().Count()); // Postcondition check
    }


    [Fact]
    public void StoringCommentInCSV()
    {
        // Arrange
        CSVDatabase<CommentRecord> commentDatabase = CSVDatabase<CommentRecord>.getInstance(commentPath);

        Assert.Single(commentDatabase.Read()); // Precondition check

        CommentRecord o = new CommentRecord { Author = "test", Comment = "This is a test", Timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds(), ObservationID = 1 };

        // Act
        commentDatabase.Store(o);

        // Assert
        Assert.Equal(2, commentDatabase.Read().Count()); // Postcondition check
    }
}