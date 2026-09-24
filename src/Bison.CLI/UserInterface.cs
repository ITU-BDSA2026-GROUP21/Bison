using SimpleDB;
using System.CommandLine;


using System.Net.Http.Json;
public static class UserInterface
{

    public static async Task run(String[] args, HttpClient client)
    {
        RootCommand rootCommand = new("Bison program app: By Daniel, Frederik, Rasmus, Thor & Valdemar");

        Command readCommand = new("read", "Read from the database");
        Command observeCommand = new("observe", "Add a new observation to the database");
        Command commentCommand = new("comment", "Add a comment to an observation");
        Command discusionCommand = new("discussion", "Read comments from an observation");
        Command proposalCommand = new("proposal", "Propose a taxon for an observation");
        Command proposalsCommand = new("proposals", "Read proposals for an observation");

        rootCommand.Add(readCommand);
        rootCommand.Add(observeCommand);
        rootCommand.Add(commentCommand);
        rootCommand.Add(discusionCommand);
        rootCommand.Add(proposalCommand);
        rootCommand.Add(proposalsCommand);

        Argument<String?> readLocation = new("optional:location")
        {
            Description = "Optional string for sorting observations by location",
            DefaultValueFactory = _ => ""
        };

        readCommand.Add(readLocation);

        readCommand.SetAction(async parseResult =>
        {
            string input = parseResult.GetValue(readLocation);
            await read(client, input);
        });



        Argument<String> observation = new("observation")
        {
            Description = "The observation you want to add"
        };

        Argument<String> location = new("location")
        {
            Description = "The location of your observation"
        };

        observeCommand.Arguments.Add(observation);
        observeCommand.Arguments.Add(location);

        observeCommand.SetAction(async parseResult =>
        {
            string input = parseResult.GetValue(observation);
            string locationArg = parseResult.GetValue(location);
            await observe(client, input, locationArg);
        });


        Argument<int> id = new("commentID")
        {
            Description = "The ID of the observation you want to comment on"
        };

        Argument<String> commentText = new("comment")
        {
            Description = "The comment you want to the observation"
        };

        commentCommand.Arguments.Add(id);
        commentCommand.Arguments.Add(commentText);

        commentCommand.SetAction(async parseResult =>
        {
            int commentID = parseResult.GetValue(id);
            string commentArg = parseResult.GetValue(commentText);
            await comment(client, commentID, commentArg);
        });


        Argument<int> discussionID = new("discussionID")
        {
            Description = "The ID of the observation you want to comment from"
        };

        discusionCommand.Arguments.Add(discussionID);

        discusionCommand.SetAction(async ParseResult =>
        {
            int ID = ParseResult.GetValue(discussionID);
            await discussion(client, ID);
        });

        Argument<int> proposalID = new("proposalID")
        {
            Description = "The ID of the observation you want to propose a taxon for"
        };

        Argument<string> taxonID = new("taxonID")
        {
            Description = "The ID of the taxon you want to propose for the observation"
        };

        proposalCommand.Arguments.Add(proposalID);
        proposalCommand.Arguments.Add(taxonID);

        proposalCommand.SetAction(async parseResult =>
        {
            int obsID = parseResult.GetValue(proposalID);
            string proposalArg = parseResult.GetValue(taxonID);
            await propose(client, obsID, proposalArg);
        });

        Argument<int> proposalsID = new("proposalsID")
        {
            Description = "The ID of the observation you want to see proposals for"
        };

        proposalsCommand.Arguments.Add(proposalsID);

        proposalsCommand.SetAction(async ParseResult =>
        {
            int ID = ParseResult.GetValue(proposalsID);
            await proposals(client, ID);
        });

        ParseResult parseResult = rootCommand.Parse(args);
        parseResult.Invoke();

    }

    public static async Task read(HttpClient client, string? readLocation)
    {
        var records = await client.GetFromJsonAsync<List<ObservationRecord>>("observations");

        PrintObservations(records, readLocation);
    }

    public static async Task observe(HttpClient client, string input, string location)
    {
        var observation = new Observation(
            input,
            location
        );

        await client.PostAsJsonAsync("observation", observation);
    }

    public static async Task comment(HttpClient client, int argID, string input)
    {
        var comment = new Comment(
            input,
            argID
        );

        var result = await client.PostAsJsonAsync("comment", comment);

        if (!result.IsSuccessStatusCode)
        {
            throw new ArgumentException(await result.Content.ReadAsStringAsync());
        }
    }

    public static async Task propose(HttpClient client, int obsID, string proposedTaxonID)
    {
        var proposal = new Proposal(
            proposedTaxonID,
            obsID
        );

        var result = await client.PostAsJsonAsync("proposal", proposal);

        if (!result.IsSuccessStatusCode)
        {
            throw new ArgumentException(await result.Content.ReadAsStringAsync());
        }
    }

    public static async Task discussion(HttpClient client, int observationID)
    {
        var CRecords = await client.GetFromJsonAsync<List<CommentRecord>>($"comments/{observationID}");
        var ORecords = await client.GetFromJsonAsync<List<ObservationRecord>>("observations");

        PrintComments(CRecords, ORecords, observationID);

    }

    public static async Task proposals(HttpClient client, int observationID)
    {
        var PRecords = await client.GetFromJsonAsync<List<ProposalRecord>>($"proposals/{observationID}");
        var ORecords = await client.GetFromJsonAsync<List<ObservationRecord>>("observations");

        PrintProposals(PRecords, ORecords, observationID);

    }

    public static void PrintObservations(IEnumerable<ObservationRecord> obs, string? location)
    {
        foreach (ObservationRecord o in obs)
        {
            if (location == "")
            {
                Console.WriteLine(o);
            }
            else
            {
                if (o.Location.StartsWith(location, StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine(o);
                }
            }
        }
    }

    public static void PrintComments(IEnumerable<CommentRecord> com, IEnumerable<ObservationRecord> obs, int ID)
    {

        foreach (ObservationRecord o in obs)
        {
            if (o.ID == ID)
            {
                Console.WriteLine(o);
            }
        }

        foreach (CommentRecord r in com)
        {
            if (r.ObservationID == ID)
            {
                Console.WriteLine("- " + r);
            }
        }
    }

    public static void PrintProposals(IEnumerable<ProposalRecord> pro, IEnumerable<ObservationRecord> obs, int ID)
    {

        foreach (ObservationRecord o in obs)
        {
            if (o.ID == ID)
            {
                Console.WriteLine(o);
            }
        }

        foreach (ProposalRecord p in pro)
        {
            if (p.ObservationID == ID)
            {
                Console.WriteLine("- " + p);
            }
        }
    }
}