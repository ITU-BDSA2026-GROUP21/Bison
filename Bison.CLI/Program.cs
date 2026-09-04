using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Text;
using CsvHelper;
using CsvHelper.Configuration;
using SimpleDB;

class Program
{
    static void Main(string[] args)
    {

        IDatabaseRepository<ObservationRecord> database = new CSVDatabase<ObservationRecord>("../data/bison_observe_cli_db.csv");

        if (args[0].ToLower() == "read")
        {
            read(database);
        }
        else if (args[0].ToLower() == "observe")
        {
            observe(database, args);
        }
        else
        {
            Console.WriteLine("Did not input --read or --observe");
        }
    }

    static void read(IDatabaseRepository<ObservationRecord> database)
    {
        var records = database.Read();

        UserInterface.PrintObservations(records);

    }

    static void observe(IDatabaseRepository<ObservationRecord> database, string[] args)
    {

        database.Store(new ObservationRecord { Author = Environment.UserName, Observation = args[1], Timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds() });
        
    }
}

