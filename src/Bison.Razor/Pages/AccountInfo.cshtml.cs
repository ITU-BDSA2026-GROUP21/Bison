using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Bison.Razor.Pages;

public class AccountInfoModel : PageModel
{

    private readonly IObservationService _service;
    public List<ReadObservationDTO> Observations { get; set; } = new();
    public List<ReadCommentDTO> Comments { get; set; } = new();
    public List<ReadProposalDTO> Proposals { get; set; } = new();

    public AccountInfoModel(IObservationService service)
    {
        _service = service;
    }

    public async Task<IActionResult> OnGetAsync(string author, int page = 1)
    {
        Observations = await _service.GetObservationsByAuthor(author, page);
        Comments = await _service.GetCommentsByAuthor(author, page);
        Proposals = await _service.GetProposalsByAuthor(author, page);
        return Page();
    }
}
