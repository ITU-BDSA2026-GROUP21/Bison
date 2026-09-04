namespace SimpleDB;

public record ObservationRecord
{
    public required string Author {get; set;}
    public required string Observation {get; set;}
    public required long Timestamp {get; set;}
}