using System.Globalization;

public class ObservationService : IObservationService
{
    private readonly IObservationRepository _repository;
    private const string DTFormat = "MM/dd/yy H:mm:ss";
    public ObservationService(IObservationRepository repository)
    {
        _repository = repository;
    }
    public Task<ReadObservationDTO?> GetObservationById(int id)
    {
        throw new NotImplementedException();
    }
    public async Task<List<ReadObservationDTO>> GetObservations(int page)
    {
        List<Observation> observations = await _repository.GetObservations(page);
        List<ReadObservationDTO> dtos = new List<ReadObservationDTO>();

        foreach (Observation o in observations)
        {
            ReadObservationDTO dto = ToDTO(o);

            dtos.Add(dto);
        }

        return dtos;
    }
    public Task<List<ReadObservationDTO>> GetObservationsByAuthor(string author, int page)
    {
        throw new NotImplementedException();
    }
    public Task<List<ReadCommentDTO>> GetComments(int observationId, int page)
    {
        throw new NotImplementedException();
    }
    public Task<List<ReadProposalDTO>> GetProposals(int observationId, int page)
    {
        throw new NotImplementedException();
    }

    //Static private methods
    private static ReadObservationDTO ToDTO(Observation o)
    {
        ReadObservationDTO dto = new ReadObservationDTO
        {
            Id = o.PostId,
            Username = o.Author.Name,
            Text = o.Text,
            Timestamp = DateTimeToString(o.TimeStamp)
        };

        return dto;
    }
    private static string DateTimeToString(DateTime t)
    {
        return t.ToString(DTFormat, CultureInfo.InvariantCulture);
    }
}