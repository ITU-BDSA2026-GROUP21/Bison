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

app.Run();
public record Observation(string Author, string Message, long Timestamp);