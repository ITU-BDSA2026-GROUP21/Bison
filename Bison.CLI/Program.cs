using System;
using System.IO;
using System.Text;

class Program
{
    static void Main(string[] args)
    {

        if (args[0].ToLower() == "read")

        {
            read();
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

    static void read()
    {
        try
        {
            using StreamReader reader = new (@"bison_observe_cli_db.csv");
            
            //Skip first line
            reader.ReadLine(); 

            while (reader.Peek() >= 0)
            {
                //Need to check if there exists a line first

                string line = reader.ReadLine();
                string[] data = line.Split(",");
                long seconds = long.Parse(data[2]);
                DateTimeOffset date = DateTimeOffset.FromUnixTimeSeconds(seconds);
                Console.WriteLine(data[0] + " @ " + date.ToString("MM/dd/yy HH:mm:ss") + ": " + data[1]);

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

