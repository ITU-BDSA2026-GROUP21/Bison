using Bison.Core.Models;
using Dafny;

namespace Bison.Tests.Unit;

public class FilterTests
{
    [Fact]
    public void FilterBy_Top_Taxon_Returns_All()
    {
        // Arrange: Taxons copied from DbInitializer
        var tPelecaniformes = new Taxon() { TaxonId = 1, dwc_TaxonID = "MSTSNM:Arter:3e4e67e4-f785-ea11-aa77-501ac539d1ea", VernacularName = "Årefodede", Parent = null };
        var tArdeidae = new Taxon() { TaxonId = 2, dwc_TaxonID = "MSTSNM:Arter:495067e4-f785-ea11-aa77-501ac539d1ea", VernacularName = "Hejrer", Parent = tPelecaniformes };
        var tFregatidae = new Taxon() { TaxonId = 3, dwc_TaxonID = "MSTSNM:Arter:a15367e4-f785-ea11-aa77-501ac539d1ea", VernacularName = "Fregatfugle", Parent = tPelecaniformes };
        var tPelecanidae = new Taxon() { TaxonId = 4, dwc_TaxonID = "MSTSNM:Arter:025767e4-f785-ea11-aa77-501ac539d1ea", VernacularName = "Pelikaner", Parent = tPelecaniformes };
        var tPhalacrocoracidae = new Taxon() { TaxonId = 5, dwc_TaxonID = "MSTSNM:Arter:2a5767e4-f785-ea11-aa77-501ac539d1ea", VernacularName = "Skarver", Parent = tPelecaniformes };
        var tSulidae = new Taxon() { TaxonId = 6, dwc_TaxonID = "MSTSNM:Arter:5c5967e4-f785-ea11-aa77-501ac539d1ea", VernacularName = "Suler", Parent = tPelecaniformes };
        var tThreskiornithidae = new Taxon() { TaxonId = 7, dwc_TaxonID = "MSTSNM:Arter:af5967e4-f785-ea11-aa77-501ac539d1ea", VernacularName = "Ibiser & skestorke", Parent = tPelecaniformes };
        var tArdea = new Taxon() { TaxonId = 8, dwc_TaxonID = "MSTSNM:Arter:7f9ef9f3-f785-ea11-aa77-501ac539d1ea", VernacularName = null, Parent = tArdeidae };
        var tArdeola = new Taxon() { TaxonId = 9, dwc_TaxonID = "MSTSNM:Arter:819ef9f3-f785-ea11-aa77-501ac539d1ea", VernacularName = null, Parent = tArdeidae };
        var tBotaurus = new Taxon() { TaxonId = 10, dwc_TaxonID = "MSTSNM:Arter:dca0f9f3-f785-ea11-aa77-501ac539d1ea", VernacularName = null, Parent = tArdeidae };
        var tBubulcus = new Taxon() { TaxonId = 11, dwc_TaxonID = "MSTSNM:Arter:74a1f9f3-f785-ea11-aa77-501ac539d1ea", VernacularName = null, Parent = tArdeidae };
        var tEgretta = new Taxon() { TaxonId = 12, dwc_TaxonID = "MSTSNM:Arter:25aaf9f3-f785-ea11-aa77-501ac539d1ea", VernacularName = null, Parent = tArdeidae };
        var tIxobrychus = new Taxon() { TaxonId = 13, dwc_TaxonID = "MSTSNM:Arter:eeb1f9f3-f785-ea11-aa77-501ac539d1ea", VernacularName = null, Parent = tArdeidae };
        var tNycticorax = new Taxon() { TaxonId = 14, dwc_TaxonID = "MSTSNM:Arter:fab9f9f3-f785-ea11-aa77-501ac539d1ea", VernacularName = null, Parent = tArdeidae };
        var tFregata = new Taxon() { TaxonId = 15, dwc_TaxonID = "MSTSNM:Arter:0aadf9f3-f785-ea11-aa77-501ac539d1ea", VernacularName = null, Parent = tFregatidae };
        var tPelecanus = new Taxon() { TaxonId = 16, dwc_TaxonID = "MSTSNM:Arter:0bbdf9f3-f785-ea11-aa77-501ac539d1ea", VernacularName = null, Parent = tPelecanidae };
        var tGulosus = new Taxon() { TaxonId = 17, dwc_TaxonID = "MSTSNM:Arter:701cda16-1f30-4973-b36b-af7e00fd7609", VernacularName = null, Parent = tPhalacrocoracidae };
        var tMicrocarbo = new Taxon() { TaxonId = 18, dwc_TaxonID = "MSTSNM:Arter:cb487af6-7074-427a-a7cb-b20900a28376", VernacularName = null, Parent = tPhalacrocoracidae };
        var tPhalacrocorax = new Taxon() { TaxonId = 19, dwc_TaxonID = "MSTSNM:Arter:c5bdf9f3-f785-ea11-aa77-501ac539d1ea", VernacularName = null, Parent = tPhalacrocoracidae };
        var tMorus = new Taxon() { TaxonId = 20, dwc_TaxonID = "MSTSNM:Arter:cb50a8dd-d411-4bc3-869b-ac2800d7e676", VernacularName = null, Parent = tSulidae };
        var tSula = new Taxon() { TaxonId = 21, dwc_TaxonID = "MSTSNM:Arter:dfcd4cd2-9f34-4122-a30e-adf600a544fb", VernacularName = null, Parent = tSulidae };
        var tGeronticus = new Taxon() { TaxonId = 22, dwc_TaxonID = "MSTSNM:Arter:ef2c03c2-1064-461b-9b3e-b0b500cd20d7", VernacularName = null, Parent = tThreskiornithidae };
        var tPlatalea = new Taxon() { TaxonId = 23, dwc_TaxonID = "MSTSNM:Arter:33bff9f3-f785-ea11-aa77-501ac539d1ea", VernacularName = null, Parent = tThreskiornithidae };
        var tPlegadis = new Taxon() { TaxonId = 24, dwc_TaxonID = "MSTSNM:Arter:7dbff9f3-f785-ea11-aa77-501ac539d1ea", VernacularName = null, Parent = tThreskiornithidae };
        var tArdeaAlba = new Taxon() { TaxonId = 25, dwc_TaxonID = "MSTSNM:Arter:c18811f4-f785-ea11-aa77-501ac539d1ea", VernacularName = "Sølvhejre", Parent = tArdea };
        var tArdeaCinerea = new Taxon() { TaxonId = 26, dwc_TaxonID = "MSTSNM:Arter:c28811f4-f785-ea11-aa77-501ac539d1ea", VernacularName = "Fiskehejre", Parent = tArdea };
        var tArdeaIbis = new Taxon() { TaxonId = 27, dwc_TaxonID = "MSTSNM:Arter:f69111f4-f785-ea11-aa77-501ac539d1ea", VernacularName = "Kohejre", Parent = tArdea };
        var tArdeaPurpurea = new Taxon() { TaxonId = 28, dwc_TaxonID = "MSTSNM:Arter:c38811f4-f785-ea11-aa77-501ac539d1ea", VernacularName = "Purpurhejre", Parent = tArdea };
        var tArdeolaRalloides = new Taxon() { TaxonId = 29, dwc_TaxonID = "MSTSNM:Arter:c68811f4-f785-ea11-aa77-501ac539d1ea", VernacularName = "Tophejre", Parent = tArdeola };
        var tBotaurusLentiginosus = new Taxon() { TaxonId = 30, dwc_TaxonID = "MSTSNM:Arter:2c9011f4-f785-ea11-aa77-501ac539d1ea", VernacularName = "Amerikansk rørdrum", Parent = tBotaurus };
        var tBotaurusMinutus = new Taxon() { TaxonId = 31, dwc_TaxonID = "MSTSNM:Arter:f54eddf8-f785-ea11-aa77-501ac539d1ea", VernacularName = "Dværghejre", Parent = tBotaurus };
        var tBotaurusStellaris = new Taxon() { TaxonId = 32, dwc_TaxonID = "MSTSNM:Arter:2d9011f4-f785-ea11-aa77-501ac539d1ea", VernacularName = "Rørdrum", Parent = tBotaurus };
        var tEgrettaGarzetta = new Taxon() { TaxonId = 33, dwc_TaxonID = "MSTSNM:Arter:ca4d8cf8-f785-ea11-aa77-501ac539d1ea", VernacularName = "Silkehejre", Parent = tEgretta };
        var tNycticoraxNycticorax = new Taxon() { TaxonId = 34, dwc_TaxonID = "MSTSNM:Arter:2d66ddf8-f785-ea11-aa77-501ac539d1ea", VernacularName = "Nathejre", Parent = tNycticorax };
        var tFregataMagnificens = new Taxon() { TaxonId = 35, dwc_TaxonID = "MSTSNM:Arter:7b578cf8-f785-ea11-aa77-501ac539d1ea", VernacularName = "Pragtfregatfugl", Parent = tFregata };
        var tPelecanusCrispus = new Taxon() { TaxonId = 36, dwc_TaxonID = "MSTSNM:Arter:ee6dddf8-f785-ea11-aa77-501ac539d1ea", VernacularName = "Krøltoppet pelikan", Parent = tPelecanus };
        var tGulosusAristotelis = new Taxon() { TaxonId = 37, dwc_TaxonID = "MSTSNM:Arter:c4f52bf9-f785-ea11-aa77-501ac539d1ea", VernacularName = "Topskarv", Parent = tGulosus };
        var tMicrocarboPygmaeus = new Taxon() { TaxonId = 38, dwc_TaxonID = "MSTSNM:Arter:b17392a6-9d0e-486b-91ed-b18b00d742e2", VernacularName = "Dværgskarv", Parent = tMicrocarbo };
        var tPhalacrocoraxCarbo = new Taxon() { TaxonId = 39, dwc_TaxonID = "MSTSNM:Arter:c5f52bf9-f785-ea11-aa77-501ac539d1ea", VernacularName = "Skarv", Parent = tPhalacrocorax };
        var tMorusBassanus = new Taxon() { TaxonId = 40, dwc_TaxonID = "MSTSNM:Arter:dc60ddf8-f785-ea11-aa77-501ac539d1ea", VernacularName = "Sule", Parent = tMorus };
        var tSulaLeucogaster = new Taxon() { TaxonId = 41, dwc_TaxonID = "MSTSNM:Arter:28d4148a-200d-4a20-a727-adf600a57270", VernacularName = "Brun sule", Parent = tSula };
        var tGeronticusEremita = new Taxon() { TaxonId = 42, dwc_TaxonID = "MSTSNM:Arter:4b50c87f-f904-4fc0-8bb4-b0b500cd367d", VernacularName = "Eremitibis", Parent = tGeronticus };
        var tPlataleaLeucorodia = new Taxon() { TaxonId = 43, dwc_TaxonID = "MSTSNM:Arter:f3fa2bf9-f785-ea11-aa77-501ac539d1ea", VernacularName = "Skestork", Parent = tPlatalea };
        var tPlegadisFalcinellus = new Taxon() { TaxonId = 44, dwc_TaxonID = "MSTSNM:Arter:3efc2bf9-f785-ea11-aa77-501ac539d1ea", VernacularName = "Sort ibis", Parent = tPlegadis };
        var tPhalacrocoraxCarboCarbo = new Taxon() { TaxonId = 45, dwc_TaxonID = "MSTSNM:Arter:c3cdcafa-f785-ea11-aa77-501ac539d1ea", VernacularName = "Storskarv", Parent = tPhalacrocoraxCarbo };
        var tPhalacrocoraxCarboSinensis = new Taxon() { TaxonId = 46, dwc_TaxonID = "MSTSNM:Arter:c4cdcafa-f785-ea11-aa77-501ac539d1ea", VernacularName = "Mellemskarv", Parent = tPhalacrocoraxCarbo };

        // Arrange: Author copied from DbInitializer
        var a1 = new Author() { AuthorId = 1, Name = "Roger Histand", Email = "Roger+Histand@hotmail.com", Posts = new List<Post>() };

        // Arrange: Create observations
        var p1 = new Observation { Author = a1, Text = "", Taxon = tPhalacrocoraxCarboSinensis };
        var p2 = new Observation { Author = a1, Text = "", Taxon = tMorusBassanus };
        var p3 = new Observation { Author = a1, Text = "", Taxon = tPlataleaLeucorodia };

        // Arrange: Create observation sequence
        ISequence<Observation> dafnySequence = Sequence<Observation>.FromElements(p1, p2, p3);

        // Act: Filter
        ISequence<Observation> filteredSequence = TaxonFilterWrapper.__default.FilterByWrapped(tPelecaniformes, dafnySequence);

        // Assert: 
        Assert.Equal(dafnySequence, filteredSequence);
    }

    [Fact]
    public void FilterBy_tArdea_Returns_Children()
    {
        // Arrange: Taxons copied from DbInitializer
        var tPelecaniformes = new Taxon() { TaxonId = 1, dwc_TaxonID = "MSTSNM:Arter:3e4e67e4-f785-ea11-aa77-501ac539d1ea", VernacularName = "Årefodede", Parent = null };
        var tArdeidae = new Taxon() { TaxonId = 2, dwc_TaxonID = "MSTSNM:Arter:495067e4-f785-ea11-aa77-501ac539d1ea", VernacularName = "Hejrer", Parent = tPelecaniformes };
        var tFregatidae = new Taxon() { TaxonId = 3, dwc_TaxonID = "MSTSNM:Arter:a15367e4-f785-ea11-aa77-501ac539d1ea", VernacularName = "Fregatfugle", Parent = tPelecaniformes };
        var tPelecanidae = new Taxon() { TaxonId = 4, dwc_TaxonID = "MSTSNM:Arter:025767e4-f785-ea11-aa77-501ac539d1ea", VernacularName = "Pelikaner", Parent = tPelecaniformes };
        var tPhalacrocoracidae = new Taxon() { TaxonId = 5, dwc_TaxonID = "MSTSNM:Arter:2a5767e4-f785-ea11-aa77-501ac539d1ea", VernacularName = "Skarver", Parent = tPelecaniformes };
        var tSulidae = new Taxon() { TaxonId = 6, dwc_TaxonID = "MSTSNM:Arter:5c5967e4-f785-ea11-aa77-501ac539d1ea", VernacularName = "Suler", Parent = tPelecaniformes };
        var tThreskiornithidae = new Taxon() { TaxonId = 7, dwc_TaxonID = "MSTSNM:Arter:af5967e4-f785-ea11-aa77-501ac539d1ea", VernacularName = "Ibiser & skestorke", Parent = tPelecaniformes };
        var tArdea = new Taxon() { TaxonId = 8, dwc_TaxonID = "MSTSNM:Arter:7f9ef9f3-f785-ea11-aa77-501ac539d1ea", VernacularName = null, Parent = tArdeidae };
        var tArdeola = new Taxon() { TaxonId = 9, dwc_TaxonID = "MSTSNM:Arter:819ef9f3-f785-ea11-aa77-501ac539d1ea", VernacularName = null, Parent = tArdeidae };
        var tBotaurus = new Taxon() { TaxonId = 10, dwc_TaxonID = "MSTSNM:Arter:dca0f9f3-f785-ea11-aa77-501ac539d1ea", VernacularName = null, Parent = tArdeidae };
        var tBubulcus = new Taxon() { TaxonId = 11, dwc_TaxonID = "MSTSNM:Arter:74a1f9f3-f785-ea11-aa77-501ac539d1ea", VernacularName = null, Parent = tArdeidae };
        var tEgretta = new Taxon() { TaxonId = 12, dwc_TaxonID = "MSTSNM:Arter:25aaf9f3-f785-ea11-aa77-501ac539d1ea", VernacularName = null, Parent = tArdeidae };
        var tIxobrychus = new Taxon() { TaxonId = 13, dwc_TaxonID = "MSTSNM:Arter:eeb1f9f3-f785-ea11-aa77-501ac539d1ea", VernacularName = null, Parent = tArdeidae };
        var tNycticorax = new Taxon() { TaxonId = 14, dwc_TaxonID = "MSTSNM:Arter:fab9f9f3-f785-ea11-aa77-501ac539d1ea", VernacularName = null, Parent = tArdeidae };
        var tFregata = new Taxon() { TaxonId = 15, dwc_TaxonID = "MSTSNM:Arter:0aadf9f3-f785-ea11-aa77-501ac539d1ea", VernacularName = null, Parent = tFregatidae };
        var tPelecanus = new Taxon() { TaxonId = 16, dwc_TaxonID = "MSTSNM:Arter:0bbdf9f3-f785-ea11-aa77-501ac539d1ea", VernacularName = null, Parent = tPelecanidae };
        var tGulosus = new Taxon() { TaxonId = 17, dwc_TaxonID = "MSTSNM:Arter:701cda16-1f30-4973-b36b-af7e00fd7609", VernacularName = null, Parent = tPhalacrocoracidae };
        var tMicrocarbo = new Taxon() { TaxonId = 18, dwc_TaxonID = "MSTSNM:Arter:cb487af6-7074-427a-a7cb-b20900a28376", VernacularName = null, Parent = tPhalacrocoracidae };
        var tPhalacrocorax = new Taxon() { TaxonId = 19, dwc_TaxonID = "MSTSNM:Arter:c5bdf9f3-f785-ea11-aa77-501ac539d1ea", VernacularName = null, Parent = tPhalacrocoracidae };
        var tMorus = new Taxon() { TaxonId = 20, dwc_TaxonID = "MSTSNM:Arter:cb50a8dd-d411-4bc3-869b-ac2800d7e676", VernacularName = null, Parent = tSulidae };
        var tSula = new Taxon() { TaxonId = 21, dwc_TaxonID = "MSTSNM:Arter:dfcd4cd2-9f34-4122-a30e-adf600a544fb", VernacularName = null, Parent = tSulidae };
        var tGeronticus = new Taxon() { TaxonId = 22, dwc_TaxonID = "MSTSNM:Arter:ef2c03c2-1064-461b-9b3e-b0b500cd20d7", VernacularName = null, Parent = tThreskiornithidae };
        var tPlatalea = new Taxon() { TaxonId = 23, dwc_TaxonID = "MSTSNM:Arter:33bff9f3-f785-ea11-aa77-501ac539d1ea", VernacularName = null, Parent = tThreskiornithidae };
        var tPlegadis = new Taxon() { TaxonId = 24, dwc_TaxonID = "MSTSNM:Arter:7dbff9f3-f785-ea11-aa77-501ac539d1ea", VernacularName = null, Parent = tThreskiornithidae };
        var tArdeaAlba = new Taxon() { TaxonId = 25, dwc_TaxonID = "MSTSNM:Arter:c18811f4-f785-ea11-aa77-501ac539d1ea", VernacularName = "Sølvhejre", Parent = tArdea };
        var tArdeaCinerea = new Taxon() { TaxonId = 26, dwc_TaxonID = "MSTSNM:Arter:c28811f4-f785-ea11-aa77-501ac539d1ea", VernacularName = "Fiskehejre", Parent = tArdea };
        var tArdeaIbis = new Taxon() { TaxonId = 27, dwc_TaxonID = "MSTSNM:Arter:f69111f4-f785-ea11-aa77-501ac539d1ea", VernacularName = "Kohejre", Parent = tArdea };
        var tArdeaPurpurea = new Taxon() { TaxonId = 28, dwc_TaxonID = "MSTSNM:Arter:c38811f4-f785-ea11-aa77-501ac539d1ea", VernacularName = "Purpurhejre", Parent = tArdea };
        var tArdeolaRalloides = new Taxon() { TaxonId = 29, dwc_TaxonID = "MSTSNM:Arter:c68811f4-f785-ea11-aa77-501ac539d1ea", VernacularName = "Tophejre", Parent = tArdeola };
        var tBotaurusLentiginosus = new Taxon() { TaxonId = 30, dwc_TaxonID = "MSTSNM:Arter:2c9011f4-f785-ea11-aa77-501ac539d1ea", VernacularName = "Amerikansk rørdrum", Parent = tBotaurus };
        var tBotaurusMinutus = new Taxon() { TaxonId = 31, dwc_TaxonID = "MSTSNM:Arter:f54eddf8-f785-ea11-aa77-501ac539d1ea", VernacularName = "Dværghejre", Parent = tBotaurus };
        var tBotaurusStellaris = new Taxon() { TaxonId = 32, dwc_TaxonID = "MSTSNM:Arter:2d9011f4-f785-ea11-aa77-501ac539d1ea", VernacularName = "Rørdrum", Parent = tBotaurus };
        var tEgrettaGarzetta = new Taxon() { TaxonId = 33, dwc_TaxonID = "MSTSNM:Arter:ca4d8cf8-f785-ea11-aa77-501ac539d1ea", VernacularName = "Silkehejre", Parent = tEgretta };
        var tNycticoraxNycticorax = new Taxon() { TaxonId = 34, dwc_TaxonID = "MSTSNM:Arter:2d66ddf8-f785-ea11-aa77-501ac539d1ea", VernacularName = "Nathejre", Parent = tNycticorax };
        var tFregataMagnificens = new Taxon() { TaxonId = 35, dwc_TaxonID = "MSTSNM:Arter:7b578cf8-f785-ea11-aa77-501ac539d1ea", VernacularName = "Pragtfregatfugl", Parent = tFregata };
        var tPelecanusCrispus = new Taxon() { TaxonId = 36, dwc_TaxonID = "MSTSNM:Arter:ee6dddf8-f785-ea11-aa77-501ac539d1ea", VernacularName = "Krøltoppet pelikan", Parent = tPelecanus };
        var tGulosusAristotelis = new Taxon() { TaxonId = 37, dwc_TaxonID = "MSTSNM:Arter:c4f52bf9-f785-ea11-aa77-501ac539d1ea", VernacularName = "Topskarv", Parent = tGulosus };
        var tMicrocarboPygmaeus = new Taxon() { TaxonId = 38, dwc_TaxonID = "MSTSNM:Arter:b17392a6-9d0e-486b-91ed-b18b00d742e2", VernacularName = "Dværgskarv", Parent = tMicrocarbo };
        var tPhalacrocoraxCarbo = new Taxon() { TaxonId = 39, dwc_TaxonID = "MSTSNM:Arter:c5f52bf9-f785-ea11-aa77-501ac539d1ea", VernacularName = "Skarv", Parent = tPhalacrocorax };
        var tMorusBassanus = new Taxon() { TaxonId = 40, dwc_TaxonID = "MSTSNM:Arter:dc60ddf8-f785-ea11-aa77-501ac539d1ea", VernacularName = "Sule", Parent = tMorus };
        var tSulaLeucogaster = new Taxon() { TaxonId = 41, dwc_TaxonID = "MSTSNM:Arter:28d4148a-200d-4a20-a727-adf600a57270", VernacularName = "Brun sule", Parent = tSula };
        var tGeronticusEremita = new Taxon() { TaxonId = 42, dwc_TaxonID = "MSTSNM:Arter:4b50c87f-f904-4fc0-8bb4-b0b500cd367d", VernacularName = "Eremitibis", Parent = tGeronticus };
        var tPlataleaLeucorodia = new Taxon() { TaxonId = 43, dwc_TaxonID = "MSTSNM:Arter:f3fa2bf9-f785-ea11-aa77-501ac539d1ea", VernacularName = "Skestork", Parent = tPlatalea };
        var tPlegadisFalcinellus = new Taxon() { TaxonId = 44, dwc_TaxonID = "MSTSNM:Arter:3efc2bf9-f785-ea11-aa77-501ac539d1ea", VernacularName = "Sort ibis", Parent = tPlegadis };
        var tPhalacrocoraxCarboCarbo = new Taxon() { TaxonId = 45, dwc_TaxonID = "MSTSNM:Arter:c3cdcafa-f785-ea11-aa77-501ac539d1ea", VernacularName = "Storskarv", Parent = tPhalacrocoraxCarbo };
        var tPhalacrocoraxCarboSinensis = new Taxon() { TaxonId = 46, dwc_TaxonID = "MSTSNM:Arter:c4cdcafa-f785-ea11-aa77-501ac539d1ea", VernacularName = "Mellemskarv", Parent = tPhalacrocoraxCarbo };

        // Arrange: Author copied from DbInitializer
        var a1 = new Author() { AuthorId = 1, Name = "Roger Histand", Email = "Roger+Histand@hotmail.com", Posts = new List<Post>() };

        // Arrange: Create observations
        var p1 = new Observation { Author = a1, Text = "", Taxon = tPhalacrocoraxCarboSinensis };
        var p2 = new Observation { Author = a1, Text = "", Taxon = tMorusBassanus };
        var p3 = new Observation { Author = a1, Text = "", Taxon = tPlataleaLeucorodia };
        var p4 = new Observation { Author = a1, Text = "", Taxon = tArdeaAlba };
        var p5 = new Observation { Author = a1, Text = "", Taxon = tArdeaCinerea };
        var p6 = new Observation { Author = a1, Text = "", Taxon = tArdeaIbis };
        var p7 = new Observation { Author = a1, Text = "", Taxon = tArdeaPurpurea };

        // Arrange: Create observation sequence
        ISequence<Observation> dafnySequence = Sequence<Observation>.FromElements(p1, p2, p3, p4, p5, p6, p7);

        // Act: Filter
        ISequence<Observation> filteredSequence = TaxonFilterWrapper.__default.FilterByWrapped(tArdea, dafnySequence);

        // Assert: Not equal to original sequence
        Assert.NotEqual(dafnySequence, filteredSequence);

        // Assert: Size equal to expected size
        int expectedSize = 4;
        Assert.Equal(expectedSize, filteredSequence.Count);
    }

    [Fact]
    public void FilterBy_Non_Parent_Returns_None()
    {
        // Arrange: Taxons copied from DbInitializer
        var tPelecaniformes = new Taxon() { TaxonId = 1, dwc_TaxonID = "MSTSNM:Arter:3e4e67e4-f785-ea11-aa77-501ac539d1ea", VernacularName = "Årefodede", Parent = null };
        var tArdeidae = new Taxon() { TaxonId = 2, dwc_TaxonID = "MSTSNM:Arter:495067e4-f785-ea11-aa77-501ac539d1ea", VernacularName = "Hejrer", Parent = tPelecaniformes };
        var tFregatidae = new Taxon() { TaxonId = 3, dwc_TaxonID = "MSTSNM:Arter:a15367e4-f785-ea11-aa77-501ac539d1ea", VernacularName = "Fregatfugle", Parent = tPelecaniformes };
        var tPelecanidae = new Taxon() { TaxonId = 4, dwc_TaxonID = "MSTSNM:Arter:025767e4-f785-ea11-aa77-501ac539d1ea", VernacularName = "Pelikaner", Parent = tPelecaniformes };
        var tPhalacrocoracidae = new Taxon() { TaxonId = 5, dwc_TaxonID = "MSTSNM:Arter:2a5767e4-f785-ea11-aa77-501ac539d1ea", VernacularName = "Skarver", Parent = tPelecaniformes };
        var tSulidae = new Taxon() { TaxonId = 6, dwc_TaxonID = "MSTSNM:Arter:5c5967e4-f785-ea11-aa77-501ac539d1ea", VernacularName = "Suler", Parent = tPelecaniformes };
        var tThreskiornithidae = new Taxon() { TaxonId = 7, dwc_TaxonID = "MSTSNM:Arter:af5967e4-f785-ea11-aa77-501ac539d1ea", VernacularName = "Ibiser & skestorke", Parent = tPelecaniformes };
        var tArdea = new Taxon() { TaxonId = 8, dwc_TaxonID = "MSTSNM:Arter:7f9ef9f3-f785-ea11-aa77-501ac539d1ea", VernacularName = null, Parent = tArdeidae };
        var tArdeola = new Taxon() { TaxonId = 9, dwc_TaxonID = "MSTSNM:Arter:819ef9f3-f785-ea11-aa77-501ac539d1ea", VernacularName = null, Parent = tArdeidae };
        var tBotaurus = new Taxon() { TaxonId = 10, dwc_TaxonID = "MSTSNM:Arter:dca0f9f3-f785-ea11-aa77-501ac539d1ea", VernacularName = null, Parent = tArdeidae };
        var tBubulcus = new Taxon() { TaxonId = 11, dwc_TaxonID = "MSTSNM:Arter:74a1f9f3-f785-ea11-aa77-501ac539d1ea", VernacularName = null, Parent = tArdeidae };
        var tEgretta = new Taxon() { TaxonId = 12, dwc_TaxonID = "MSTSNM:Arter:25aaf9f3-f785-ea11-aa77-501ac539d1ea", VernacularName = null, Parent = tArdeidae };
        var tIxobrychus = new Taxon() { TaxonId = 13, dwc_TaxonID = "MSTSNM:Arter:eeb1f9f3-f785-ea11-aa77-501ac539d1ea", VernacularName = null, Parent = tArdeidae };
        var tNycticorax = new Taxon() { TaxonId = 14, dwc_TaxonID = "MSTSNM:Arter:fab9f9f3-f785-ea11-aa77-501ac539d1ea", VernacularName = null, Parent = tArdeidae };
        var tFregata = new Taxon() { TaxonId = 15, dwc_TaxonID = "MSTSNM:Arter:0aadf9f3-f785-ea11-aa77-501ac539d1ea", VernacularName = null, Parent = tFregatidae };
        var tPelecanus = new Taxon() { TaxonId = 16, dwc_TaxonID = "MSTSNM:Arter:0bbdf9f3-f785-ea11-aa77-501ac539d1ea", VernacularName = null, Parent = tPelecanidae };
        var tGulosus = new Taxon() { TaxonId = 17, dwc_TaxonID = "MSTSNM:Arter:701cda16-1f30-4973-b36b-af7e00fd7609", VernacularName = null, Parent = tPhalacrocoracidae };
        var tMicrocarbo = new Taxon() { TaxonId = 18, dwc_TaxonID = "MSTSNM:Arter:cb487af6-7074-427a-a7cb-b20900a28376", VernacularName = null, Parent = tPhalacrocoracidae };
        var tPhalacrocorax = new Taxon() { TaxonId = 19, dwc_TaxonID = "MSTSNM:Arter:c5bdf9f3-f785-ea11-aa77-501ac539d1ea", VernacularName = null, Parent = tPhalacrocoracidae };
        var tMorus = new Taxon() { TaxonId = 20, dwc_TaxonID = "MSTSNM:Arter:cb50a8dd-d411-4bc3-869b-ac2800d7e676", VernacularName = null, Parent = tSulidae };
        var tSula = new Taxon() { TaxonId = 21, dwc_TaxonID = "MSTSNM:Arter:dfcd4cd2-9f34-4122-a30e-adf600a544fb", VernacularName = null, Parent = tSulidae };
        var tGeronticus = new Taxon() { TaxonId = 22, dwc_TaxonID = "MSTSNM:Arter:ef2c03c2-1064-461b-9b3e-b0b500cd20d7", VernacularName = null, Parent = tThreskiornithidae };
        var tPlatalea = new Taxon() { TaxonId = 23, dwc_TaxonID = "MSTSNM:Arter:33bff9f3-f785-ea11-aa77-501ac539d1ea", VernacularName = null, Parent = tThreskiornithidae };
        var tPlegadis = new Taxon() { TaxonId = 24, dwc_TaxonID = "MSTSNM:Arter:7dbff9f3-f785-ea11-aa77-501ac539d1ea", VernacularName = null, Parent = tThreskiornithidae };
        var tArdeaAlba = new Taxon() { TaxonId = 25, dwc_TaxonID = "MSTSNM:Arter:c18811f4-f785-ea11-aa77-501ac539d1ea", VernacularName = "Sølvhejre", Parent = tArdea };
        var tArdeaCinerea = new Taxon() { TaxonId = 26, dwc_TaxonID = "MSTSNM:Arter:c28811f4-f785-ea11-aa77-501ac539d1ea", VernacularName = "Fiskehejre", Parent = tArdea };
        var tArdeaIbis = new Taxon() { TaxonId = 27, dwc_TaxonID = "MSTSNM:Arter:f69111f4-f785-ea11-aa77-501ac539d1ea", VernacularName = "Kohejre", Parent = tArdea };
        var tArdeaPurpurea = new Taxon() { TaxonId = 28, dwc_TaxonID = "MSTSNM:Arter:c38811f4-f785-ea11-aa77-501ac539d1ea", VernacularName = "Purpurhejre", Parent = tArdea };
        var tArdeolaRalloides = new Taxon() { TaxonId = 29, dwc_TaxonID = "MSTSNM:Arter:c68811f4-f785-ea11-aa77-501ac539d1ea", VernacularName = "Tophejre", Parent = tArdeola };
        var tBotaurusLentiginosus = new Taxon() { TaxonId = 30, dwc_TaxonID = "MSTSNM:Arter:2c9011f4-f785-ea11-aa77-501ac539d1ea", VernacularName = "Amerikansk rørdrum", Parent = tBotaurus };
        var tBotaurusMinutus = new Taxon() { TaxonId = 31, dwc_TaxonID = "MSTSNM:Arter:f54eddf8-f785-ea11-aa77-501ac539d1ea", VernacularName = "Dværghejre", Parent = tBotaurus };
        var tBotaurusStellaris = new Taxon() { TaxonId = 32, dwc_TaxonID = "MSTSNM:Arter:2d9011f4-f785-ea11-aa77-501ac539d1ea", VernacularName = "Rørdrum", Parent = tBotaurus };
        var tEgrettaGarzetta = new Taxon() { TaxonId = 33, dwc_TaxonID = "MSTSNM:Arter:ca4d8cf8-f785-ea11-aa77-501ac539d1ea", VernacularName = "Silkehejre", Parent = tEgretta };
        var tNycticoraxNycticorax = new Taxon() { TaxonId = 34, dwc_TaxonID = "MSTSNM:Arter:2d66ddf8-f785-ea11-aa77-501ac539d1ea", VernacularName = "Nathejre", Parent = tNycticorax };
        var tFregataMagnificens = new Taxon() { TaxonId = 35, dwc_TaxonID = "MSTSNM:Arter:7b578cf8-f785-ea11-aa77-501ac539d1ea", VernacularName = "Pragtfregatfugl", Parent = tFregata };
        var tPelecanusCrispus = new Taxon() { TaxonId = 36, dwc_TaxonID = "MSTSNM:Arter:ee6dddf8-f785-ea11-aa77-501ac539d1ea", VernacularName = "Krøltoppet pelikan", Parent = tPelecanus };
        var tGulosusAristotelis = new Taxon() { TaxonId = 37, dwc_TaxonID = "MSTSNM:Arter:c4f52bf9-f785-ea11-aa77-501ac539d1ea", VernacularName = "Topskarv", Parent = tGulosus };
        var tMicrocarboPygmaeus = new Taxon() { TaxonId = 38, dwc_TaxonID = "MSTSNM:Arter:b17392a6-9d0e-486b-91ed-b18b00d742e2", VernacularName = "Dværgskarv", Parent = tMicrocarbo };
        var tPhalacrocoraxCarbo = new Taxon() { TaxonId = 39, dwc_TaxonID = "MSTSNM:Arter:c5f52bf9-f785-ea11-aa77-501ac539d1ea", VernacularName = "Skarv", Parent = tPhalacrocorax };
        var tMorusBassanus = new Taxon() { TaxonId = 40, dwc_TaxonID = "MSTSNM:Arter:dc60ddf8-f785-ea11-aa77-501ac539d1ea", VernacularName = "Sule", Parent = tMorus };
        var tSulaLeucogaster = new Taxon() { TaxonId = 41, dwc_TaxonID = "MSTSNM:Arter:28d4148a-200d-4a20-a727-adf600a57270", VernacularName = "Brun sule", Parent = tSula };
        var tGeronticusEremita = new Taxon() { TaxonId = 42, dwc_TaxonID = "MSTSNM:Arter:4b50c87f-f904-4fc0-8bb4-b0b500cd367d", VernacularName = "Eremitibis", Parent = tGeronticus };
        var tPlataleaLeucorodia = new Taxon() { TaxonId = 43, dwc_TaxonID = "MSTSNM:Arter:f3fa2bf9-f785-ea11-aa77-501ac539d1ea", VernacularName = "Skestork", Parent = tPlatalea };
        var tPlegadisFalcinellus = new Taxon() { TaxonId = 44, dwc_TaxonID = "MSTSNM:Arter:3efc2bf9-f785-ea11-aa77-501ac539d1ea", VernacularName = "Sort ibis", Parent = tPlegadis };
        var tPhalacrocoraxCarboCarbo = new Taxon() { TaxonId = 45, dwc_TaxonID = "MSTSNM:Arter:c3cdcafa-f785-ea11-aa77-501ac539d1ea", VernacularName = "Storskarv", Parent = tPhalacrocoraxCarbo };
        var tPhalacrocoraxCarboSinensis = new Taxon() { TaxonId = 46, dwc_TaxonID = "MSTSNM:Arter:c4cdcafa-f785-ea11-aa77-501ac539d1ea", VernacularName = "Mellemskarv", Parent = tPhalacrocoraxCarbo };

        // Arrange: Author copied from DbInitializer
        var a1 = new Author() { AuthorId = 1, Name = "Roger Histand", Email = "Roger+Histand@hotmail.com", Posts = new List<Post>() };

        // Arrange: Create observations
        var p1 = new Observation { Author = a1, Text = "", Taxon = tPhalacrocoraxCarboSinensis };
        var p2 = new Observation { Author = a1, Text = "", Taxon = tMorusBassanus };
        var p3 = new Observation { Author = a1, Text = "", Taxon = tPlataleaLeucorodia };
        var p4 = new Observation { Author = a1, Text = "", Taxon = tArdeaAlba };
        var p5 = new Observation { Author = a1, Text = "", Taxon = tArdeaCinerea };
        var p6 = new Observation { Author = a1, Text = "", Taxon = tArdeaIbis };
        var p7 = new Observation { Author = a1, Text = "", Taxon = tArdeaPurpurea };

        // Arrange: Create observation sequence
        ISequence<Observation> dafnySequence = Sequence<Observation>.FromElements(p1, p2, p3, p4, p5, p6, p7);

        // Act: Filter
        ISequence<Observation> filteredSequence = TaxonFilterWrapper.__default.FilterByWrapped(tBubulcus, dafnySequence);

        // Assert: Not equal to original sequence
        Assert.NotEqual(dafnySequence, filteredSequence);

        // Assert: Size equal to expected size
        int expectedSize = 0;
        Assert.Equal(expectedSize, filteredSequence.Count);
    }

    [Fact]
    public void FilterBy_Child_NotReturns_Parent()
    {
        // Arrange: Taxons copied from DbInitializer
        var tPelecaniformes = new Taxon() { TaxonId = 1, dwc_TaxonID = "MSTSNM:Arter:3e4e67e4-f785-ea11-aa77-501ac539d1ea", VernacularName = "Årefodede", Parent = null };
        var tArdeidae = new Taxon() { TaxonId = 2, dwc_TaxonID = "MSTSNM:Arter:495067e4-f785-ea11-aa77-501ac539d1ea", VernacularName = "Hejrer", Parent = tPelecaniformes };
        var tFregatidae = new Taxon() { TaxonId = 3, dwc_TaxonID = "MSTSNM:Arter:a15367e4-f785-ea11-aa77-501ac539d1ea", VernacularName = "Fregatfugle", Parent = tPelecaniformes };
        var tPelecanidae = new Taxon() { TaxonId = 4, dwc_TaxonID = "MSTSNM:Arter:025767e4-f785-ea11-aa77-501ac539d1ea", VernacularName = "Pelikaner", Parent = tPelecaniformes };
        var tPhalacrocoracidae = new Taxon() { TaxonId = 5, dwc_TaxonID = "MSTSNM:Arter:2a5767e4-f785-ea11-aa77-501ac539d1ea", VernacularName = "Skarver", Parent = tPelecaniformes };
        var tSulidae = new Taxon() { TaxonId = 6, dwc_TaxonID = "MSTSNM:Arter:5c5967e4-f785-ea11-aa77-501ac539d1ea", VernacularName = "Suler", Parent = tPelecaniformes };
        var tThreskiornithidae = new Taxon() { TaxonId = 7, dwc_TaxonID = "MSTSNM:Arter:af5967e4-f785-ea11-aa77-501ac539d1ea", VernacularName = "Ibiser & skestorke", Parent = tPelecaniformes };
        var tArdea = new Taxon() { TaxonId = 8, dwc_TaxonID = "MSTSNM:Arter:7f9ef9f3-f785-ea11-aa77-501ac539d1ea", VernacularName = null, Parent = tArdeidae };
        var tArdeola = new Taxon() { TaxonId = 9, dwc_TaxonID = "MSTSNM:Arter:819ef9f3-f785-ea11-aa77-501ac539d1ea", VernacularName = null, Parent = tArdeidae };
        var tBotaurus = new Taxon() { TaxonId = 10, dwc_TaxonID = "MSTSNM:Arter:dca0f9f3-f785-ea11-aa77-501ac539d1ea", VernacularName = null, Parent = tArdeidae };
        var tBubulcus = new Taxon() { TaxonId = 11, dwc_TaxonID = "MSTSNM:Arter:74a1f9f3-f785-ea11-aa77-501ac539d1ea", VernacularName = null, Parent = tArdeidae };
        var tEgretta = new Taxon() { TaxonId = 12, dwc_TaxonID = "MSTSNM:Arter:25aaf9f3-f785-ea11-aa77-501ac539d1ea", VernacularName = null, Parent = tArdeidae };
        var tIxobrychus = new Taxon() { TaxonId = 13, dwc_TaxonID = "MSTSNM:Arter:eeb1f9f3-f785-ea11-aa77-501ac539d1ea", VernacularName = null, Parent = tArdeidae };
        var tNycticorax = new Taxon() { TaxonId = 14, dwc_TaxonID = "MSTSNM:Arter:fab9f9f3-f785-ea11-aa77-501ac539d1ea", VernacularName = null, Parent = tArdeidae };
        var tFregata = new Taxon() { TaxonId = 15, dwc_TaxonID = "MSTSNM:Arter:0aadf9f3-f785-ea11-aa77-501ac539d1ea", VernacularName = null, Parent = tFregatidae };
        var tPelecanus = new Taxon() { TaxonId = 16, dwc_TaxonID = "MSTSNM:Arter:0bbdf9f3-f785-ea11-aa77-501ac539d1ea", VernacularName = null, Parent = tPelecanidae };
        var tGulosus = new Taxon() { TaxonId = 17, dwc_TaxonID = "MSTSNM:Arter:701cda16-1f30-4973-b36b-af7e00fd7609", VernacularName = null, Parent = tPhalacrocoracidae };
        var tMicrocarbo = new Taxon() { TaxonId = 18, dwc_TaxonID = "MSTSNM:Arter:cb487af6-7074-427a-a7cb-b20900a28376", VernacularName = null, Parent = tPhalacrocoracidae };
        var tPhalacrocorax = new Taxon() { TaxonId = 19, dwc_TaxonID = "MSTSNM:Arter:c5bdf9f3-f785-ea11-aa77-501ac539d1ea", VernacularName = null, Parent = tPhalacrocoracidae };
        var tMorus = new Taxon() { TaxonId = 20, dwc_TaxonID = "MSTSNM:Arter:cb50a8dd-d411-4bc3-869b-ac2800d7e676", VernacularName = null, Parent = tSulidae };
        var tSula = new Taxon() { TaxonId = 21, dwc_TaxonID = "MSTSNM:Arter:dfcd4cd2-9f34-4122-a30e-adf600a544fb", VernacularName = null, Parent = tSulidae };
        var tGeronticus = new Taxon() { TaxonId = 22, dwc_TaxonID = "MSTSNM:Arter:ef2c03c2-1064-461b-9b3e-b0b500cd20d7", VernacularName = null, Parent = tThreskiornithidae };
        var tPlatalea = new Taxon() { TaxonId = 23, dwc_TaxonID = "MSTSNM:Arter:33bff9f3-f785-ea11-aa77-501ac539d1ea", VernacularName = null, Parent = tThreskiornithidae };
        var tPlegadis = new Taxon() { TaxonId = 24, dwc_TaxonID = "MSTSNM:Arter:7dbff9f3-f785-ea11-aa77-501ac539d1ea", VernacularName = null, Parent = tThreskiornithidae };
        var tArdeaAlba = new Taxon() { TaxonId = 25, dwc_TaxonID = "MSTSNM:Arter:c18811f4-f785-ea11-aa77-501ac539d1ea", VernacularName = "Sølvhejre", Parent = tArdea };
        var tArdeaCinerea = new Taxon() { TaxonId = 26, dwc_TaxonID = "MSTSNM:Arter:c28811f4-f785-ea11-aa77-501ac539d1ea", VernacularName = "Fiskehejre", Parent = tArdea };
        var tArdeaIbis = new Taxon() { TaxonId = 27, dwc_TaxonID = "MSTSNM:Arter:f69111f4-f785-ea11-aa77-501ac539d1ea", VernacularName = "Kohejre", Parent = tArdea };
        var tArdeaPurpurea = new Taxon() { TaxonId = 28, dwc_TaxonID = "MSTSNM:Arter:c38811f4-f785-ea11-aa77-501ac539d1ea", VernacularName = "Purpurhejre", Parent = tArdea };
        var tArdeolaRalloides = new Taxon() { TaxonId = 29, dwc_TaxonID = "MSTSNM:Arter:c68811f4-f785-ea11-aa77-501ac539d1ea", VernacularName = "Tophejre", Parent = tArdeola };
        var tBotaurusLentiginosus = new Taxon() { TaxonId = 30, dwc_TaxonID = "MSTSNM:Arter:2c9011f4-f785-ea11-aa77-501ac539d1ea", VernacularName = "Amerikansk rørdrum", Parent = tBotaurus };
        var tBotaurusMinutus = new Taxon() { TaxonId = 31, dwc_TaxonID = "MSTSNM:Arter:f54eddf8-f785-ea11-aa77-501ac539d1ea", VernacularName = "Dværghejre", Parent = tBotaurus };
        var tBotaurusStellaris = new Taxon() { TaxonId = 32, dwc_TaxonID = "MSTSNM:Arter:2d9011f4-f785-ea11-aa77-501ac539d1ea", VernacularName = "Rørdrum", Parent = tBotaurus };
        var tEgrettaGarzetta = new Taxon() { TaxonId = 33, dwc_TaxonID = "MSTSNM:Arter:ca4d8cf8-f785-ea11-aa77-501ac539d1ea", VernacularName = "Silkehejre", Parent = tEgretta };
        var tNycticoraxNycticorax = new Taxon() { TaxonId = 34, dwc_TaxonID = "MSTSNM:Arter:2d66ddf8-f785-ea11-aa77-501ac539d1ea", VernacularName = "Nathejre", Parent = tNycticorax };
        var tFregataMagnificens = new Taxon() { TaxonId = 35, dwc_TaxonID = "MSTSNM:Arter:7b578cf8-f785-ea11-aa77-501ac539d1ea", VernacularName = "Pragtfregatfugl", Parent = tFregata };
        var tPelecanusCrispus = new Taxon() { TaxonId = 36, dwc_TaxonID = "MSTSNM:Arter:ee6dddf8-f785-ea11-aa77-501ac539d1ea", VernacularName = "Krøltoppet pelikan", Parent = tPelecanus };
        var tGulosusAristotelis = new Taxon() { TaxonId = 37, dwc_TaxonID = "MSTSNM:Arter:c4f52bf9-f785-ea11-aa77-501ac539d1ea", VernacularName = "Topskarv", Parent = tGulosus };
        var tMicrocarboPygmaeus = new Taxon() { TaxonId = 38, dwc_TaxonID = "MSTSNM:Arter:b17392a6-9d0e-486b-91ed-b18b00d742e2", VernacularName = "Dværgskarv", Parent = tMicrocarbo };
        var tPhalacrocoraxCarbo = new Taxon() { TaxonId = 39, dwc_TaxonID = "MSTSNM:Arter:c5f52bf9-f785-ea11-aa77-501ac539d1ea", VernacularName = "Skarv", Parent = tPhalacrocorax };
        var tMorusBassanus = new Taxon() { TaxonId = 40, dwc_TaxonID = "MSTSNM:Arter:dc60ddf8-f785-ea11-aa77-501ac539d1ea", VernacularName = "Sule", Parent = tMorus };
        var tSulaLeucogaster = new Taxon() { TaxonId = 41, dwc_TaxonID = "MSTSNM:Arter:28d4148a-200d-4a20-a727-adf600a57270", VernacularName = "Brun sule", Parent = tSula };
        var tGeronticusEremita = new Taxon() { TaxonId = 42, dwc_TaxonID = "MSTSNM:Arter:4b50c87f-f904-4fc0-8bb4-b0b500cd367d", VernacularName = "Eremitibis", Parent = tGeronticus };
        var tPlataleaLeucorodia = new Taxon() { TaxonId = 43, dwc_TaxonID = "MSTSNM:Arter:f3fa2bf9-f785-ea11-aa77-501ac539d1ea", VernacularName = "Skestork", Parent = tPlatalea };
        var tPlegadisFalcinellus = new Taxon() { TaxonId = 44, dwc_TaxonID = "MSTSNM:Arter:3efc2bf9-f785-ea11-aa77-501ac539d1ea", VernacularName = "Sort ibis", Parent = tPlegadis };
        var tPhalacrocoraxCarboCarbo = new Taxon() { TaxonId = 45, dwc_TaxonID = "MSTSNM:Arter:c3cdcafa-f785-ea11-aa77-501ac539d1ea", VernacularName = "Storskarv", Parent = tPhalacrocoraxCarbo };
        var tPhalacrocoraxCarboSinensis = new Taxon() { TaxonId = 46, dwc_TaxonID = "MSTSNM:Arter:c4cdcafa-f785-ea11-aa77-501ac539d1ea", VernacularName = "Mellemskarv", Parent = tPhalacrocoraxCarbo };

        // Arrange: Author copied from DbInitializer
        var a1 = new Author() { AuthorId = 1, Name = "Roger Histand", Email = "Roger+Histand@hotmail.com", Posts = new List<Post>() };

        // Arrange: Create observations
        var p1 = new Observation { Author = a1, Text = "", Taxon = tPhalacrocoraxCarbo };

        // Arrange: Create observation sequence
        ISequence<Observation> dafnySequence = Sequence<Observation>.FromElements(p1);

        // Act: Filter
        ISequence<Observation> filteredSequence = TaxonFilterWrapper.__default.FilterByWrapped(tPhalacrocoraxCarboSinensis, dafnySequence);

        // Assert: Not equal to original sequence
        Assert.NotEqual(dafnySequence, filteredSequence);

        // Assert: Size equal to expected size
        int expectedSize = 0;
        Assert.Equal(expectedSize, filteredSequence.Count);
    }

    [Fact]
    public void FilterBy_Grandparent_Returns_Grandchild()
    {
        // Arrange: Taxons copied from DbInitializer
        var tPelecaniformes = new Taxon() { TaxonId = 1, dwc_TaxonID = "MSTSNM:Arter:3e4e67e4-f785-ea11-aa77-501ac539d1ea", VernacularName = "Årefodede", Parent = null };
        var tArdeidae = new Taxon() { TaxonId = 2, dwc_TaxonID = "MSTSNM:Arter:495067e4-f785-ea11-aa77-501ac539d1ea", VernacularName = "Hejrer", Parent = tPelecaniformes };
        var tFregatidae = new Taxon() { TaxonId = 3, dwc_TaxonID = "MSTSNM:Arter:a15367e4-f785-ea11-aa77-501ac539d1ea", VernacularName = "Fregatfugle", Parent = tPelecaniformes };
        var tPelecanidae = new Taxon() { TaxonId = 4, dwc_TaxonID = "MSTSNM:Arter:025767e4-f785-ea11-aa77-501ac539d1ea", VernacularName = "Pelikaner", Parent = tPelecaniformes };
        var tPhalacrocoracidae = new Taxon() { TaxonId = 5, dwc_TaxonID = "MSTSNM:Arter:2a5767e4-f785-ea11-aa77-501ac539d1ea", VernacularName = "Skarver", Parent = tPelecaniformes };
        var tSulidae = new Taxon() { TaxonId = 6, dwc_TaxonID = "MSTSNM:Arter:5c5967e4-f785-ea11-aa77-501ac539d1ea", VernacularName = "Suler", Parent = tPelecaniformes };
        var tThreskiornithidae = new Taxon() { TaxonId = 7, dwc_TaxonID = "MSTSNM:Arter:af5967e4-f785-ea11-aa77-501ac539d1ea", VernacularName = "Ibiser & skestorke", Parent = tPelecaniformes };
        var tArdea = new Taxon() { TaxonId = 8, dwc_TaxonID = "MSTSNM:Arter:7f9ef9f3-f785-ea11-aa77-501ac539d1ea", VernacularName = null, Parent = tArdeidae };
        var tArdeola = new Taxon() { TaxonId = 9, dwc_TaxonID = "MSTSNM:Arter:819ef9f3-f785-ea11-aa77-501ac539d1ea", VernacularName = null, Parent = tArdeidae };
        var tBotaurus = new Taxon() { TaxonId = 10, dwc_TaxonID = "MSTSNM:Arter:dca0f9f3-f785-ea11-aa77-501ac539d1ea", VernacularName = null, Parent = tArdeidae };
        var tBubulcus = new Taxon() { TaxonId = 11, dwc_TaxonID = "MSTSNM:Arter:74a1f9f3-f785-ea11-aa77-501ac539d1ea", VernacularName = null, Parent = tArdeidae };
        var tEgretta = new Taxon() { TaxonId = 12, dwc_TaxonID = "MSTSNM:Arter:25aaf9f3-f785-ea11-aa77-501ac539d1ea", VernacularName = null, Parent = tArdeidae };
        var tIxobrychus = new Taxon() { TaxonId = 13, dwc_TaxonID = "MSTSNM:Arter:eeb1f9f3-f785-ea11-aa77-501ac539d1ea", VernacularName = null, Parent = tArdeidae };
        var tNycticorax = new Taxon() { TaxonId = 14, dwc_TaxonID = "MSTSNM:Arter:fab9f9f3-f785-ea11-aa77-501ac539d1ea", VernacularName = null, Parent = tArdeidae };
        var tFregata = new Taxon() { TaxonId = 15, dwc_TaxonID = "MSTSNM:Arter:0aadf9f3-f785-ea11-aa77-501ac539d1ea", VernacularName = null, Parent = tFregatidae };
        var tPelecanus = new Taxon() { TaxonId = 16, dwc_TaxonID = "MSTSNM:Arter:0bbdf9f3-f785-ea11-aa77-501ac539d1ea", VernacularName = null, Parent = tPelecanidae };
        var tGulosus = new Taxon() { TaxonId = 17, dwc_TaxonID = "MSTSNM:Arter:701cda16-1f30-4973-b36b-af7e00fd7609", VernacularName = null, Parent = tPhalacrocoracidae };
        var tMicrocarbo = new Taxon() { TaxonId = 18, dwc_TaxonID = "MSTSNM:Arter:cb487af6-7074-427a-a7cb-b20900a28376", VernacularName = null, Parent = tPhalacrocoracidae };
        var tPhalacrocorax = new Taxon() { TaxonId = 19, dwc_TaxonID = "MSTSNM:Arter:c5bdf9f3-f785-ea11-aa77-501ac539d1ea", VernacularName = null, Parent = tPhalacrocoracidae };
        var tMorus = new Taxon() { TaxonId = 20, dwc_TaxonID = "MSTSNM:Arter:cb50a8dd-d411-4bc3-869b-ac2800d7e676", VernacularName = null, Parent = tSulidae };
        var tSula = new Taxon() { TaxonId = 21, dwc_TaxonID = "MSTSNM:Arter:dfcd4cd2-9f34-4122-a30e-adf600a544fb", VernacularName = null, Parent = tSulidae };
        var tGeronticus = new Taxon() { TaxonId = 22, dwc_TaxonID = "MSTSNM:Arter:ef2c03c2-1064-461b-9b3e-b0b500cd20d7", VernacularName = null, Parent = tThreskiornithidae };
        var tPlatalea = new Taxon() { TaxonId = 23, dwc_TaxonID = "MSTSNM:Arter:33bff9f3-f785-ea11-aa77-501ac539d1ea", VernacularName = null, Parent = tThreskiornithidae };
        var tPlegadis = new Taxon() { TaxonId = 24, dwc_TaxonID = "MSTSNM:Arter:7dbff9f3-f785-ea11-aa77-501ac539d1ea", VernacularName = null, Parent = tThreskiornithidae };
        var tArdeaAlba = new Taxon() { TaxonId = 25, dwc_TaxonID = "MSTSNM:Arter:c18811f4-f785-ea11-aa77-501ac539d1ea", VernacularName = "Sølvhejre", Parent = tArdea };
        var tArdeaCinerea = new Taxon() { TaxonId = 26, dwc_TaxonID = "MSTSNM:Arter:c28811f4-f785-ea11-aa77-501ac539d1ea", VernacularName = "Fiskehejre", Parent = tArdea };
        var tArdeaIbis = new Taxon() { TaxonId = 27, dwc_TaxonID = "MSTSNM:Arter:f69111f4-f785-ea11-aa77-501ac539d1ea", VernacularName = "Kohejre", Parent = tArdea };
        var tArdeaPurpurea = new Taxon() { TaxonId = 28, dwc_TaxonID = "MSTSNM:Arter:c38811f4-f785-ea11-aa77-501ac539d1ea", VernacularName = "Purpurhejre", Parent = tArdea };
        var tArdeolaRalloides = new Taxon() { TaxonId = 29, dwc_TaxonID = "MSTSNM:Arter:c68811f4-f785-ea11-aa77-501ac539d1ea", VernacularName = "Tophejre", Parent = tArdeola };
        var tBotaurusLentiginosus = new Taxon() { TaxonId = 30, dwc_TaxonID = "MSTSNM:Arter:2c9011f4-f785-ea11-aa77-501ac539d1ea", VernacularName = "Amerikansk rørdrum", Parent = tBotaurus };
        var tBotaurusMinutus = new Taxon() { TaxonId = 31, dwc_TaxonID = "MSTSNM:Arter:f54eddf8-f785-ea11-aa77-501ac539d1ea", VernacularName = "Dværghejre", Parent = tBotaurus };
        var tBotaurusStellaris = new Taxon() { TaxonId = 32, dwc_TaxonID = "MSTSNM:Arter:2d9011f4-f785-ea11-aa77-501ac539d1ea", VernacularName = "Rørdrum", Parent = tBotaurus };
        var tEgrettaGarzetta = new Taxon() { TaxonId = 33, dwc_TaxonID = "MSTSNM:Arter:ca4d8cf8-f785-ea11-aa77-501ac539d1ea", VernacularName = "Silkehejre", Parent = tEgretta };
        var tNycticoraxNycticorax = new Taxon() { TaxonId = 34, dwc_TaxonID = "MSTSNM:Arter:2d66ddf8-f785-ea11-aa77-501ac539d1ea", VernacularName = "Nathejre", Parent = tNycticorax };
        var tFregataMagnificens = new Taxon() { TaxonId = 35, dwc_TaxonID = "MSTSNM:Arter:7b578cf8-f785-ea11-aa77-501ac539d1ea", VernacularName = "Pragtfregatfugl", Parent = tFregata };
        var tPelecanusCrispus = new Taxon() { TaxonId = 36, dwc_TaxonID = "MSTSNM:Arter:ee6dddf8-f785-ea11-aa77-501ac539d1ea", VernacularName = "Krøltoppet pelikan", Parent = tPelecanus };
        var tGulosusAristotelis = new Taxon() { TaxonId = 37, dwc_TaxonID = "MSTSNM:Arter:c4f52bf9-f785-ea11-aa77-501ac539d1ea", VernacularName = "Topskarv", Parent = tGulosus };
        var tMicrocarboPygmaeus = new Taxon() { TaxonId = 38, dwc_TaxonID = "MSTSNM:Arter:b17392a6-9d0e-486b-91ed-b18b00d742e2", VernacularName = "Dværgskarv", Parent = tMicrocarbo };
        var tPhalacrocoraxCarbo = new Taxon() { TaxonId = 39, dwc_TaxonID = "MSTSNM:Arter:c5f52bf9-f785-ea11-aa77-501ac539d1ea", VernacularName = "Skarv", Parent = tPhalacrocorax };
        var tMorusBassanus = new Taxon() { TaxonId = 40, dwc_TaxonID = "MSTSNM:Arter:dc60ddf8-f785-ea11-aa77-501ac539d1ea", VernacularName = "Sule", Parent = tMorus };
        var tSulaLeucogaster = new Taxon() { TaxonId = 41, dwc_TaxonID = "MSTSNM:Arter:28d4148a-200d-4a20-a727-adf600a57270", VernacularName = "Brun sule", Parent = tSula };
        var tGeronticusEremita = new Taxon() { TaxonId = 42, dwc_TaxonID = "MSTSNM:Arter:4b50c87f-f904-4fc0-8bb4-b0b500cd367d", VernacularName = "Eremitibis", Parent = tGeronticus };
        var tPlataleaLeucorodia = new Taxon() { TaxonId = 43, dwc_TaxonID = "MSTSNM:Arter:f3fa2bf9-f785-ea11-aa77-501ac539d1ea", VernacularName = "Skestork", Parent = tPlatalea };
        var tPlegadisFalcinellus = new Taxon() { TaxonId = 44, dwc_TaxonID = "MSTSNM:Arter:3efc2bf9-f785-ea11-aa77-501ac539d1ea", VernacularName = "Sort ibis", Parent = tPlegadis };
        var tPhalacrocoraxCarboCarbo = new Taxon() { TaxonId = 45, dwc_TaxonID = "MSTSNM:Arter:c3cdcafa-f785-ea11-aa77-501ac539d1ea", VernacularName = "Storskarv", Parent = tPhalacrocoraxCarbo };
        var tPhalacrocoraxCarboSinensis = new Taxon() { TaxonId = 46, dwc_TaxonID = "MSTSNM:Arter:c4cdcafa-f785-ea11-aa77-501ac539d1ea", VernacularName = "Mellemskarv", Parent = tPhalacrocoraxCarbo };

        // Arrange: Author copied from DbInitializer
        var a1 = new Author() { AuthorId = 1, Name = "Roger Histand", Email = "Roger+Histand@hotmail.com", Posts = new List<Post>() };

        // Arrange: Create observations
        var p1 = new Observation { Author = a1, Text = "", Taxon = tPhalacrocoraxCarboSinensis };
        var p2 = new Observation { Author = a1, Text = "", Taxon = tPlegadisFalcinellus };

        // Arrange: Create observation sequence
        ISequence<Observation> dafnySequence = Sequence<Observation>.FromElements(p1, p2);

        // Act: Filter
        ISequence<Observation> filteredSequence = TaxonFilterWrapper.__default.FilterByWrapped(tPhalacrocorax, dafnySequence);

        // Assert: Not equal to original sequence
        Assert.NotEqual(dafnySequence, filteredSequence);

        // Assert: Sequence size equal to expected size
        int expectedSize = 1;
        Assert.Equal(expectedSize, filteredSequence.Count);

        // Assert: Observation sequence contains expected element
        Assert.Equal(p1, filteredSequence.ElementAt(0));
    }
}