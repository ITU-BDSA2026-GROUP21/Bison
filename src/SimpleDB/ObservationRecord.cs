using System.Runtime.InteropServices;

namespace SimpleDB;

public record ObservationRecord
{
    public required string Author { get; set; }
    public required string Observation { get; set; }
    public required long Timestamp { get; set; }
    public required int ID { get; set; }
    public required string Location { get; set; }
}