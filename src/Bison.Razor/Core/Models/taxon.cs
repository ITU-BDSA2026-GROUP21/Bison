public class Taxon
{
    public Taxon? Parent { get; set; }

    public List<Taxon> Children { get; set; }

    public required string dwc_TaxonID { get; set; }

    public string? VernacularName { get; set; }

    public int TaxonId { get; set; }
}