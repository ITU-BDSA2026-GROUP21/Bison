public record ReadCommentDTO
{
    public required string Username { get; init; }
    public required string Text { get; init; }
    public required string Timestamp { get; init; }
}