using System.Data.Common;

public record ObservationViewModel(string Author, string Message, string Timestamp);

public record CommentViewModel(string Author, string Message, string Timestamp);

public record ProposalViewModel(string Author, string Message, string Timestamp);

public interface IObservationService
{
    public List<ObservationViewModel> GetObservations(int page);
    public List<ObservationViewModel> GetObservationsFromAuthor(string author, int page);
}

public class ObservationService : IObservationService
{
    private readonly IDBFacade db;

    public ObservationService(IDBFacade db)
    {
        this.db = db;
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
}
