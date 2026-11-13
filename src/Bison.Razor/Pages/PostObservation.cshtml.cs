using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Bison.Core.Models;

namespace Bison.Razor.Pages;

[Authorize]
public class PostObservation : PageModel
{

    private readonly UserManager<Author> _userManager;

    private readonly IObservationService _observationService;

    private readonly ITaxonFilterService _taxonFilterService;

    public List<TaxonDropdownOption> TaxonDropdownOptions { get; set; } = new();

    public PostObservation(UserManager<Author> userManager, IObservationService observationService, ITaxonFilterService taxonFilterService)
    {
        _userManager = userManager;
        _observationService = observationService;
        _taxonFilterService = taxonFilterService;
    }

    [BindProperty]
    public InputModel Input { get; set; } = new();

    public class InputModel
    {
        public string Text { get; set; } = "";
        public string? TaxonId { get; set; }
        public IFormFile? Photo { get; set; }
    }

    async public Task OnGetAsync()
    {
        TaxonDropdownOptions = await _taxonFilterService.GetTaxonDropdownOptions();
    }

    async public Task<IActionResult> OnPostPostObservation()
    {

        // 1. Check that the submitted data is valid
        if (!ModelState.IsValid)
        {
            TaxonDropdownOptions = await _taxonFilterService.GetTaxonDropdownOptions();
            return Page();
        }

        string? taxonId = null;

        if (!string.IsNullOrWhiteSpace(Input.TaxonId))
        {
            taxonId = Input.TaxonId;
        }

        Author author = await _userManager.GetUserAsync(User);

        // 2. Access the submitted form values
        string description = Input.Text;
        //string? taxonId = Input.TaxonId;
        IFormFile? photo = Input.Photo;

        await _observationService.PostObservation(description, author, taxonId);

        // 4. Redirect after successful submission
        return Redirect("/obs");
    }
}