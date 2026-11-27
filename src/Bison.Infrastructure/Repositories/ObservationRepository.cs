using Microsoft.EntityFrameworkCore;
using Bison.Core.Models;
using Bison.Core;

public class ObservationRepository : IObservationRepository
{
    private readonly BisonDBContext _dbContext;
    private const int PageSize = PagingDefaults.PageSize;
    public ObservationRepository(BisonDBContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Observation?> GetObservationByID(int id)
    {
        var query = _dbContext.Observations.Include(o => o.Author)
                                           .Include(o => o.Taxon)
                                           .Where(o => o.PostId == id);

        var result = await query.FirstOrDefaultAsync();

        return result;
    }
    public async Task<List<Observation>> GetObservations(int page)
    {
        page = Math.Max(page, 1);

        var query = _dbContext.Observations.Include(o => o.Author)
                                           .Include(o => o.Taxon)
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
                                           .Include(o => o.Taxon)
                                           .Where(o => o.Author.UserName == author)
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
    public async Task<List<Comment>> GetCommentsByAuthor(string author, int page)
    {
        page = Math.Max(page, 1);

        //If it doesn't work use .Equals();
        var query = _dbContext.Comments.Include(c => c.Author)
                               .Where(c => c.Observation.Author.UserName == author)
                               .OrderByDescending(c => c.TimeStamp)
                               .ThenByDescending(c => c.PostId)
                               .Skip((page - 1) * PageSize)
                               .Take(PageSize);

        var result = await query.ToListAsync();

        return result;
    }

    public async Task<HashSet<Author>> GetObservationUpVotes(int observationId)
    {
        var query = _dbContext.Observations.Include(o => o.UpVoters)
                                           .Where(o => o.PostId == observationId);

        var result = await query.FirstOrDefaultAsync();

        HashSet<Author>? upVoters = result.UpVoters;

        if (upVoters == null)
        {
            throw new ArgumentException("This Observation has no UpVoter Set!");
        }

        return upVoters;
    }
    public async Task<HashSet<Author>> GetProposalUpVotes(int proposalId)
    {
        var query = _dbContext.Proposals.Include(p => p.UpVoters)
                                        .Where(p => p.PostId == proposalId);

        var result = await query.FirstOrDefaultAsync();

        HashSet<Author>? upVoters = result.UpVoters;

        if (upVoters == null)
        {
            throw new ArgumentException("This Proposal has no UpVoter Set!");
        }

        return upVoters;
    }

    public async Task<int> MakeComment(string text, Author author, int id)
    {

        Observation? observation = await _dbContext.Observations.FindAsync(id);

        if (observation == null)
        {
            throw new Exception("There is no observation id for the comment");
        }

        var comment = new Comment
        {
            Text = text,
            Author = author,
            TimeStamp = DateTime.UtcNow,
            Observation = observation,
        };

        _dbContext.Comments.Add(comment);
        await _dbContext.SaveChangesAsync();

        return comment.PostId;
    }

    public async Task<int> MakeProposal(string text, Author author, int id, string? taxonId)
    {
        Observation? observation = await _dbContext.Observations.FindAsync(id);

        if (observation == null)
        {
            throw new Exception("There is no observation id for the proposal");
        }

        Taxon? taxon = null;
        if (!string.IsNullOrWhiteSpace(taxonId))
        {
            taxon = await _dbContext.Taxons.FirstOrDefaultAsync(t => t.dwc_TaxonID == taxonId)
            ?? throw new ArgumentException("Taxon id is not in the database");
        }

        var proposal = new Proposal
        {
            Text = text,
            Author = author,
            TimeStamp = DateTime.UtcNow,
            Observation = observation,
            Taxon = taxon,
        };

        _dbContext.Proposals.Add(proposal);
        await _dbContext.SaveChangesAsync();

        return proposal.PostId;
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
    public async Task<List<Proposal>> GetProposalsByAuthor(string author, int page)
    {
        page = Math.Max(page, 1);

        var query = _dbContext.Proposals.Include(p => p.Author)
                                .Include(p => p.Taxon)
                                .Where(p => p.Observation.Author.UserName == author)
                                .OrderByDescending(p => p.TimeStamp)
                                .ThenByDescending(p => p.PostId)
                                .Skip((page - 1) * PageSize)
                                .Take(PageSize);

        var result = await query.ToListAsync();

        return result;
    }

    // Only meant to be used for filtered view!
    public async Task<List<Observation>> GetAllObservations()
    {
        var query = _dbContext.Observations.Include(o => o.Author)
                                           .Include(o => o.Taxon)
                                           .OrderByDescending(o => o.TimeStamp)
                                           .ThenByDescending(o => o.PostId);

        var result = await query.ToListAsync();

        return result;
    }

    public async Task<int> MakeObservation(string text, Author author, string? taxonId, string? photoName = null, string? license = null)
    {
        Taxon? taxon = null;
        if (!string.IsNullOrWhiteSpace(taxonId))
        {
            taxon = await _dbContext.Taxons.FirstOrDefaultAsync(t => t.dwc_TaxonID == taxonId)
            ?? throw new ArgumentException("Taxon id is not in the database");
        }

        var observation = new Observation
        {
            Text = text,
            Author = author,
            TimeStamp = DateTime.UtcNow,
            Taxon = taxon,
            PicturePath = photoName,
            License = license
        };

        _dbContext.Observations.Add(observation);
        await _dbContext.SaveChangesAsync();

        return observation.PostId;

    }
    public async Task<bool> AddUpvoteToObservation(Author author, int observationId)
    {
        var upVoters = await GetObservationUpVotes(observationId);

        if (upVoters.Contains(author))
        {
            return false;
        }

        // TODO: Check if this actually works, manually
        upVoters.Add(author);

        await _dbContext.SaveChangesAsync();

        return true;
    }
    public async Task<bool> AddUpvoteToProposal(Author author, int proposalId)
    {
        var upVoters = await GetProposalUpVotes(proposalId);

        if (upVoters.Contains(author))
        {
            return false;
        }

        // TODO: Check if this actually works, manually
        upVoters.Add(author);

        await _dbContext.SaveChangesAsync();

        return true;
    }

    public async Task<bool> RemoveUpvoteFromObservation(Author author, int observationId)
    {
        var upVoters = await GetObservationUpVotes(observationId);

        if (!upVoters.Contains(author))
        {
            return false;
        }

        // TODO: Check if this actually works, manually
        upVoters.Remove(author);

        await _dbContext.SaveChangesAsync();

        return true;
    }
    public async Task<bool> RemoveUpvoteFromProposal(Author author, int proposalId)
    {
        var upVoters = await GetProposalUpVotes(proposalId);

        if (!upVoters.Contains(author))
        {
            return false;
        }

        // TODO: Check if this actually works, manually
        upVoters.Remove(author);

        await _dbContext.SaveChangesAsync();

        return true;
    }
}