namespace Bison.Razor.Pages;

public class TaxonDropdown
{
    public List<TaxonDropdownOption> TaxonDropdownOptions { get; set; } = new();
    public string? SelectedDwcTaxonId { get; set; }
    public string Name { get; set; } = "";
    public string Placeholder { get; set; } = "All Taxons";
    public bool AutoSubmit { get; set; }
}