using Bison.Core.Models;

public interface ITaxonRepository
{
    public Task<Taxon?> GetTaxonByDwcId(string dwcTaxonId);
    public Task<List<Taxon>> GetTaxons();
}