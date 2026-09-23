using Microsoft.AspNetCore.Http.HttpResults;
using SimpleDB;
using System.Net.Http.Json;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

IDatabaseRepository<ObservationRecord> observationDatabase = CSVDatabase<ObservationRecord>.getInstance("../../data/bison_observe_cli_db.csv");
IDatabaseRepository<CommentRecord> commentDatabase = CSVDatabase<CommentRecord>.getInstance("../../data/bison_comment_cli_db.csv");
IDatabaseRepository<ProposalRecord> proposalDatabase = CSVDatabase<ProposalRecord>.getInstance("../../data/bison_proposal_cli_db.csv");
TaxonDB taxons = TaxonDB.getInstance("../../data/taxons/joined.csv");

app.MapGet("/observations", () => observationDatabase.Read());

app.MapGet("/comments/{id}", (int ID) =>
{
    var result = commentDatabase.Read().Where(c => c.ObservationID == ID);

    return result;
});

app.MapGet("/proposals/{id}", (int ID) =>
{
    var result = proposalDatabase.Read().Where(p => p.ObservationID == ID);

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

app.MapPost("/proposal", (Proposal proposal) =>
{
    var observationRecords = observationDatabase.Read();

    //Checks if any id in the observationDatabase matches with proposal id
    //If none exist we return a BadRequest on Results
    if (!observationRecords.Any(o => o.ID == proposal.ID))
    {
        return Results.BadRequest($"ID: {proposal.ID} does not exist!");
    }

    try
    {
        taxons.getFromID(proposal.TaxonID);
    }
    catch (ArgumentException)
    {
        return Results.BadRequest($"Taxon: {proposal.TaxonID} does not exist!");
    }

    proposalDatabase.Store(new ProposalRecord
    {
        Author = Environment.UserName,
        TaxonID = proposal.TaxonID,
        Timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
        ObservationID = proposal.ID
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

public record Proposal(
    string TaxonID,
    int ID
);