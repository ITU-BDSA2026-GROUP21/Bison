using System.Globalization;

namespace SimpleDB;

public record ProposalRecord
{
    public required string Author { get; set; }
    public required string TaxonID { get; set; }
    public required long Timestamp { get; set; }
    public required int ObservationID { get; set; }

    public override string ToString()
    {
        return Author + " @ " + DateTimeOffset.FromUnixTimeSeconds(Timestamp).ToString("MM/dd/yy HH:mm:ss", CultureInfo.InvariantCulture) + ": " + TaxonID;
    }
}