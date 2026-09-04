using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Text;
using CsvHelper;
using CsvHelper.Configuration;
using DocoptNet;
using SimpleDB;

class Program
{

    
    const string usage = @"
    Bison.
    
    Usage:
        bison run --read
        bison observe <text>
        bison (-h | --help)

    Options:
        -h --help   Show this help message
    ";

    static void Main(string[] args)
    {

        IDatabaseRepository<ObservationRecord> observationDatabase = new CSVDatabase<ObservationRecord>("../data/bison_observe_cli_db.csv");
        IDatabaseRepository<CommentRecord> commentDatabase = new CSVDatabase<CommentRecord>("../data/bison_comment_cli_db.csv");
        var arguments = new Docopt().Apply(usage, args, exit: true);

        var config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            HasHeaderRecord = false,
            NewLine = Environment.NewLine,
        };

        if (arguments["--read"].IsTrue)

        IDatabaseRepository<ObservationRecord> database = new CSVDatabase<ObservationRecord>("../data/bison_observe_cli_db.csv");

        if (args[0].ToLower() == "read")
        {
            read(observationDatabase);
        }
        else if (arguments["<text>"].IsString)
        {
            observe(observationDatabase, args);
        }
        else if (args[0].ToLower() == "comment")
        {
            comment(observationDatabase, commentDatabase, args);
            String input = arguments["<text>"].ToString();
            observe(input, config);
        }
        else
        {
            Console.WriteLine("Did not input run --read or observe <text>");
            observe(database, args);
        }
        else
        {
            Console.WriteLine("Did not input '-- read' or '-- observe'");
        }
    }

    static void read(IDatabaseRepository<ObservationRecord> observationDB)
    {
        var records = observationDB.Read();

        UserInterface.PrintObservations(records);

    }

    static void observe(IDatabaseRepository<ObservationRecord> observationDB, string[] args)
    static void observe(String input, CsvConfiguration config)
    {
        using StreamWriter sw = File.AppendText(@"bison_observe_cli_db.csv");
        using (var csv = new CsvWriter(sw, config))
        {
            var records = new List<ObservationRecord>
            {
                new ObservationRecord { Author = Environment.UserName, Observation = input, Timestamp = DateTimeOffset.Now.ToUnixTimeSeconds() }
            };
            csv.WriteRecords(records);
    
        }
    static void observe(IDatabaseRepository<ObservationRecord> database, string[] args)
    {
        var records = observationDB.Read();
        ObservationRecord last = records.Last();

        observationDB.Store(new ObservationRecord { Author = Environment.UserName, Observation = args[1], Timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds(), ID = last.ID++ });
        
    }

    static void comment(IDatabaseRepository<ObservationRecord> observationDB, IDatabaseRepository<CommentRecord> commentDB, string[] args)
    {
        int argID = Int32.Parse(args[2]);
        var records = commentDB.Read();
        var observationRecords = observationDB.Read();
        foreach (ObservationRecord obs in observationRecords)
        {
            if (obs.ID == argID)
            {
                commentDB.Store(new CommentRecord { Author = Environment.UserName, Comment = args[1], Timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds(), ObservationID = argID});
                return;
            }
        }
        Console.WriteLine("ID: " + argID + " does not exist!");
    }
}

