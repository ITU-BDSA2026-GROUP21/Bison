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
            observe(args);
        }
        else
        {
            Console.WriteLine("Did not input --read or --observe");
        }
    }

    static void read(IDatabaseRepository<ObservationRecord> database)
    {
        var records = database.Read();

        foreach(ObservationRecord obs in records)
        {
            DateTimeOffset date = DateTimeOffset.FromUnixTimeSeconds((long)Convert.ToDouble(obs.Timestamp));
            Console.WriteLine(obs.Author + " @ " +  date.ToString("MM/dd/yy HH:mm:ss")  + ": " + obs.Observation);
        }
    }

    static void observe(string[] args)
    {
        using StreamWriter sw = File.AppendText(@"bison_observe_cli_db.csv");
        DateTimeOffset currentDate = DateTimeOffset.Now;
        string username = Environment.UserName;
        sw.WriteLine(username + ",\"" + args[1] + "\"," + currentDate.ToUnixTimeSeconds());
    }
}

