public interface IObservationService
{
    public Task<ReadObservationDTO?> GetObservationById(int id);
    public Task<List<ReadObservationDTO>> GetObservations(int page);
    public Task<List<ReadObservationDTO>> GetObservationsByAuthor(string author, int page);
    public Task<List<ReadCommentDTO>> GetComments(int observationId, int page);
    public Task<List<ReadProposalDTO>> GetProposals(int observationId, int page);
}