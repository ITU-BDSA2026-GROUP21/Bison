using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Bison.Razor.Pages;

public class ObservationModel : PageModel
{
    private readonly IObservationService _service;

    public ReadObservationDTO? Observation { get; set; }

    public List<ReadCommentDTO> Comments { get; set; } = new();

    public List<ReadProposalDTO> Proposals { get; set; } = new();

    public ObservationModel(IObservationService service)
    {
        _service = service;
    }

    public async Task<IActionResult> OnGetAsync(int id, [FromQuery] int page = 1)
    {
        Observation = await _service.GetObservationById(id);
        if (Observation == null)
        {
            return NotFound();
        }
        Comments = await _service.GetComments(id, page);
        Proposals = await _service.GetProposals(id, page);
        return Page();
    }
}