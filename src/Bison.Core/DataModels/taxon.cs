public class Taxon
{
    public Taxon? Parent { get; set; }

    public List<Taxon> Children { get; set; }

    public string DwcTaxonId { get; set; }

    public string VernacularName { get; set; }
}