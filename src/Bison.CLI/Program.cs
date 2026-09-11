using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Text;
using CsvHelper;
using CsvHelper.Configuration;
using SimpleDB;
using System.CommandLine;
using System.ComponentModel;

class Program
{
    static void Main(string[] args)
    {
        RootCommand rootCommand = new("Bison program app: By Daniel, Frederik, Rasmus, Thor & Valdemar");

        IDatabaseRepository<ObservationRecord> observationDatabase = CSVDatabase<ObservationRecord>.getInstance("../../data/bison_observe_cli_db.csv");
        IDatabaseRepository<CommentRecord> commentDatabase = CSVDatabase<CommentRecord>.getInstance("../../data/bison_comment_cli_db.csv");

        Command readCommand = new("read", "Read from the database");
        Command obeserveCommand = new("observe", "Add a new observation to the database");
        Command commentCommand = new("comment", "Add a comment to an observation");

        rootCommand.Add(readCommand);
        rootCommand.Add(obeserveCommand);
        rootCommand.Add(commentCommand);


        readCommand.SetAction(parseResult =>
        {
            read(observationDatabase);
        });


        Argument<String> observation = new("observation")
        {
            Description = "The observation you want to add"
        };

        obeserveCommand.Arguments.Add(observation);

        obeserveCommand.SetAction(parseResult =>
        {
            string input = parseResult.GetValue(observation);
            observe(observationDatabase, input);
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
            comment(observationDatabase, commentDatabase, commentID, commentArg);
        });


        ParseResult parseResult = rootCommand.Parse(args);
        parseResult.Invoke();
    }

    static void read(IDatabaseRepository<ObservationRecord> observationDB)
    {
        var records = observationDB.Read();

        UserInterface.PrintObservations(records);

    }

    static void observe(IDatabaseRepository<ObservationRecord> observationDB, string input)
    {
        var records = observationDB.Read();
        ObservationRecord last = records.Last();

        observationDB.Store(new ObservationRecord { Author = Environment.UserName, Observation = input, Timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds(), ID = ++last.ID });

    }

    static void comment(IDatabaseRepository<ObservationRecord> observationDB, IDatabaseRepository<CommentRecord> commentDB, int argID, string input)
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
        Console.WriteLine("ID: " + argID + " does not exist!");
    }
}

