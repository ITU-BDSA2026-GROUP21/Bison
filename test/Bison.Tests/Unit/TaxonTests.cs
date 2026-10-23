using Bison.Core.Models;

// These tests are for directly testing the Taxon method IsSubTaxon. 
// Testing this may be redundant, as we already test the filter that's based on this code, 
// but since our verified dafny code relies on it, it seemed prudent to test it.
public class TaxonTests
{
    [Fact]
    public void IsSubTaxon_Child_Returns_True()
    {
        // Arrange: Taxons copied from DbInitializer
        var tPelecaniformes = new Taxon() { TaxonId = 1, dwc_TaxonID = "MSTSNM:Arter:3e4e67e4-f785-ea11-aa77-501ac539d1ea", VernacularName = "Årefodede", Parent = null };
        var tPhalacrocoracidae = new Taxon() { TaxonId = 5, dwc_TaxonID = "MSTSNM:Arter:2a5767e4-f785-ea11-aa77-501ac539d1ea", VernacularName = "Skarver", Parent = tPelecaniformes };
        var tPhalacrocorax = new Taxon() { TaxonId = 19, dwc_TaxonID = "MSTSNM:Arter:c5bdf9f3-f785-ea11-aa77-501ac539d1ea", VernacularName = null, Parent = tPhalacrocoracidae };
        var tPhalacrocoraxCarbo = new Taxon() { TaxonId = 39, dwc_TaxonID = "MSTSNM:Arter:c5f52bf9-f785-ea11-aa77-501ac539d1ea", VernacularName = "Skarv", Parent = tPhalacrocorax };
        var tPhalacrocoraxCarboCarbo = new Taxon() { TaxonId = 45, dwc_TaxonID = "MSTSNM:Arter:c3cdcafa-f785-ea11-aa77-501ac539d1ea", VernacularName = "Storskarv", Parent = tPhalacrocoraxCarbo };

        // Act and assert: Check if child correctly returns true
        Assert.True(tPhalacrocoraxCarboCarbo.IsSubTaxon(tPhalacrocoraxCarbo));
    }

    [Fact]
    public void IsSubTaxon_Child_Reverse_Returns_False()
    {
        // Arrange: Taxons copied from DbInitializer
        var tPelecaniformes = new Taxon() { TaxonId = 1, dwc_TaxonID = "MSTSNM:Arter:3e4e67e4-f785-ea11-aa77-501ac539d1ea", VernacularName = "Årefodede", Parent = null };
        var tPhalacrocoracidae = new Taxon() { TaxonId = 5, dwc_TaxonID = "MSTSNM:Arter:2a5767e4-f785-ea11-aa77-501ac539d1ea", VernacularName = "Skarver", Parent = tPelecaniformes };
        var tPhalacrocorax = new Taxon() { TaxonId = 19, dwc_TaxonID = "MSTSNM:Arter:c5bdf9f3-f785-ea11-aa77-501ac539d1ea", VernacularName = null, Parent = tPhalacrocoracidae };
        var tPhalacrocoraxCarbo = new Taxon() { TaxonId = 39, dwc_TaxonID = "MSTSNM:Arter:c5f52bf9-f785-ea11-aa77-501ac539d1ea", VernacularName = "Skarv", Parent = tPhalacrocorax };
        var tPhalacrocoraxCarboCarbo = new Taxon() { TaxonId = 45, dwc_TaxonID = "MSTSNM:Arter:c3cdcafa-f785-ea11-aa77-501ac539d1ea", VernacularName = "Storskarv", Parent = tPhalacrocoraxCarbo };

        // Act and assert: Check if reverse parent-child relationship correctly returns false
        Assert.False(tPhalacrocoraxCarbo.IsSubTaxon(tPhalacrocoraxCarboCarbo));
    }

    [Fact]
    public void IsSubTaxon_GrandChild_Returns_True()
    {
        // Arrange: Taxons copied from DbInitializer
        var tPelecaniformes = new Taxon() { TaxonId = 1, dwc_TaxonID = "MSTSNM:Arter:3e4e67e4-f785-ea11-aa77-501ac539d1ea", VernacularName = "Årefodede", Parent = null };
        var tPhalacrocoracidae = new Taxon() { TaxonId = 5, dwc_TaxonID = "MSTSNM:Arter:2a5767e4-f785-ea11-aa77-501ac539d1ea", VernacularName = "Skarver", Parent = tPelecaniformes };
        var tPhalacrocorax = new Taxon() { TaxonId = 19, dwc_TaxonID = "MSTSNM:Arter:c5bdf9f3-f785-ea11-aa77-501ac539d1ea", VernacularName = null, Parent = tPhalacrocoracidae };
        var tPhalacrocoraxCarbo = new Taxon() { TaxonId = 39, dwc_TaxonID = "MSTSNM:Arter:c5f52bf9-f785-ea11-aa77-501ac539d1ea", VernacularName = "Skarv", Parent = tPhalacrocorax };
        var tPhalacrocoraxCarboCarbo = new Taxon() { TaxonId = 45, dwc_TaxonID = "MSTSNM:Arter:c3cdcafa-f785-ea11-aa77-501ac539d1ea", VernacularName = "Storskarv", Parent = tPhalacrocoraxCarbo };

        // Act and assert: Check if grandchild correctly returns true
        Assert.True(tPhalacrocoraxCarboCarbo.IsSubTaxon(tPhalacrocorax));
    }

    [Fact]
    public void IsSubTaxon_Self_Returns_False()
    {
        // Arrange: Taxons copied from DbInitializer
        var tPelecaniformes = new Taxon() { TaxonId = 1, dwc_TaxonID = "MSTSNM:Arter:3e4e67e4-f785-ea11-aa77-501ac539d1ea", VernacularName = "Årefodede", Parent = null };
        var tPhalacrocoracidae = new Taxon() { TaxonId = 5, dwc_TaxonID = "MSTSNM:Arter:2a5767e4-f785-ea11-aa77-501ac539d1ea", VernacularName = "Skarver", Parent = tPelecaniformes };
        var tPhalacrocorax = new Taxon() { TaxonId = 19, dwc_TaxonID = "MSTSNM:Arter:c5bdf9f3-f785-ea11-aa77-501ac539d1ea", VernacularName = null, Parent = tPhalacrocoracidae };
        var tPhalacrocoraxCarbo = new Taxon() { TaxonId = 39, dwc_TaxonID = "MSTSNM:Arter:c5f52bf9-f785-ea11-aa77-501ac539d1ea", VernacularName = "Skarv", Parent = tPhalacrocorax };
        var tPhalacrocoraxCarboCarbo = new Taxon() { TaxonId = 45, dwc_TaxonID = "MSTSNM:Arter:c3cdcafa-f785-ea11-aa77-501ac539d1ea", VernacularName = "Storskarv", Parent = tPhalacrocoraxCarbo };

        // Act and assert: Check if self correctly returns false.
        Assert.False(tPhalacrocoraxCarboCarbo.IsSubTaxon(tPhalacrocoraxCarboCarbo));
    }

    [Fact]
    public void IsSubTaxon_Non_Ancestor_Returns_False()
    {
        // Arrange: Taxons copied from DbInitializer
        var tPelecaniformes = new Taxon() { TaxonId = 1, dwc_TaxonID = "MSTSNM:Arter:3e4e67e4-f785-ea11-aa77-501ac539d1ea", VernacularName = "Årefodede", Parent = null };
        var tPhalacrocoracidae = new Taxon() { TaxonId = 5, dwc_TaxonID = "MSTSNM:Arter:2a5767e4-f785-ea11-aa77-501ac539d1ea", VernacularName = "Skarver", Parent = tPelecaniformes };
        var tSulidae = new Taxon() { TaxonId = 6, dwc_TaxonID = "MSTSNM:Arter:5c5967e4-f785-ea11-aa77-501ac539d1ea", VernacularName = "Suler", Parent = tPelecaniformes };
        var tPhalacrocorax = new Taxon() { TaxonId = 19, dwc_TaxonID = "MSTSNM:Arter:c5bdf9f3-f785-ea11-aa77-501ac539d1ea", VernacularName = null, Parent = tPhalacrocoracidae };
        var tMorus = new Taxon() { TaxonId = 20, dwc_TaxonID = "MSTSNM:Arter:cb50a8dd-d411-4bc3-869b-ac2800d7e676", VernacularName = null, Parent = tSulidae };
        var tPhalacrocoraxCarbo = new Taxon() { TaxonId = 39, dwc_TaxonID = "MSTSNM:Arter:c5f52bf9-f785-ea11-aa77-501ac539d1ea", VernacularName = "Skarv", Parent = tPhalacrocorax };
        var tPhalacrocoraxCarboCarbo = new Taxon() { TaxonId = 45, dwc_TaxonID = "MSTSNM:Arter:c3cdcafa-f785-ea11-aa77-501ac539d1ea", VernacularName = "Storskarv", Parent = tPhalacrocoraxCarbo };

        // Act and assert: Check if non ancestor correctly returns false.
        Assert.False(tPhalacrocoraxCarboCarbo.IsSubTaxon(tMorus));
    }

    [Fact]
    public void IsSubTaxon_Empty_Parent_Returns_False()
    {
        // Arrange: Taxon, copied from DbInitializer
        var tPelecaniformes = new Taxon() { TaxonId = 1, dwc_TaxonID = "MSTSNM:Arter:3e4e67e4-f785-ea11-aa77-501ac539d1ea", VernacularName = "Årefodede", Parent = null };
        var tArdeidae = new Taxon() { TaxonId = 2, dwc_TaxonID = "MSTSNM:Arter:495067e4-f785-ea11-aa77-501ac539d1ea", VernacularName = "Hejrer", Parent = tPelecaniformes };

        // Act and assert: Check that an IsSubTaxon call does not throw on an empty parent.
        Assert.False(tPelecaniformes.IsSubTaxon(tArdeidae));
    }
}