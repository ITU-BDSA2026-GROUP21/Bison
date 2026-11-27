using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Identity;
using Bison.Core.Models;

namespace Bison.Razor.Pages;

public class ObservationModel : PageModel
{
    private readonly IObservationService _observationService;
    private readonly ITaxonFilterService _taxonFilterService;
    public string? SelectedDwcTaxonId { get; set; }
    public List<TaxonDropdownOption> TaxonDropdownOptions { get; set; } = new();
    private readonly UserManager<Author> _userManager;
    public Post MostUpvoted;

    public ReadObservationDTO? Observation { get; set; }

    public List<ReadCommentDTO> Comments { get; set; } = new();

    public List<ReadProposalDTO> Proposals { get; set; } = new();

    public Dictionary<int, bool> HasUpvoted { get; set; } = new();

    public ObservationModel(UserManager<Author> userManager, IObservationService observationService, ITaxonFilterService taxonFilterService)
    {
        _observationService = observationService;
        _userManager = userManager;
        _taxonFilterService = taxonFilterService;
    }

    public async Task<IActionResult> OnGetAsync(int id, [FromQuery] int page = 1)
    {
        Observation = await _observationService.GetObservationById(id);
        if (Observation == null)
        {
            return NotFound();
        }
        Comments = await _observationService.GetComments(id, page);
        Proposals = await _observationService.GetProposals(id, page);
        TaxonDropdownOptions = await _taxonFilterService.GetTaxonDropdownOptions();
        MostUpvoted = await _observationService.GetPostWithMaxUpvotes(id);
        Author author = await _userManager.GetUserAsync(User);
        bool observationVote = author != null && await _observationService.HasAuthorUpvotedObservation(author, Observation.Id);
        HasUpvoted.Add(Observation.Id, observationVote);
        foreach (var pro in Proposals)
        {
            bool proposalVote = author != null && await _observationService.HasAuthorUpvotedProposal(author, pro.Id);
            HasUpvoted.Add(pro.Id, proposalVote);
        }
        return Page();
    }

    //Posting comment logic:

    [BindProperty]
    public InputModel Input { get; set; } = new();

    public class InputModel
    {
        public string CommentText { get; set; } = "";

        public string ProposalText { get; set; } = "";

        public string? TaxonId { get; set; }
    }

    async public Task<IActionResult> OnPostPostComment(int id)
    {
        // 1. Check that the submitted data is valid
        if (!ModelState.IsValid)
        {
            return RedirectToPage(new { id });
        }

        if (id == null)
        {
            throw new Exception("The comment is missing an observation");
        }

        Author author = await _userManager.GetUserAsync(User);

        // 2. Access the submitted form values
        string description = Input.CommentText;

        await _observationService.PostComment(description, author, id);

        // 4. Redirect to the observation page after successful submission
        return RedirectToPage(new { id });
    }

    //BUG/TODO: When posting a proposal without a taxon foreign key exception 
    async public Task<IActionResult> OnPostPostProposal(int id)
    {
        if (!ModelState.IsValid)
        {
            TaxonDropdownOptions = await _taxonFilterService.GetTaxonDropdownOptions();
            return RedirectToPage(new { id });
        }

        //Maybe not necessary
        if (id == null)
        {
            ModelState.AddModelError("Input.TaxonId", "Choose a taxon for your proposal.");
        }

        if (id == null)
        {
            throw new Exception("The proposal is missing an observation");
        }

        string? taxonId = null;

        if (!string.IsNullOrWhiteSpace(Input.TaxonId))
        {
            taxonId = Input.TaxonId;
        }

        Author author = await _userManager.GetUserAsync(User);

        string description = Input.ProposalText;

        await _observationService.PostProposal(description, author, id, taxonId);

        return RedirectToPage(new { id });
    }

    async public Task<IActionResult> OnPostUpvoteObservationAsync(int id)
    {
        Author? author = await _userManager.GetUserAsync(User);

        bool hasUpvoted = await _observationService.HasAuthorUpvotedObservation(author, id);

        if (hasUpvoted)
        {
            await _observationService.RemoveUpvoteFromObservation(author, id);
        }
        else
        {
            await _observationService.AddUpvoteToObservation(author, id);
        }

        TaxonDropdownOptions = await _taxonFilterService.GetTaxonDropdownOptions();
        return RedirectToPage(new { id });
    }

    async public Task<IActionResult> OnPostUpvoteProposalAsync(int id, int proposalId)
    {
        Author author = await _userManager.GetUserAsync(User);

        bool hasUpvoted = await _observationService.HasAuthorUpvotedProposal(author, proposalId);

        if (hasUpvoted)
        {
            await _observationService.RemoveUpvoteFromProposal(author, proposalId);
        }
        else
        {
            await _observationService.AddUpvoteToProposal(author, proposalId);
        }

        TaxonDropdownOptions = await _taxonFilterService.GetTaxonDropdownOptions();
        return RedirectToPage(new { id });
    }
}

