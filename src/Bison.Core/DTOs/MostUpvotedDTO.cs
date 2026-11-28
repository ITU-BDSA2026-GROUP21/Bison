public record MostUpvotedTaxonDTO
{
    public required string Username { get; init; }
    public required string Timestamp { get; init; }
    public required string TaxonName { get; init; }
    public required int Upvotes { get; init; }
}