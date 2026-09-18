using SimpleDB;
using System.CommandLine;

using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
public static class UserInterface
{

    public static void run(String[] args, IDatabaseRepository<ObservationRecord> observations, IDatabaseRepository<CommentRecord> comments)
    {
        RootCommand rootCommand = new("Bison program app: By Daniel, Frederik, Rasmus, Thor & Valdemar");

        Command readCommand = new("read", "Read from the database");
        Command obeserveCommand = new("observe", "Add a new observation to the database");
        Command commentCommand = new("comment", "Add a comment to an observation");
        Command discusionCommand = new("discussion", "Read comments from an observation");

        rootCommand.Add(readCommand);
        rootCommand.Add(obeserveCommand);
        rootCommand.Add(commentCommand);
        rootCommand.Add(discusionCommand);

        Argument<String?> readLocation = new("optional:location")
        {
            Description = "Optional string for sorting observations by location",
            DefaultValueFactory = _ => ""
        };

        readCommand.Add(readLocation);

        readCommand.SetAction(async parseResult =>
        {
            string input = parseResult.GetValue(readLocation);
            await read(input);
        });



        Argument<String> observation = new("observation")
        {
            Description = "The observation you want to add"
        };

        Argument<String> location = new("location")
        {
            Description = "The location of your observation"
        };

        obeserveCommand.Arguments.Add(observation);
        obeserveCommand.Arguments.Add(location);

        obeserveCommand.SetAction(async parseResult =>
        {
            string input = parseResult.GetValue(observation);
            string locationArg = parseResult.GetValue(location);
            await observe(input, locationArg);
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

        commentCommand.SetAction(parseResult =>
        {
            int commentID = parseResult.GetValue(id);
            string commentArg = parseResult.GetValue(commentText);
            comment(observations, comments, commentID, commentArg);
        });


        Argument<int> discusionID = new("discussionID")
        {
            Description = "The ID of the observation you want to comment from"
        };

        discusionCommand.Arguments.Add(discusionID);

        discusionCommand.SetAction(ParseResult =>
        {
            int ID = ParseResult.GetValue(discusionID);
            discussion(comments, observations, ID);
        });


        ParseResult parseResult = rootCommand.Parse(args);
        parseResult.Invoke();

    }

    public static async Task read(string? readLocation)
    {
        var baseURL = "http://localhost:5004";
        using HttpClient client = new();

        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        client.BaseAddress = new Uri(baseURL);

        var records = await client.GetFromJsonAsync<List<ObservationRecord>>("observations");

        PrintObservations(records, readLocation);
    }

    public static async Task observe(string input, string location)
    {

        var baseURL = "http://localhost:5004";
        using HttpClient client = new();

        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        client.BaseAddress = new Uri(baseURL);

        var observation = new Observation(
            input,
            location
        );

        await client.PostAsJsonAsync("observation", observation);
    }

    public static void comment(IDatabaseRepository<ObservationRecord> observationDB, IDatabaseRepository<CommentRecord> commentDB,
    int argID, string input)
    {
        var observationRecords = observationDB.Read();
        foreach (ObservationRecord obs in observationRecords)
        {
            if (obs.ID == argID)
            {
                commentDB.Store(new CommentRecord
                {
                    Author = Environment.UserName,
                    Comment = input,
                    Timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                    ObservationID = argID
                });
                return;
            }
        }

        throw new ArgumentException("ID: " + argID + " does not exist!");
    }

    public static void discussion(IDatabaseRepository<CommentRecord> commentDB, IDatabaseRepository<ObservationRecord> observationDB,
    int observationID)
    {
        var commentRecords = commentDB.Read();
        var observationRecords = observationDB.Read();
        PrintComments(commentRecords, observationRecords, observationID);
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
}
public record Observation(
    string Message,
    string Location
);

public record Comment(
    string Message,
    int ID
);