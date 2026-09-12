using SimpleDB;
using System.CommandLine;
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

        readCommand.SetAction(parseResult =>
        {
            read(observations);
        });


        Argument<String> observation = new("observation")
        {
            Description = "The observation you want to add"
        };

        obeserveCommand.Arguments.Add(observation);

        obeserveCommand.SetAction(parseResult =>
        {
            string input = parseResult.GetValue(observation);
            observe(observations, input);
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
    public static void read(IDatabaseRepository<ObservationRecord> observationDB)
    {
        var records = observationDB.Read();

        PrintObservations(records);

    }

    public static void observe(IDatabaseRepository<ObservationRecord> observationDB, string input)
    {
        var records = observationDB.Read();
        ObservationRecord last = records.Last();

        observationDB.Store(new ObservationRecord { Author = Environment.UserName, Observation = input, Timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds(), ID = ++last.ID });

    }

    public static void comment(IDatabaseRepository<ObservationRecord> observationDB, IDatabaseRepository<CommentRecord> commentDB, int argID, string input)
    {
        var observationRecords = observationDB.Read();
        foreach (ObservationRecord obs in observationRecords)
        {
            if (obs.ID == argID)
            {
                commentDB.Store(new CommentRecord { Author = Environment.UserName, Comment = input, Timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds(), ObservationID = argID });
                return;
            }
        }

        throw new ArgumentException("ID: " + argID + " does not exist!");
    }

    static void discussion(IDatabaseRepository<CommentRecord> commentDB, IDatabaseRepository<ObservationRecord> observationDB, int observationID)
    {
        var commentRecords = commentDB.Read();
        var observationRecords = observationDB.Read();
        PrintComments(commentRecords, observationRecords, observationID);
    }


    public static void PrintObservations(IEnumerable<ObservationRecord> obs)
    {
        foreach (ObservationRecord o in obs)
        {
            Console.WriteLine(o);
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