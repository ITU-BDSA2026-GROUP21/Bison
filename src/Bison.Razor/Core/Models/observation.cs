public class Observation : Post
{
    public Taxon? Taxon { get; set; }

    public ICollection<Comment> Comments { get; set; }

    public ICollection<Proposal> Proposals { get; set; }
}