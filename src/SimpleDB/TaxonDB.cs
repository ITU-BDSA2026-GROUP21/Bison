namespace SimpleDB;

using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;

public sealed class TaxonDB
{
    private readonly string filePath;
    private readonly List<TaxonRecord> records;
    private static TaxonDB? instance;
    CsvConfiguration config = new CsvConfiguration(CultureInfo.InvariantCulture)
    {
        HasHeaderRecord = true,
        NewLine = Environment.NewLine,
    };

    private TaxonDB(string filePath)
    {
        this.filePath = filePath;
        records = new List<TaxonRecord>();

        try
        {
            using StreamReader reader = new(filePath);
            using (var csv = new CsvReader(reader, config))
            {

                csv.Read();
                csv.ReadHeader();
                while (csv.Read())
                {
                    var record = new TaxonRecord
                    {
                        TaxonID = csv.GetField<string>("dwc:taxonID"),
                        ParentID = csv.GetField<string>("dwc:parentNameUsageID"),
                        TaxonRank = csv.GetField<string>("dwc:taxonRank"),
                        ScientificName = csv.GetField<string>("dwc:scientificName"),
                        VernacularName = csv.GetField<string>("dwc:vernacularName")
                    };
                    records.Add(record);
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("StreamReader error" + e.Message);
        }
    }

    public static TaxonDB getInstance(string filePath)
    {
        if (instance == null)
        {
            instance = new TaxonDB(filePath);
        }
        return instance;
    }

}