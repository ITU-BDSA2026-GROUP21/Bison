namespace SimpleDB;

public record CommentRecord
{
    public required string Author {get; set;}
    public required string Comment {get; set;}
    public required long Timestamp {get; set;}
    public required int ObservationID {get; set;}
}