using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Bison.Razor.Pages;

public class UserTimelineModel : PageModel
{
    private readonly OldIObservationService _service;
    public List<ObservationViewModel> Observations { get; set; }

    public UserTimelineModel(OldIObservationService service)
    {
        _service = service;
    }

    public ActionResult OnGet(string author, [FromQuery] int page)
    {
        Observations = _service.GetObservationsFromAuthor(author, page);
        return Page();
    }
}
