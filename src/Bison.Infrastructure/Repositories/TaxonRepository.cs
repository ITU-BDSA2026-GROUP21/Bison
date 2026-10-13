using Bison.Core.Models;
using Microsoft.EntityFrameworkCore;
public class TaxonRepository : ITaxonRepository
{
    private readonly BisonDBContext _dbContext;
    public TaxonRepository(BisonDBContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<Taxon?> GetTaxonByDwcId(string dwcTaxonId)
    {
        var query = _dbContext.Taxons.Where(t => t.dwc_TaxonID == dwcTaxonId);

        var result = await query.FirstOrDefaultAsync();

        return result;
    }

    //In the filtering service, this should be called before GetTaxonByDwcId, so that every Taxon is loaded into memory, 
    // so IsSubTaxon can properly check all paths.
    public async Task<List<Taxon>> GetTaxons()
    {
        var query = _dbContext.Taxons;

        var result = await query.ToListAsync();

        return result;
    }
}