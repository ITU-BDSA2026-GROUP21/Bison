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
        return DBFacade.GetObservations();
    }

    public List<ObservationViewModel> GetObservationsFromAuthor(string author)
    {
        // filter by the provided author name
        return DBFacade.GetObservations().Where(x => x.Author == author).ToList();
    }
}
