using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Bison.Core.Models;

namespace Bison.Razor.Pages;

[Authorize]
public class AccountInfoModel : PageModel
{

    private readonly IObservationService _observationService;
    private readonly UserManager<Author> _userManager;
    public List<ReadObservationDTO> Observations { get; set; } = new();
    public List<ReadCommentDTO> Comments { get; set; } = new();
    public List<ReadProposalDTO> Proposals { get; set; } = new();
    public Author Author { get; set; }

    public AccountInfoModel(IObservationService observationService, UserManager<Author> userManager)
    {
        _observationService = observationService;
        _userManager = userManager;
    }

    public async Task<IActionResult> OnGetAsync(int page = 1)
    {
        Author = await _userManager.GetUserAsync(User);

        string author = Author.UserName;

        Observations = await _observationService.GetObservationsByAuthor(author, page);
        Comments = await _observationService.GetCommentsByAuthor(author, page);
        Proposals = await _observationService.GetProposalsByAuthor(author, page);
        return Page();
    }

    public async Task<IActionResult> OnPostForgetMe()
    {
        Author = await _userManager.GetUserAsync(User);
        string author = Author.UserName;
        await _observationService.DeleteFullByAuthor(author);
        return Redirect("/obs");
    }
}
