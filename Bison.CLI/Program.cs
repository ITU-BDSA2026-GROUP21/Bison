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

        if (arguments["--read"].IsTrue)
        {
            read(observationDatabase);
        }
        else if (arguments["<text>"].IsString)
        {
            String input = arguments["<text>"].ToString();
            observe(observationDatabase, input);
        }
        /* else if (arguments["--comment"].IsTrue)
        {
            comment(observationDatabase, commentDatabase, input); //TODO: ADD DOCOPT OPTION TO COMMENT
        } */
        else
        {
            Console.WriteLine("Did not input run --read or observe <text>");
        }
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

        observationDB.Store(new ObservationRecord { Author = Environment.UserName, Observation = input, Timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds(), ID = last.ID++ });
        
    }

    //TODO: FIX DOCOPT COMMENT OPTION
    /* static void comment(IDatabaseRepository<ObservationRecord> observationDB, IDatabaseRepository<CommentRecord> commentDB, string input)
    {
        int argID = Int32.Parse(input);
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
    }*/
}

