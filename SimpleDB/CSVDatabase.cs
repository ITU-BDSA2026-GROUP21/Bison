namespace SimpleDB;

using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;

sealed public class CSVDatabase<T> : IDatabaseRepository<T>
{
    private readonly string filePath;

    public CSVDatabase(string filePath)
    {
        this.filePath = filePath;
    }
    public IEnumerable<T> Read(int? limit = null)
    {
        var config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            HasHeaderRecord = true,
            NewLine = Environment.NewLine,
        };

        try
        {
            using StreamReader reader = new (filePath);
            using (var csv = new CsvReader(reader, config)) 
            {
                var records = csv.GetRecords<T>().ToList();

                return records;
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("StreamReader error" + e.Message);
            return Enumerable.Empty<T>();
        }
    }

    public void Store(T record)
    {
        
    }
}