using System.Globalization;
using Bison.Core.Models;

public class ObservationService : IObservationService
{
    private readonly IObservationRepository _repository;
    private readonly IPhotoService _photoService;
    private const string DTFormat = "MM/dd/yy H:mm:ss";
    public ObservationService(IObservationRepository repository, IPhotoService photoService)
    {
        _repository = repository;
        _photoService = photoService;
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

    public async Task<int> PostObservation(string text, Author author, string? taxonId, Stream? photo = null)
    {
        if (photo != null)
        {
            string photoName = await _photoService.UploadPhotoAsync(photo);
            return await _repository.MakeObservation(text, author, taxonId, photoName);
        }
        else
        {
            return await _repository.MakeObservation(text, author, taxonId);
        }
    }

    public async Task<int> PostComment(string text, Author author, int id)
    {
        return await _repository.MakeComment(text, author, id);
    }
}