using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Bison.Razor.Pages;

public class PublicModel : PageModel
{
    private readonly IObservationService _service;
    public List<ReadObservationDTO> Observations { get; set; } = new();

    public PublicModel(IObservationService service)
    {
        _service = service;
    }

    public async Task<IActionResult> OnGetAsync([FromQuery] int page = 1)
    {
        Observations = await _service.GetObservations(page);
        return Page();
    }
}
