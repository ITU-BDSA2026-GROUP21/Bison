using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Bison.Razor.Pages;

public class PublicModel : PageModel
{
    private readonly IObservationService _observationService;
    private readonly ITaxonFilterService _taxonFilterService;
    public List<ReadObservationDTO> Observations { get; set; } = new();

    public PublicModel(IObservationService observationService, ITaxonFilterService taxonFilterService)
    {
        _observationService = observationService;
        _taxonFilterService = taxonFilterService;
    }

    public async Task<IActionResult> OnGetAsync([FromQuery] string? dwcTaxonId, [FromQuery] int page = 1)
    {
        if (dwcTaxonId != null)
        {
            Observations = await _taxonFilterService.GetFilteredObservations(dwcTaxonId, page);
        }
        else
        {
            Observations = await _observationService.GetObservations(page);
        }

        return Page();
    }
}
