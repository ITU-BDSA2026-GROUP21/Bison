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
            observe(args, config);
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

