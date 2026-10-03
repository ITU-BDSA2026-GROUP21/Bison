using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Bison.Razor.Pages;

public class ObservationModel : PageModel
{
    private readonly OldIObservationService _service;

    public List<ObservationViewModel> Observations { get; set; }

    public ObservationViewModel Observation { get; set; }

    public List<CommentViewModel> Comments { get; set; }

    public List<ProposalViewModel> Proposals { get; set; }

    public ObservationModel(OldIObservationService service)
    {
        _service = service;
    }

    public ActionResult OnGet(int id, [FromQuery] int page)
    {
        Observation = _service.GetObservationFromID(id);
        Observations = _service.GetObservations(page);
        Comments = _service.GetCommentsFromObservationID(id, page);
        Proposals = _service.GetProposalsFromObservationID(id, page);
        return Page();
    }
}