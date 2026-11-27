using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Bison.Razor.Pages;

public class AccountInfoModel : PageModel
{

    private readonly IObservationService _observationService;
    private readonly IAuthorService _authorService;
    public List<ReadObservationDTO> Observations { get; set; } = new();
    public List<ReadCommentDTO> Comments { get; set; } = new();
    public List<ReadProposalDTO> Proposals { get; set; } = new();
    public AuthorDTO Author { get; set; }

    public AccountInfoModel(IObservationService observationService, IAuthorService authorService)
    {
        _observationService = observationService;
        _authorService = authorService;
    }

    public async Task<IActionResult> OnGetAsync(string author, int page = 1)
    {
        Observations = await _observationService.GetObservationsByAuthor(author, page);
        Comments = await _observationService.GetCommentsByAuthor(author, page);
        Proposals = await _observationService.GetProposalsByAuthor(author, page);
        Author = await _authorService.GetAuthorInformationAsync(author);
        return Page();
    }
}
