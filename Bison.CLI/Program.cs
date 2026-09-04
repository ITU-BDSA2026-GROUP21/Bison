using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Text;
using CsvHelper;
using CsvHelper.Configuration;

class Program
{
    static void Main(string[] args)
    {

        var config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            HasHeaderRecord = true,
            NewLine = Environment.NewLine,
        };

        if (args[0].ToLower() == "read")

        {
            read(config);
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

    static void read(CsvConfiguration config)
    {
        try
        {
            using StreamReader reader = new (@"bison_observe_cli_db.csv");
            using (var csv = new CsvReader(reader, config)) 
            {
                var records = csv.GetRecords<ObservationRecord>();

                foreach(ObservationRecord obs in records)
                {
                    DateTimeOffset date = DateTimeOffset.FromUnixTimeSeconds((long)Convert.ToDouble(obs.Timestamp));
                    Console.WriteLine(obs.Author + " @ " +  date.ToString("MM/dd/yy HH:mm:ss")  + ": " + obs.Observation);
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
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

