using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Bison.Razor.Pages;

public class UserTimelineModel : PageModel
{
    private readonly IObservationService _service;
    public List<ReadObservationDTO> Observations { get; set; } = new();

    public UserTimelineModel(IObservationService service)
    {
        _service = service;
    }

    public async Task<IActionResult> OnGetAsync(string author, [FromQuery] int page = 1)
    {
        Observations = await _service.GetObservationsByAuthor(author, page);
        return Page();
    }
}
