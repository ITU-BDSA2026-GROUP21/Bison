using System.Globalization;

namespace SimpleDB;

public record ObservationRecord
{
    public required string Author { get; set; }
    public required string Observation { get; set; }
    public required long Timestamp { get; set; }
    public required int ID { get; set; }

    public override string ToString()
    {
        return Author + " @ " + DateTimeOffset.FromUnixTimeSeconds(Timestamp).ToString("MM/dd/yy HH:mm:ss", CultureInfo.InvariantCulture) + ": " + Observation;
    }
}