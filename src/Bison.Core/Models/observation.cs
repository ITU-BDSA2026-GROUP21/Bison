using System.Runtime.InteropServices;

public class Observation : Post
{
    public Taxon? Taxon { get; set; }
    public ICollection<Comment> Comments { get; set; } = new List<Comment>();
    public ICollection<Proposal> Proposals { get; set; } = new List<Proposal>();
    public Taxon GetTaxon()
    {
        if(Taxon == null) throw new Exception("Observation has no taxon");
        return Taxon;
    }

}