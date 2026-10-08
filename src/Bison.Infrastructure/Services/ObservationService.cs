using System.Globalization;

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

        return ToDTO(o);
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
    public async Task<List<ReadObservationDTO>> GetObservationsByAuthor(string author, int page)
    {
        List<Observation> observations = await _repository.GetObservationsByAuthor(author, page);
        List<ReadObservationDTO> dtos = new List<ReadObservationDTO>();

        foreach (Observation o in observations)
        {
            ReadObservationDTO dto = ToDTO(o);

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
            ReadCommentDTO dto = ToDTO(c);

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
            ReadProposalDTO dto = ToDTO(p);

            dtos.Add(dto);
        }

        return dtos;
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
    private static ReadCommentDTO ToDTO(Comment c)
    {
        ReadCommentDTO dto = new ReadCommentDTO
        {
            Username = c.Author.Name,
            Text = c.Text,
            Timestamp = DateTimeToString(c.TimeStamp)
        };

        return dto;
    }
    private static ReadProposalDTO ToDTO(Proposal p)
    {
        string name;

        if (p.Taxon.VernacularName == null || p.Taxon.VernacularName == "")
        {
            name = "No vernacular name";
        }
        else
        {
            name = p.Taxon.VernacularName;
        }

        ReadProposalDTO dto = new ReadProposalDTO
        {
            Username = p.Author.Name,
            Text = p.Text,
            Timestamp = DateTimeToString(p.TimeStamp),
            DwcTaxonId = p.Taxon.dwc_TaxonID,
            TaxonName = name
        };

        return dto;
    }
    private static string DateTimeToString(DateTime t)
    {
        return t.ToString(DTFormat, CultureInfo.InvariantCulture);
    }
}