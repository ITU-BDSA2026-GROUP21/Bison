namespace SimpleDB;

using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;

sealed public class CSVDatabase<T> : IDatabaseRepository<T>
{
    private readonly string filePath;
    private static CSVDatabase<T>? instance;
    CsvConfiguration config = new CsvConfiguration(CultureInfo.InvariantCulture)
    {
        HasHeaderRecord = true,
        NewLine = Environment.NewLine,
    };

    private CSVDatabase(string filePath)
    {
        this.filePath = filePath;
    }

    public static CSVDatabase<T> getInstance(string filePath)
    {
        if (instance == null)
        {
            instance = new CSVDatabase<T>(filePath);
        }
        return instance;
    }
    public IEnumerable<T> Read(int? limit = null)
    {

        try
        {
            using StreamReader reader = new(filePath);
            using (var csv = new CsvReader(reader, config))
            {
                var records = csv.GetRecords<T>();

                if (limit.HasValue)
                {
                    records = records.Take(limit.Value);
                }

                return records.ToList();
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
        using StreamWriter sw = File.AppendText(filePath);
        using (var csv = new CsvWriter(sw, config))
        {
            csv.WriteRecord(record);
            csv.NextRecord();
        }
    }
}