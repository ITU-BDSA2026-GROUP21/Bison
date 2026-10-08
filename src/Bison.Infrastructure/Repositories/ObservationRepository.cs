using Microsoft.EntityFrameworkCore;

public class ObservationRepository : IObservationRepository
{
    private readonly BisonDBContext _dbContext;
    private const int PageSize = 32;
    public ObservationRepository(BisonDBContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Observation?> GetObservationByID(int id)
    {
        var query = _dbContext.Observations.Include(o => o.Author)
                                           .Where(o => o.PostId == id);

        var result = await query.FirstOrDefaultAsync();

        return result;
    }
    public async Task<List<Observation>> GetObservations(int page)
    {
        page = Math.Max(page, 1);

        var query = _dbContext.Observations.Include(o => o.Author)
                                           .OrderByDescending(o => o.TimeStamp)
                                           .ThenByDescending(o => o.PostId)
                                           .Skip((page - 1) * PageSize)
                                           .Take(PageSize);

        var result = await query.ToListAsync();

        return result;
    }
    // The method below assumes that Author.Name is unique. Two authors with the same name will currently share timelines.
    public async Task<List<Observation>> GetObservationsByAuthor(string author, int page)
    {
        page = Math.Max(page, 1);

        var query = _dbContext.Observations.Include(o => o.Author)
                                           .Where(o => o.Author.Name == author)
                                           .OrderByDescending(o => o.TimeStamp)
                                           .ThenByDescending(o => o.PostId)
                                           .Skip((page - 1) * PageSize)
                                           .Take(PageSize);

        var result = await query.ToListAsync();

        return result;
    }
    public async Task<List<Comment>> GetCommentsByObservationID(int observationId, int page)
    {
        page = Math.Max(page, 1);

        var query = _dbContext.Comments.Include(c => c.Author)
                                       .Where(c => c.Observation.PostId == observationId)
                                       .OrderByDescending(c => c.TimeStamp)
                                       .ThenByDescending(c => c.PostId)
                                       .Skip((page - 1) * PageSize)
                                       .Take(PageSize);

        var result = await query.ToListAsync();

        return result;
    }
    public async Task<List<Proposal>> GetProposalsByObservationID(int observationId, int page)
    {
        page = Math.Max(page, 1);

        var query = _dbContext.Proposals.Include(p => p.Author)
                                        .Include(p => p.Taxon)
                                        .Where(p => p.Observation.PostId == observationId)
                                        .OrderByDescending(p => p.TimeStamp)
                                        .ThenByDescending(p => p.PostId)
                                        .Skip((page - 1) * PageSize)
                                        .Take(PageSize);

        var result = await query.ToListAsync();

        return result;
    }
}