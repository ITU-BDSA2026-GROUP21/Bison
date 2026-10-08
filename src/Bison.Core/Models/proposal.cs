public class Proposal : Post
{
    public required Observation Observation { get; set; }
    public required Taxon Taxon { get; set; }
}