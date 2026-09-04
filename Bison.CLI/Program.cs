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
            HasHeaderRecord = false,
            NewLine = Environment.NewLine,
        };

        if (args[0].ToLower() == "read")

        {
            read(config);
        }
        else if (args[0].ToLower() == "observe")
        {
            observe(args, config);
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

                UserInterface.PrintObservations(records);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("StreamReader error" + e.Message);
        }
    }

    static void observe(string[] args, CsvConfiguration config)
    {
        using StreamWriter sw = File.AppendText(@"bison_observe_cli_db.csv");
        using (var csv = new CsvWriter(sw, config))
        {
            var records = new List<ObservationRecord>
            {
                new ObservationRecord { Author = Environment.UserName, Observation = args[1], Timestamp = DateTimeOffset.Now.ToUnixTimeSeconds() }
            };
            csv.WriteRecords(records);
    
        }
    }
}

