using SimpleDB;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

IDatabaseRepository<ObservationRecord> observationDatabase = CSVDatabase<ObservationRecord>.getInstance("../../data/bison_observe_cli_db.csv");
IDatabaseRepository<CommentRecord> commentDatabase = CSVDatabase<CommentRecord>.getInstance("../../data/bison_comment_cli_db.csv");

app.MapGet("/observations", () => observationDatabase.Read());

app.MapGet("/comments/{id}", (int ID) =>
{
    var result = commentDatabase.Read().Where(c => c.ObservationID == ID);

    return result;
});

app.MapPost("/observation", (Observation observation) => observationDatabase.Store(new ObservationRecord
{
    Author = Environment.UserName,
    Observation = observation.Message,
    Timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
    ID = ++observationDatabase.Read().Last().ID,
    Location = observation.Location
}));

app.MapPost("/comment", (Comment comment) =>
{
    var observationRecords = observationDatabase.Read();
    foreach (ObservationRecord obs in observationRecords)
    {
        if (obs.ID == comment.ID)
        {
            commentDatabase.Store(new CommentRecord
            {
                Author = Environment.UserName,
                Comment = comment.Message,
                Timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                ObservationID = comment.ID
            });
            return;
        }
    }

    throw new ArgumentException("ID: " + comment.ID + " does not exist!");
});


app.Run();

public record Observation(
    string Message,
    string Location
);

public record Comment(
    string Message,
    int ID
);