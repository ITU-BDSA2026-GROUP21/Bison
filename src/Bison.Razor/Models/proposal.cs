public class Proposal : Post
{
    public int Id { get; set; }

    public Observation Observation { get; set; }

    public Taxon Taxon { get; set; }
}