using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Bison.Razor.Pages;

public class PublicModel : PageModel
{
    private readonly IObservationService _observationService;
    private readonly ITaxonFilterService _taxonFilterService;
    public List<ReadObservationDTO> Observations { get; set; } = new();
    public int CurrentPage { get; set; }
    public string? SelectedDwcTaxonId { get; set; }
    public List<TaxonDropdownOption> TaxonDropdownOptions { get; set; } = new();

    public PublicModel(IObservationService observationService, ITaxonFilterService taxonFilterService)
    {
        _observationService = observationService;
        _taxonFilterService = taxonFilterService;
    }

    public async Task<IActionResult> OnGetAsync([FromQuery] string? dwcTaxonId, [FromQuery] int page = 1)
    {
        CurrentPage = Math.Max(page, 1);

        TaxonDropdownOptions = await _taxonFilterService.GetTaxonDropdownOptions();

        if (!string.IsNullOrWhiteSpace(dwcTaxonId))
        {
            SelectedDwcTaxonId = dwcTaxonId;
            Observations = await _taxonFilterService.GetFilteredObservations(dwcTaxonId, CurrentPage);
        }
        else
        {
            Observations = await _observationService.GetObservations(CurrentPage);
        }

        return Page();
    }
}
