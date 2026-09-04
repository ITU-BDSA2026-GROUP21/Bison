using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Text;
using CsvHelper;
using CsvHelper.Configuration;
using DocoptNet;

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

        var arguments = new Docopt().Apply(usage, args, exit: true);

        var config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            HasHeaderRecord = false,
            NewLine = Environment.NewLine,
        };

        if (arguments["--read"].IsTrue)

        {
            read(config);
        }
        else if (arguments["<text>"].IsString)
        {
            String input = arguments["<text>"].ToString();
            observe(input, config);
        }
        else
        {
            Console.WriteLine("Did not input run --read or observe <text>");
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
    }
}

