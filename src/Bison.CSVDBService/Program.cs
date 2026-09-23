using Microsoft.AspNetCore.Http.HttpResults;
using SimpleDB;
using System.Net.Http.Json;

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

    //Checks if any id in the observationDatabase matches with comment id
    //If none exist we return a BadRequest on Results
    if (!observationRecords.Any(o => o.ID == comment.ID))
    {
        return Results.BadRequest($"ID: {comment.ID} does not exist!");
    }

    commentDatabase.Store(new CommentRecord
    {
        Author = Environment.UserName,
        Comment = comment.Message,
        Timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
        ObservationID = comment.ID
    });
    return Results.Ok();


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