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

app.Run();

public record Observation(
    string Message,
    string Location
);