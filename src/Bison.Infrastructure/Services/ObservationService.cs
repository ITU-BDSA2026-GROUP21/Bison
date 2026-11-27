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
    public async Task<List<ReadCommentDTO>> GetCommentsByAuthor(string author, int page)
    {
        List<Comment> comments = await _repository.GetCommentsByAuthor(author, page);
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
    public async Task<List<ReadProposalDTO>> GetProposalsByAuthor(string author, int page)
    {
        List<Proposal> proposals = await _repository.GetProposalsByAuthor(author, page);
        List<ReadProposalDTO> dtos = new List<ReadProposalDTO>();

        foreach (Proposal p in proposals)
        {
            ReadProposalDTO dto = DTOConverter.ToDTO(p);

            dtos.Add(dto);
        }

        return dtos;
    }

    public async Task<int> PostObservation(string text, Author author, string? taxonId, Stream? photo = null, string? license = null)
    {
        if (photo != null)
        {
            // Upload photo and get the returned photo name
            string photoName = await _photoService.UploadPhotoAsync(photo);

            try
            {
                return await _repository.MakeObservation(text, author, taxonId, photoName, license);
            }
            catch (Exception) // If the observation failed to be saved by the repository, delete the photo and rethrow the caught exception.
            {
                _photoService.DeletePhoto(photoName);
                throw;
            }
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

    public async Task<int> PostProposal(string text, Author author, int id, string? taxonId)
    {
        return await _repository.MakeProposal(text, author, id, taxonId);
    }
    public async Task<int> GetObservationUpvotes(int observationId)
    {
        HashSet<Author> set = await _repository.GetObservationUpVotes(observationId);
        return set.Count();
    }
    public async Task<int> GetProposalUpvotes(int proposalId)
    {
        HashSet<Author> set = await _repository.GetProposalUpVotes(proposalId);
        return set.Count();
    }
    public async Task<int> GetMaxProposalUpvotes(int observationId)
    {
        Observation obs = await _repository.GetObservationByID(observationId);
        Proposal max = null;
        foreach (var proposal in obs.Proposals)
        {
            int upvotes = await GetProposalUpvotes(proposal.PostId);
            if (upvotes > max.UpVoters.Count())
            {
                max = proposal;
            }
            else if (upvotes == max.UpVoters.Count())
            {
                max = proposal.TimeStamp > max.TimeStamp ? proposal : max;
            }
        }
        return max.UpVoters.Count();
    }
    public async Task<bool> AddUpvoteToObservation(Author author, int observationId)
    {
        return await _repository.AddUpvoteToObservation(author, observationId);
    }
    public async Task<bool> AddUpvoteToProposal(Author author, int proposalId)
    {
        return await _repository.AddUpvoteToProposal(author, proposalId);
    }
    public async Task<bool> RemoveUpvoteFromObservation(Author author, int observationId)
    {
        return await _repository.RemoveUpvoteFromObservation(author, observationId);
    }
    public async Task<bool> RemoveUpvoteFromProposal(Author author, int proposalId)
    {
        return await _repository.RemoveUpvoteFromProposal(author, proposalId);
    }
    public async Task<bool> HasAuthorUpvotedObservation(Author author, int observationId)
    {
        var upVoters = await _repository.GetObservationUpVotes(observationId);
        return upVoters.Contains(author);
    }
    public async Task<bool> HasAuthorUpvotedProposal(Author author, int proposalId)
    {
        var upVoters = await _repository.GetProposalUpVotes(proposalId);
        return upVoters.Contains(author);
    }
}