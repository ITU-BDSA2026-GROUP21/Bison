using Bison.Core.Models;
public interface IObservationService
{
    public Task<ReadObservationDTO?> GetObservationById(int id);
    public Task<List<ReadObservationDTO>> GetObservations(int page);
    public Task<List<ReadObservationDTO>> GetObservationsByAuthor(string author, int page);
    public Task<List<ReadCommentDTO>> GetComments(int observationId, int page);
    public Task<List<ReadCommentDTO>> GetCommentsByAuthor(string author, int page);
    public Task<List<ReadProposalDTO>> GetProposals(int observationId, int page);
    public Task<List<ReadProposalDTO>> GetProposalsByAuthor(string author, int page);
    public Task<int> PostObservation(string text, Author author, string? taxonId, Stream? photo = null, string? license = null);
    public Task<int> PostComment(string text, Author author, int id);
    public Task<int> PostProposal(string text, Author author, int id, string? taxonId);
    public Task<int> GetObservationUpvotes(int observationId);
    public Task<int> GetProposalUpvotes(int proposalId);
    public Task<int> GetMaxProposalUpvotes(int observationId);
}