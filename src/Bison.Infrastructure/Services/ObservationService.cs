using System.Globalization;
using Bison.Core.Models;

public class ObservationService : IObservationService
{
    private readonly IObservationRepository _repository;
    private const string DTFormat = "MM/dd/yy H:mm:ss";
    public ObservationService(IObservationRepository repository)
    {
        _repository = repository;
    }
    public async Task<ReadObservationDTO?> GetObservationById(int id)
    {
        Observation? o = await _repository.GetObservationByID(id);

        if (o == null)
        {
            return null;
        }

        return DTOConverter.ToDTO(o);
    }
    public async Task<List<ReadObservationDTO>> GetObservations(int page)
    {
        List<Observation> observations = await _repository.GetObservations(page);
        List<ReadObservationDTO> dtos = new List<ReadObservationDTO>();

        foreach (Observation o in observations)
        {
            ReadObservationDTO dto = DTOConverter.ToDTO(o);

            dtos.Add(dto);
        }

        return dtos;
    }
    public async Task<List<ReadObservationDTO>> GetObservationsByAuthor(string author, int page)
    {
        List<Observation> observations = await _repository.GetObservationsByAuthor(author, page);
        List<ReadObservationDTO> dtos = new List<ReadObservationDTO>();

        foreach (Observation o in observations)
        {
            ReadObservationDTO dto = DTOConverter.ToDTO(o);

            dtos.Add(dto);
        }

        return dtos;
    }
    public async Task<List<ReadCommentDTO>> GetComments(int observationId, int page)
    {
        List<Comment> comments = await _repository.GetCommentsByObservationID(observationId, page);
        List<ReadCommentDTO> dtos = new List<ReadCommentDTO>();

        foreach (Comment c in comments)
        {
            ReadCommentDTO dto = DTOConverter.ToDTO(c);

            dtos.Add(dto);
        }

        return dtos;
    }
    public async Task<List<ReadProposalDTO>> GetProposals(int observationId, int page)
    {
        List<Proposal> proposals = await _repository.GetProposalsByObservationID(observationId, page);
        List<ReadProposalDTO> dtos = new List<ReadProposalDTO>();

        foreach (Proposal p in proposals)
        {
            ReadProposalDTO dto = DTOConverter.ToDTO(p);

            dtos.Add(dto);
        }

        return dtos;
    }
}