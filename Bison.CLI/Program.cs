using System;
using System.IO;
using System.Text;

class Program
{
    static void Main(string[] args)
    {
        string fileName = @"C:\Users\ninvr\Bison.CLI\bison_observe_cli_db.csv";

        try
        {
            using StreamReader reader = new(fileName);
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                Console.WriteLine(line);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }
}