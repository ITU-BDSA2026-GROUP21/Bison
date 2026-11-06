public interface ITaxonFilterService
{
    public Task<List<TaxonDropdownOption>> GetTaxonDropdownOptions();
    public Task<List<ReadObservationDTO>> GetFilteredObservations(string dwcTaxonId, int page);

    // Maybe we'll have an author version of the filtered observations, this is probably what its signature would look like.
    //public Task<List<ReadObservationDTO>> GetFilteredObservationsByAuthor(string author, string dwc_taxonId, int page);
}