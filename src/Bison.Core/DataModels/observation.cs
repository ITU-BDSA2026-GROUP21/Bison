using System.Formats.Tar;

public class Observation : Post
{
    public int Id { get; set; }

    public Taxon? Taxon { get; set; }
}