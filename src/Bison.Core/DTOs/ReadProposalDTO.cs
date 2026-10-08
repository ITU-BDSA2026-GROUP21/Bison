public record ReadProposalDTO
{
    public required string Username { get; init; }
    public required string Text { get; init; }
    public required string Timestamp { get; init; }
    public required string DwcTaxonId { get; init; }
    public required string TaxonName { get; init; }
}