namespace SimpleDB;

public record TaxonRecord
{
    public required string TaxonID { get; set; }
    public required string ParentID { get; set; }
    public required TaxonRank TaxonRank { get; set; }
    public required string ScientificName { get; set; }
    public required string VernacularName { get; set; }
}


public enum TaxonRank
{
    ORDER,
    FAMILY,
    GENUS,
    SPECIES,
    SUBSPECIES
}
// dwc:taxonID,dwc:parentNameUsageID,dwc:acceptedNameUsageID,dwc:taxonomicStatus,dwc:taxonRank,dwc:scientificName,
// dwc:scientificNameAuthorship,dcterms:language,dwc:vernacularName,clb:merged