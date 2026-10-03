public interface IObservationRepository
{
    public Task<Observation?> GetObservationByID(int id);
    public Task<List<Observation>> GetObservations(int page);
    // The method below assumes that Author.Name is unique. Two authors with the same name will currently share timelines.
    public Task<List<Observation>> GetObservationsByAuthor(string author, int page);
    public Task<List<Comment>> GetCommentsByObservationID(int observationId, int page);
    public Task<List<Proposal>> GetProposalsByObservationID(int observationId, int page);
}