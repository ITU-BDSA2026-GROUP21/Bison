using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Bison.Core.Models;
using Microsoft.CodeAnalysis.Scripting;

namespace Bison.Razor.Pages;

[Authorize]
[RequestSizeLimit(11 * 1024 * 1024)]
[RequestFormLimits(MultipartBodyLengthLimit = 10 * 1024 * 1024)]
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
        public string? License { get; set; }
    }

    async public Task OnGetAsync()
    {
        TaxonDropdownOptions = await _taxonFilterService.GetTaxonDropdownOptions();
    }

    async public Task<IActionResult> OnPostPostObservation()
    {

        string[] allowed = { "by", "by-sa", "by-nc", "by-nc-sa", "by-nd", "by-nc-nd"};
        if(Input.Photo != null && !allowed.Contains(Input.License))
        {
            
            ModelState.AddModelError("Input.License", "Choose a license for your photo.");
        }

        // 1. Check that the submitted data is valid
        if (!ModelState.IsValid)
        {
            TaxonDropdownOptions = await _taxonFilterService.GetTaxonDropdownOptions();
            //return Redirect("/PostObservation");
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

        try // Handle the actual creation of the observation in a try-catch to handle image upload errors more gracefully
        {
            // 3. Check if a photo has been uploaded. If yes, call PostObservation with an additional Stream.
            if (photo != null)
            {  
                using Stream photoStream = photo.OpenReadStream(); // Using the using keyword to dispose of the stream after we're done using it.
                await _observationService.PostObservation(description, author, taxonId, photoStream, Input.License);
            }
            else
            {
                await _observationService.PostObservation(description, author, taxonId);
            }
        }
        catch (ArgumentException ex)
        {
            ModelState.AddModelError("Input.Photo", ex.Message);

            TaxonDropdownOptions = await _taxonFilterService.GetTaxonDropdownOptions();

            return Page();
        }

        // 4. Redirect after successful submission
        return Redirect("/obs");
    }
}