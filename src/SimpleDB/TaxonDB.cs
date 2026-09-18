namespace SimpleDB;

using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;

public sealed class TaxonDB
{
    private readonly string filePath = "data/taxons/joined.csv";
    private readonly List<TaxonRecord> records;
    private static TaxonDB? instance;
    CsvConfiguration config = new CsvConfiguration(CultureInfo.InvariantCulture)
    {
        HasHeaderRecord = true,
        NewLine = Environment.NewLine,
    };

    private TaxonDB()
    {
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

    public static TaxonDB getInstance()
    {
        if (instance == null)
        {
            instance = new TaxonDB();
        }
        return instance;
    }

    public TaxonRecord getFromID(string ID)
    {
        foreach (TaxonRecord tr in records)
        {
            if (tr.TaxonID == ID)
            {
                return tr;
            }
        }
        throw new ArgumentException("No such TaxonID exists in the database!");
    }

    public TaxonRecord getFromVernacular(string name)
    {
        foreach (TaxonRecord tr in records)
        {
            if (tr.VernacularName == name)
            {
                return tr;
            }
        }
        throw new ArgumentException("No bird with such a name exists in the database!");
    }

    public TaxonRecord getSuperTaxon(string ID)
    {
        string parentID = getFromID(ID).ParentID;
        return getFromID(parentID);
    }

    public List<TaxonRecord> getSubTaxons(string ID)
    {
        List<TaxonRecord> list = new List<TaxonRecord>();

        foreach (TaxonRecord tr in records)
        {
            if (tr.ParentID == ID)
            {
                list.Add(tr);
            }
        }

        return list;
    }
}