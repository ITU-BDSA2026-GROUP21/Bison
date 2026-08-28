using System;
using System.IO;
using System.Text;

class Program
{
    static void Main(string[] args)
    {
        string fileName = @"bison_observe_cli_db.csv";

        try
        {
            using StreamReader reader = new (fileName);
            
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
        
        using StreamWriter sw = File.AppendText(fileName);
        DateTimeOffset currentDate = DateTimeOffset.Now;
        string username = Environment.UserName;
        sw.WriteLine(username + ",\"" + args[0] + "\"," + currentDate.ToUnixTimeSeconds());
    }
}
