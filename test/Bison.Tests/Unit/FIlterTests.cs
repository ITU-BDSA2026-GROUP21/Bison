using Bison.Core.Models;
using Dafny;

namespace Bison.Tests.Unit;

// TODO: Possibly extract the taxon creation to a helper.
// These tests are for checking the Dafny generated code in Bison.Infrastructure/Dafny/Generated/TaxonFilter.cs
public class FilterTests
{
    [Fact]
    public void FilterBy_Top_Taxon_Returns_All() // A test to check that filtering by the very top taxa filters out nothing
    {
        // Arrange: Taxons copied from DbInitializer
        var tPelecaniformes = new Taxon() { TaxonId = 1, dwc_TaxonID = "MSTSNM:Arter:3e4e67e4-f785-ea11-aa77-501ac539d1ea", VernacularName = "Årefodede", Parent = null };
        var tPhalacrocoracidae = new Taxon() { TaxonId = 5, dwc_TaxonID = "MSTSNM:Arter:2a5767e4-f785-ea11-aa77-501ac539d1ea", VernacularName = "Skarver", Parent = tPelecaniformes };
        var tSulidae = new Taxon() { TaxonId = 6, dwc_TaxonID = "MSTSNM:Arter:5c5967e4-f785-ea11-aa77-501ac539d1ea", VernacularName = "Suler", Parent = tPelecaniformes };
        var tThreskiornithidae = new Taxon() { TaxonId = 7, dwc_TaxonID = "MSTSNM:Arter:af5967e4-f785-ea11-aa77-501ac539d1ea", VernacularName = "Ibiser & skestorke", Parent = tPelecaniformes };
        var tPhalacrocorax = new Taxon() { TaxonId = 19, dwc_TaxonID = "MSTSNM:Arter:c5bdf9f3-f785-ea11-aa77-501ac539d1ea", VernacularName = null, Parent = tPhalacrocoracidae };
        var tMorus = new Taxon() { TaxonId = 20, dwc_TaxonID = "MSTSNM:Arter:cb50a8dd-d411-4bc3-869b-ac2800d7e676", VernacularName = null, Parent = tSulidae };
        var tPlatalea = new Taxon() { TaxonId = 23, dwc_TaxonID = "MSTSNM:Arter:33bff9f3-f785-ea11-aa77-501ac539d1ea", VernacularName = null, Parent = tThreskiornithidae };
        var tPhalacrocoraxCarbo = new Taxon() { TaxonId = 39, dwc_TaxonID = "MSTSNM:Arter:c5f52bf9-f785-ea11-aa77-501ac539d1ea", VernacularName = "Skarv", Parent = tPhalacrocorax };
        var tMorusBassanus = new Taxon() { TaxonId = 40, dwc_TaxonID = "MSTSNM:Arter:dc60ddf8-f785-ea11-aa77-501ac539d1ea", VernacularName = "Sule", Parent = tMorus };
        var tPlataleaLeucorodia = new Taxon() { TaxonId = 43, dwc_TaxonID = "MSTSNM:Arter:f3fa2bf9-f785-ea11-aa77-501ac539d1ea", VernacularName = "Skestork", Parent = tPlatalea };
        var tPhalacrocoraxCarboSinensis = new Taxon() { TaxonId = 46, dwc_TaxonID = "MSTSNM:Arter:c4cdcafa-f785-ea11-aa77-501ac539d1ea", VernacularName = "Mellemskarv", Parent = tPhalacrocoraxCarbo };

        // Arrange: Author copied from DbInitializer
        var a1 = new Author() { UserName = "Roger Histand", Email = "Roger+Histand@hotmail.com", Posts = new List<Post>() };

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
    public void FilterBy_tArdea_Returns_Children() // A test to check that filtering by a taxon returns all of it's children, while filtering out all others.
    {
        // Arrange: Taxons copied from DbInitializer
        var tPelecaniformes = new Taxon() { TaxonId = 1, dwc_TaxonID = "MSTSNM:Arter:3e4e67e4-f785-ea11-aa77-501ac539d1ea", VernacularName = "Årefodede", Parent = null };
        var tArdeidae = new Taxon() { TaxonId = 2, dwc_TaxonID = "MSTSNM:Arter:495067e4-f785-ea11-aa77-501ac539d1ea", VernacularName = "Hejrer", Parent = tPelecaniformes };
        var tPhalacrocoracidae = new Taxon() { TaxonId = 5, dwc_TaxonID = "MSTSNM:Arter:2a5767e4-f785-ea11-aa77-501ac539d1ea", VernacularName = "Skarver", Parent = tPelecaniformes };
        var tSulidae = new Taxon() { TaxonId = 6, dwc_TaxonID = "MSTSNM:Arter:5c5967e4-f785-ea11-aa77-501ac539d1ea", VernacularName = "Suler", Parent = tPelecaniformes };
        var tThreskiornithidae = new Taxon() { TaxonId = 7, dwc_TaxonID = "MSTSNM:Arter:af5967e4-f785-ea11-aa77-501ac539d1ea", VernacularName = "Ibiser & skestorke", Parent = tPelecaniformes };
        var tArdea = new Taxon() { TaxonId = 8, dwc_TaxonID = "MSTSNM:Arter:7f9ef9f3-f785-ea11-aa77-501ac539d1ea", VernacularName = null, Parent = tArdeidae };
        var tPhalacrocorax = new Taxon() { TaxonId = 19, dwc_TaxonID = "MSTSNM:Arter:c5bdf9f3-f785-ea11-aa77-501ac539d1ea", VernacularName = null, Parent = tPhalacrocoracidae };
        var tMorus = new Taxon() { TaxonId = 20, dwc_TaxonID = "MSTSNM:Arter:cb50a8dd-d411-4bc3-869b-ac2800d7e676", VernacularName = null, Parent = tSulidae };
        var tPlatalea = new Taxon() { TaxonId = 23, dwc_TaxonID = "MSTSNM:Arter:33bff9f3-f785-ea11-aa77-501ac539d1ea", VernacularName = null, Parent = tThreskiornithidae };
        var tArdeaAlba = new Taxon() { TaxonId = 25, dwc_TaxonID = "MSTSNM:Arter:c18811f4-f785-ea11-aa77-501ac539d1ea", VernacularName = "Sølvhejre", Parent = tArdea };
        var tArdeaCinerea = new Taxon() { TaxonId = 26, dwc_TaxonID = "MSTSNM:Arter:c28811f4-f785-ea11-aa77-501ac539d1ea", VernacularName = "Fiskehejre", Parent = tArdea };
        var tArdeaIbis = new Taxon() { TaxonId = 27, dwc_TaxonID = "MSTSNM:Arter:f69111f4-f785-ea11-aa77-501ac539d1ea", VernacularName = "Kohejre", Parent = tArdea };
        var tArdeaPurpurea = new Taxon() { TaxonId = 28, dwc_TaxonID = "MSTSNM:Arter:c38811f4-f785-ea11-aa77-501ac539d1ea", VernacularName = "Purpurhejre", Parent = tArdea };
        var tPhalacrocoraxCarbo = new Taxon() { TaxonId = 39, dwc_TaxonID = "MSTSNM:Arter:c5f52bf9-f785-ea11-aa77-501ac539d1ea", VernacularName = "Skarv", Parent = tPhalacrocorax };
        var tMorusBassanus = new Taxon() { TaxonId = 40, dwc_TaxonID = "MSTSNM:Arter:dc60ddf8-f785-ea11-aa77-501ac539d1ea", VernacularName = "Sule", Parent = tMorus };
        var tPlataleaLeucorodia = new Taxon() { TaxonId = 43, dwc_TaxonID = "MSTSNM:Arter:f3fa2bf9-f785-ea11-aa77-501ac539d1ea", VernacularName = "Skestork", Parent = tPlatalea };
        var tPhalacrocoraxCarboSinensis = new Taxon() { TaxonId = 46, dwc_TaxonID = "MSTSNM:Arter:c4cdcafa-f785-ea11-aa77-501ac539d1ea", VernacularName = "Mellemskarv", Parent = tPhalacrocoraxCarbo };

        // Arrange: Author copied from DbInitializer
        var a1 = new Author() { UserName = "Roger Histand", Email = "Roger+Histand@hotmail.com", Posts = new List<Post>() };

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
    public void FilterBy_Non_Parent_Returns_None() // A test to check that filtering observations by a taxon that is not a parent of them returns an empty sequence.
    {
        // Arrange: Taxons copied from DbInitializer
        var tPelecaniformes = new Taxon() { TaxonId = 1, dwc_TaxonID = "MSTSNM:Arter:3e4e67e4-f785-ea11-aa77-501ac539d1ea", VernacularName = "Årefodede", Parent = null };
        var tArdeidae = new Taxon() { TaxonId = 2, dwc_TaxonID = "MSTSNM:Arter:495067e4-f785-ea11-aa77-501ac539d1ea", VernacularName = "Hejrer", Parent = tPelecaniformes };
        var tPhalacrocoracidae = new Taxon() { TaxonId = 5, dwc_TaxonID = "MSTSNM:Arter:2a5767e4-f785-ea11-aa77-501ac539d1ea", VernacularName = "Skarver", Parent = tPelecaniformes };
        var tSulidae = new Taxon() { TaxonId = 6, dwc_TaxonID = "MSTSNM:Arter:5c5967e4-f785-ea11-aa77-501ac539d1ea", VernacularName = "Suler", Parent = tPelecaniformes };
        var tThreskiornithidae = new Taxon() { TaxonId = 7, dwc_TaxonID = "MSTSNM:Arter:af5967e4-f785-ea11-aa77-501ac539d1ea", VernacularName = "Ibiser & skestorke", Parent = tPelecaniformes };
        var tArdea = new Taxon() { TaxonId = 8, dwc_TaxonID = "MSTSNM:Arter:7f9ef9f3-f785-ea11-aa77-501ac539d1ea", VernacularName = null, Parent = tArdeidae };
        var tBubulcus = new Taxon() { TaxonId = 11, dwc_TaxonID = "MSTSNM:Arter:74a1f9f3-f785-ea11-aa77-501ac539d1ea", VernacularName = null, Parent = tArdeidae };
        var tPhalacrocorax = new Taxon() { TaxonId = 19, dwc_TaxonID = "MSTSNM:Arter:c5bdf9f3-f785-ea11-aa77-501ac539d1ea", VernacularName = null, Parent = tPhalacrocoracidae };
        var tMorus = new Taxon() { TaxonId = 20, dwc_TaxonID = "MSTSNM:Arter:cb50a8dd-d411-4bc3-869b-ac2800d7e676", VernacularName = null, Parent = tSulidae };
        var tPlatalea = new Taxon() { TaxonId = 23, dwc_TaxonID = "MSTSNM:Arter:33bff9f3-f785-ea11-aa77-501ac539d1ea", VernacularName = null, Parent = tThreskiornithidae };
        var tArdeaAlba = new Taxon() { TaxonId = 25, dwc_TaxonID = "MSTSNM:Arter:c18811f4-f785-ea11-aa77-501ac539d1ea", VernacularName = "Sølvhejre", Parent = tArdea };
        var tArdeaCinerea = new Taxon() { TaxonId = 26, dwc_TaxonID = "MSTSNM:Arter:c28811f4-f785-ea11-aa77-501ac539d1ea", VernacularName = "Fiskehejre", Parent = tArdea };
        var tArdeaIbis = new Taxon() { TaxonId = 27, dwc_TaxonID = "MSTSNM:Arter:f69111f4-f785-ea11-aa77-501ac539d1ea", VernacularName = "Kohejre", Parent = tArdea };
        var tArdeaPurpurea = new Taxon() { TaxonId = 28, dwc_TaxonID = "MSTSNM:Arter:c38811f4-f785-ea11-aa77-501ac539d1ea", VernacularName = "Purpurhejre", Parent = tArdea };
        var tPhalacrocoraxCarbo = new Taxon() { TaxonId = 39, dwc_TaxonID = "MSTSNM:Arter:c5f52bf9-f785-ea11-aa77-501ac539d1ea", VernacularName = "Skarv", Parent = tPhalacrocorax };
        var tMorusBassanus = new Taxon() { TaxonId = 40, dwc_TaxonID = "MSTSNM:Arter:dc60ddf8-f785-ea11-aa77-501ac539d1ea", VernacularName = "Sule", Parent = tMorus };
        var tPlataleaLeucorodia = new Taxon() { TaxonId = 43, dwc_TaxonID = "MSTSNM:Arter:f3fa2bf9-f785-ea11-aa77-501ac539d1ea", VernacularName = "Skestork", Parent = tPlatalea };
        var tPhalacrocoraxCarboSinensis = new Taxon() { TaxonId = 46, dwc_TaxonID = "MSTSNM:Arter:c4cdcafa-f785-ea11-aa77-501ac539d1ea", VernacularName = "Mellemskarv", Parent = tPhalacrocoraxCarbo };

        // Arrange: Author copied from DbInitializer
        var a1 = new Author() { UserName = "Roger Histand", Email = "Roger+Histand@hotmail.com", Posts = new List<Post>() };

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
    public void FilterBy_Child_NotReturns_Parent() // A test to check that filtering by a taxon's child does not return observations with their child as a taxon.
    {
        // Arrange: Taxons copied from DbInitializer
        var tPelecaniformes = new Taxon() { TaxonId = 1, dwc_TaxonID = "MSTSNM:Arter:3e4e67e4-f785-ea11-aa77-501ac539d1ea", VernacularName = "Årefodede", Parent = null };
        var tPhalacrocoracidae = new Taxon() { TaxonId = 5, dwc_TaxonID = "MSTSNM:Arter:2a5767e4-f785-ea11-aa77-501ac539d1ea", VernacularName = "Skarver", Parent = tPelecaniformes };
        var tPhalacrocorax = new Taxon() { TaxonId = 19, dwc_TaxonID = "MSTSNM:Arter:c5bdf9f3-f785-ea11-aa77-501ac539d1ea", VernacularName = null, Parent = tPhalacrocoracidae };
        var tPhalacrocoraxCarbo = new Taxon() { TaxonId = 39, dwc_TaxonID = "MSTSNM:Arter:c5f52bf9-f785-ea11-aa77-501ac539d1ea", VernacularName = "Skarv", Parent = tPhalacrocorax };
        var tPhalacrocoraxCarboSinensis = new Taxon() { TaxonId = 46, dwc_TaxonID = "MSTSNM:Arter:c4cdcafa-f785-ea11-aa77-501ac539d1ea", VernacularName = "Mellemskarv", Parent = tPhalacrocoraxCarbo };

        // Arrange: Author copied from DbInitializer
        var a1 = new Author() { UserName = "Roger Histand", Email = "Roger+Histand@hotmail.com", Posts = new List<Post>() };

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
    public void FilterBy_Grandparent_Returns_Grandchild() // A test to check that filtering by a grandparent taxon returns observations that have the root taxon's grandchildren as taxa
    {
        // Arrange: Taxons copied from DbInitializer
        var tPelecaniformes = new Taxon() { TaxonId = 1, dwc_TaxonID = "MSTSNM:Arter:3e4e67e4-f785-ea11-aa77-501ac539d1ea", VernacularName = "Årefodede", Parent = null };
        var tPhalacrocoracidae = new Taxon() { TaxonId = 5, dwc_TaxonID = "MSTSNM:Arter:2a5767e4-f785-ea11-aa77-501ac539d1ea", VernacularName = "Skarver", Parent = tPelecaniformes };
        var tThreskiornithidae = new Taxon() { TaxonId = 7, dwc_TaxonID = "MSTSNM:Arter:af5967e4-f785-ea11-aa77-501ac539d1ea", VernacularName = "Ibiser & skestorke", Parent = tPelecaniformes };
        var tPhalacrocorax = new Taxon() { TaxonId = 19, dwc_TaxonID = "MSTSNM:Arter:c5bdf9f3-f785-ea11-aa77-501ac539d1ea", VernacularName = null, Parent = tPhalacrocoracidae };
        var tPlegadis = new Taxon() { TaxonId = 24, dwc_TaxonID = "MSTSNM:Arter:7dbff9f3-f785-ea11-aa77-501ac539d1ea", VernacularName = null, Parent = tThreskiornithidae };
        var tPhalacrocoraxCarbo = new Taxon() { TaxonId = 39, dwc_TaxonID = "MSTSNM:Arter:c5f52bf9-f785-ea11-aa77-501ac539d1ea", VernacularName = "Skarv", Parent = tPhalacrocorax };
        var tPlegadisFalcinellus = new Taxon() { TaxonId = 44, dwc_TaxonID = "MSTSNM:Arter:3efc2bf9-f785-ea11-aa77-501ac539d1ea", VernacularName = "Sort ibis", Parent = tPlegadis };
        var tPhalacrocoraxCarboSinensis = new Taxon() { TaxonId = 46, dwc_TaxonID = "MSTSNM:Arter:c4cdcafa-f785-ea11-aa77-501ac539d1ea", VernacularName = "Mellemskarv", Parent = tPhalacrocoraxCarbo };

        // Arrange: Author copied from DbInitializer
        var a1 = new Author() { UserName = "Roger Histand", Email = "Roger+Histand@hotmail.com", Posts = new List<Post>() };

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

    [Fact]
    public void FilterBy_Root_Returns_Itself() // A test to check that filtering observations with the same root as their taxon does not filter them out.
    {
        // Arrange: Taxons copied from DbInitializer
        var tPelecaniformes = new Taxon() { TaxonId = 1, dwc_TaxonID = "MSTSNM:Arter:3e4e67e4-f785-ea11-aa77-501ac539d1ea", VernacularName = "Årefodede", Parent = null };
        var tPhalacrocoracidae = new Taxon() { TaxonId = 5, dwc_TaxonID = "MSTSNM:Arter:2a5767e4-f785-ea11-aa77-501ac539d1ea", VernacularName = "Skarver", Parent = tPelecaniformes };
        var tPhalacrocorax = new Taxon() { TaxonId = 19, dwc_TaxonID = "MSTSNM:Arter:c5bdf9f3-f785-ea11-aa77-501ac539d1ea", VernacularName = null, Parent = tPhalacrocoracidae };
        var tPhalacrocoraxCarbo = new Taxon() { TaxonId = 39, dwc_TaxonID = "MSTSNM:Arter:c5f52bf9-f785-ea11-aa77-501ac539d1ea", VernacularName = "Skarv", Parent = tPhalacrocorax };
        var tPhalacrocoraxCarboCarbo = new Taxon() { TaxonId = 45, dwc_TaxonID = "MSTSNM:Arter:c3cdcafa-f785-ea11-aa77-501ac539d1ea", VernacularName = "Storskarv", Parent = tPhalacrocoraxCarbo };
        var tPhalacrocoraxCarboSinensis = new Taxon() { TaxonId = 46, dwc_TaxonID = "MSTSNM:Arter:c4cdcafa-f785-ea11-aa77-501ac539d1ea", VernacularName = "Mellemskarv", Parent = tPhalacrocoraxCarbo };

        // Arrange: Author copied from DbInitializer
        var a1 = new Author() { UserName = "Roger Histand", Email = "Roger+Histand@hotmail.com", Posts = new List<Post>() };

        // Arrange: Create observations
        var p1 = new Observation { Author = a1, Text = "", Taxon = tPhalacrocoraxCarboSinensis };
        var p2 = new Observation { Author = a1, Text = "", Taxon = tPhalacrocoraxCarboCarbo };

        // Arrange: Create observation sequence
        ISequence<Observation> dafnySequence = Sequence<Observation>.FromElements(p1, p2);

        // Act: Filter
        ISequence<Observation> filteredSequence = TaxonFilterWrapper.__default.FilterByWrapped(tPhalacrocoraxCarboSinensis, dafnySequence);

        // Assert: Not equal to original sequence
        Assert.NotEqual(dafnySequence, filteredSequence);

        // Assert: Sequence size equal to expected size
        int expectedSize = 1;
        Assert.Equal(expectedSize, filteredSequence.Count);

        // Assert: Observation sequence contains expected element
        Assert.Equal(p1, filteredSequence.ElementAt(0));
    }

    [Fact]
    public void FilterBy_Empty_Taxon_Filters_Out() // A test to check that filtering observations with no given taxa filters them out.
    {
        // Arrange: Taxons copied from DbInitializer
        var tPelecaniformes = new Taxon() { TaxonId = 1, dwc_TaxonID = "MSTSNM:Arter:3e4e67e4-f785-ea11-aa77-501ac539d1ea", VernacularName = "Årefodede", Parent = null };
        var tPhalacrocoracidae = new Taxon() { TaxonId = 5, dwc_TaxonID = "MSTSNM:Arter:2a5767e4-f785-ea11-aa77-501ac539d1ea", VernacularName = "Skarver", Parent = tPelecaniformes };
        var tPhalacrocorax = new Taxon() { TaxonId = 19, dwc_TaxonID = "MSTSNM:Arter:c5bdf9f3-f785-ea11-aa77-501ac539d1ea", VernacularName = null, Parent = tPhalacrocoracidae };
        var tPhalacrocoraxCarbo = new Taxon() { TaxonId = 39, dwc_TaxonID = "MSTSNM:Arter:c5f52bf9-f785-ea11-aa77-501ac539d1ea", VernacularName = "Skarv", Parent = tPhalacrocorax };
        var tPhalacrocoraxCarboCarbo = new Taxon() { TaxonId = 45, dwc_TaxonID = "MSTSNM:Arter:c3cdcafa-f785-ea11-aa77-501ac539d1ea", VernacularName = "Storskarv", Parent = tPhalacrocoraxCarbo };

        // Arrange: Author copied from DbInitializer
        var a1 = new Author() { UserName = "Roger Histand", Email = "Roger+Histand@hotmail.com", Posts = new List<Post>() };

        // Arrange: Create observations
        var p1 = new Observation { Author = a1, Text = "" };
        var p2 = new Observation { Author = a1, Text = "", Taxon = tPhalacrocoraxCarboCarbo };

        // Arrange: Create observation sequence
        ISequence<Observation> dafnySequence = Sequence<Observation>.FromElements(p1, p2);

        // Act: Filter
        ISequence<Observation> filteredSequence = TaxonFilterWrapper.__default.FilterByWrapped(tPhalacrocoraxCarboCarbo, dafnySequence);

        // Assert: Not equal to original sequence
        Assert.NotEqual(dafnySequence, filteredSequence);

        // Assert: Sequence size equal to expected size
        int expectedSize = 1;
        Assert.Equal(expectedSize, filteredSequence.Count);

        // Assert: Observation sequence contains expected element
        Assert.Equal(p2, filteredSequence.ElementAt(0));
    }
}