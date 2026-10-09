using System.Formats.Asn1;

public class Taxon
{
    public Taxon? Parent { get; set; }
    public List<Taxon> Children { get; set; } = new List<Taxon>();
    public required string dwc_TaxonID { get; set; }
    public string? VernacularName { get; set; }
    public int TaxonId { get; set; }

    public bool IsSubTaxon(Taxon potentialAncestor)
    {
        if (this.Parent == potentialAncestor)
        {
            return true;
        }
        else if (this.Parent != null)
        {
            return this.Parent.IsSubTaxon(potentialAncestor);
        }
        else
        {
            return false;
        }
    }
}