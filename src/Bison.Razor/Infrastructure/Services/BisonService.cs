using System.Data.Common;

public interface IObservationService
{
    public ObservationViewModel GetObservationFromID(int id);
    public List<ObservationViewModel> GetObservations(int page);
    public List<ObservationViewModel> GetObservationsFromAuthor(string author, int page);
    public List<CommentViewModel> GetCommentsFromObservationID(int id, int page);
    public List<ProposalViewModel> GetProposalsFromObservationID(int id, int page);
}

public class ObservationService : IObservationService
{
    private readonly IDBFacade db;

    public ObservationService(IDBFacade db)
    {
        this.db = db;
    }

    public ObservationViewModel GetObservationFromID(int id)
    {
        return db.GetObservationByID(id);
    }

    public List<ObservationViewModel> GetObservations(int page)
    {
        return db.GetObservations(page);
    }

    public List<ObservationViewModel> GetObservationsFromAuthor(string author, int page)
    {
        // filter by the provided author name
        return db.GetObservationsByAuthor(author, page);
    }

    public List<CommentViewModel> GetCommentsFromObservationID(int id, int page)
    {
        return db.GetCommentsByObservationID(id, page);
    }

    public List<ProposalViewModel> GetProposalsFromObservationID(int id, int page)
    {
        return db.GetProposalsByObservationID(id, page);
    }
}
