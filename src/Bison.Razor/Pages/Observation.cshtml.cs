using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Identity;
using Bison.Core.Models;

namespace Bison.Razor.Pages;

public class ObservationModel : PageModel
{
    private readonly IObservationService _service;
    private readonly UserManager<Author> _userManager;

    public ReadObservationDTO? Observation { get; set; }

    public List<ReadCommentDTO> Comments { get; set; } = new();

    public List<ReadProposalDTO> Proposals { get; set; } = new();

    public ObservationModel(UserManager<Author> userManager ,IObservationService service)
    {
        _service = service;
        _userManager = userManager;
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

    //Posting comment logic:
    
    [BindProperty]
    public InputModel Input { get; set; } = new();

    public class InputModel
    {
        public string Text { get; set; } = "";
    }

    async public Task<IActionResult> OnPostPostComment(int id)
    {
        // 1. Check that the submitted data is valid
        if (!ModelState.IsValid)
        {
            return RedirectToPage(new {id});
        }

        if (id == null)
        {
            throw new Exception("The comment is missing an observation");
        }

        Author author = await _userManager.GetUserAsync(User);

        // 2. Access the submitted form values
        string description = Input.Text;

        await _service.PostComment(description, author, id);

        // 4. Redirect to the observation page after successful submission
        return RedirectToPage(new {id});
    }


}

