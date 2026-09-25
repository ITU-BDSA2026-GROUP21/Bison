
public record ObservationViewModel(string Author, string Message, string Timestamp);

public interface IObservationService
{
    public List<ObservationViewModel> GetObservations();
    public List<ObservationViewModel> GetObservationsFromAuthor(string author);
}

public class ObservationService : IObservationService
{
    public List<ObservationViewModel> GetObservations()
    {
        return DBFacade.getObs();
    }

    public List<ObservationViewModel> GetObservationsFromAuthor(string author)
    {
        // filter by the provided author name
        return DBFacade.getObs().Where(x => x.Author == author).ToList();
    }
}
