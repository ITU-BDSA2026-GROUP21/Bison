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

    public PostObservation(UserManager<Author> userManager, IObservationService observationService)
    {
        _userManager = userManager;
        _observationService = observationService;
    }


    [BindProperty]
    public InputModel Input { get; set; } = new();

    public class InputModel
    {
        public string Text { get; set; } = "";
        public int TaxonId { get; set; }
        public IFormFile? Photo { get; set; }
    }

    async public Task<IActionResult> OnPostPostObservation()
    {
        // 1. Check that the submitted data is valid
        if (!ModelState.IsValid)
        {
            return Page();
        }

        Author? author = await _userManager.GetUserAsync(User);

        // 2. Access the submitted form values
        string description = Input.Text;
        int taxonId = Input.TaxonId;
        IFormFile? photo = Input.Photo;

        //_observationService.PostObservation(description, author, taxonId);

        // 3. TODO: Create and save the observation
        // Use your observation service / database here.



        // 4. Redirect after successful submission
        return RedirectToPage("/Index");
    }
}