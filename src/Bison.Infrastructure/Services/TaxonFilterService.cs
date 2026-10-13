using Bison.Core.Models;
public class TaxonFilterService : ITaxonFilterService
{
    private readonly IObservationRepository _observationRepository;
    private readonly ITaxonRepository _taxonRepository;
    private const int PageSize = PagingDefaults.PageSize;
    public TaxonFilterService(IObservationRepository observationRepository, ITaxonRepository taxonRepository)
    {
        _observationRepository = observationRepository;
        _taxonRepository = taxonRepository;
    }
    public async Task<List<ReadObservationDTO>> GetFilteredObservations(string dwcTaxonId, int page)
    {
        _ = await _taxonRepository.GetTaxons(); // Makes sure that the Taxon hierarchy is tracked in memory.

        Taxon? searchTaxon = await _taxonRepository.GetTaxonByDwcId(dwcTaxonId);

        if (searchTaxon == null)
        {
            return new List<ReadObservationDTO>();
        }

        // Get all observations as a List, convert to an Array, then to a Dafny Sequence
        List<Observation> observationList = await _observationRepository.GetAllObservations();
        Observation[] observationArray = observationList.ToArray();
        Dafny.ISequence<Observation> observationSequence = Dafny.Sequence<Observation>.FromArray(observationArray);

        // Filter the dafny Sequence with FilterByWrapped, convert to an array, then to a List .
        Dafny.ISequence<Observation> filteredSequence = TaxonFilterWrapper.__default.FilterByWrapped(searchTaxon, observationSequence);
        Observation[] filteredArray = filteredSequence.CloneAsArray();
        List<Observation> filteredList = filteredArray.ToList();

        page = Math.Max(page, 1);

        //Using LINQ to skip and take the correct amount of Observations for the page.
        List<Observation> pagedList = filteredList.Skip((page - 1) * PageSize).Take(PageSize).ToList();

        List<ReadObservationDTO> dtos = new List<ReadObservationDTO>();

        foreach (Observation o in pagedList)
        {
            ReadObservationDTO dto = DTOConverter.ToDTO(o);

            dtos.Add(dto);
        }

        return dtos;
    }
}