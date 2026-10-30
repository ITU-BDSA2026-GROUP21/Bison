using Bison.Core.Models;
using System.Globalization;

public class DTOConverterTests
{
    [Fact]
    public void Observation_ToDTO_Maps_Correctly()
    {
        // Arrange: Taxons, copied from DbInitializer
        var tPelecaniformes = new Taxon() { TaxonId = 1, dwc_TaxonID = "MSTSNM:Arter:3e4e67e4-f785-ea11-aa77-501ac539d1ea", VernacularName = "Årefodede", Parent = null };
        var tArdeidae = new Taxon() { TaxonId = 2, dwc_TaxonID = "MSTSNM:Arter:495067e4-f785-ea11-aa77-501ac539d1ea", VernacularName = "Hejrer", Parent = tPelecaniformes };
        var tArdea = new Taxon() { TaxonId = 8, dwc_TaxonID = "MSTSNM:Arter:7f9ef9f3-f785-ea11-aa77-501ac539d1ea", VernacularName = null, Parent = tArdeidae };
        var tArdeaCinerea = new Taxon() { TaxonId = 26, dwc_TaxonID = "MSTSNM:Arter:c28811f4-f785-ea11-aa77-501ac539d1ea", VernacularName = "Fiskehejre", Parent = tArdea };

        // Arrange: Author, copied from DbInitializer
        var a1 = new Author() { UserName = "Roger Histand", Email = "Roger+Histand@hotmail.com", Posts = new List<Post>() };

        // Arrange: Observation, copied from DbInitializer
        var o1 = new Observation() { PostId = 1, Author = a1, Taxon = tArdeaCinerea, Text = "Grey heron standing still in the reeds at the lake.", TimeStamp = DateTime.Parse("2023-08-01 07:12:00") };

        // Arrange: Create manual DTO to check against
        var expectedDTO = new ReadObservationDTO { Id = 1, Username = "Roger Histand", Text = "Grey heron standing still in the reeds at the lake.", Timestamp = "08/01/23 7:12:00" };

        // Act: Call ToDTO to get actual DTO to test
        var actualDTO = DTOConverter.ToDTO(o1);

        // Assert: That all fields in actual DTO matches expected fields
        Assert.Equal(expectedDTO, actualDTO);
    }

    [Fact]
    public void Comment_ToDTO_Maps_Correctly()
    {
        // Arrange: Taxons, copied from DbInitializer
        var tPelecaniformes = new Taxon() { TaxonId = 1, dwc_TaxonID = "MSTSNM:Arter:3e4e67e4-f785-ea11-aa77-501ac539d1ea", VernacularName = "Årefodede", Parent = null };
        var tArdeidae = new Taxon() { TaxonId = 2, dwc_TaxonID = "MSTSNM:Arter:495067e4-f785-ea11-aa77-501ac539d1ea", VernacularName = "Hejrer", Parent = tPelecaniformes };
        var tArdea = new Taxon() { TaxonId = 8, dwc_TaxonID = "MSTSNM:Arter:7f9ef9f3-f785-ea11-aa77-501ac539d1ea", VernacularName = null, Parent = tArdeidae };
        var tArdeaCinerea = new Taxon() { TaxonId = 26, dwc_TaxonID = "MSTSNM:Arter:c28811f4-f785-ea11-aa77-501ac539d1ea", VernacularName = "Fiskehejre", Parent = tArdea };

        // Arrange: Authors, copied from DbInitializer
        var a1 = new Author() { UserName = "Roger Histand", Email = "Roger+Histand@hotmail.com", Posts = new List<Post>() };
        var a2 = new Author() { UserName = "Luanna Muro", Email = "Luanna-Muro@ku.dk", Posts = new List<Post>() };

        // Arrange: Observation, copied from DbInitializer
        var o1 = new Observation() { PostId = 1, Author = a1, Taxon = tArdeaCinerea, Text = "Grey heron standing still in the reeds at the lake.", TimeStamp = DateTime.Parse("2023-08-01 07:12:00") };

        // Arrange: Comment, copied from DbInitializer
        var c1 = new Comment() { PostId = 11, Author = a2, Observation = o1, Text = "Nice! I saw one there yesterday too.", TimeStamp = DateTime.Parse("2023-08-01 09:00:00") };

        // Arrange: Create manual DTO to check against
        var expectedDTO = new ReadCommentDTO { Username = "Luanna Muro", Text = "Nice! I saw one there yesterday too.", Timestamp = "08/01/23 9:00:00" };

        // Act: Call ToDTO to get actual DTO to test
        var actualDTO = DTOConverter.ToDTO(c1);

        // Assert: That all fields in actual DTO matches expected fields
        Assert.Equal(expectedDTO, actualDTO);
    }

    [Fact]
    public void Proposal_ToDTO_Maps_Correctly()
    {
        // Arrange: Taxons, copied from DbInitializer
        var tPelecaniformes = new Taxon() { TaxonId = 1, dwc_TaxonID = "MSTSNM:Arter:3e4e67e4-f785-ea11-aa77-501ac539d1ea", VernacularName = "Årefodede", Parent = null };
        var tArdeidae = new Taxon() { TaxonId = 2, dwc_TaxonID = "MSTSNM:Arter:495067e4-f785-ea11-aa77-501ac539d1ea", VernacularName = "Hejrer", Parent = tPelecaniformes };
        var tEgretta = new Taxon() { TaxonId = 12, dwc_TaxonID = "MSTSNM:Arter:25aaf9f3-f785-ea11-aa77-501ac539d1ea", VernacularName = null, Parent = tArdeidae };
        var tArdea = new Taxon() { TaxonId = 8, dwc_TaxonID = "MSTSNM:Arter:7f9ef9f3-f785-ea11-aa77-501ac539d1ea", VernacularName = null, Parent = tArdeidae };
        var tArdeaAlba = new Taxon() { TaxonId = 25, dwc_TaxonID = "MSTSNM:Arter:c18811f4-f785-ea11-aa77-501ac539d1ea", VernacularName = "Sølvhejre", Parent = tArdea };
        var tEgrettaGarzetta = new Taxon() { TaxonId = 33, dwc_TaxonID = "MSTSNM:Arter:ca4d8cf8-f785-ea11-aa77-501ac539d1ea", VernacularName = "Silkehejre", Parent = tEgretta };


        // Arrange: Authors, copied from DbInitializer
        var a1 = new Author() { UserName = "Roger Histand", Email = "Roger+Histand@hotmail.com", Posts = new List<Post>() };
        var a2 = new Author() { UserName = "Luanna Muro", Email = "Luanna-Muro@ku.dk", Posts = new List<Post>() };
        var a3 = new Author() { UserName = "Wendell Ballan", Email = "Wendell-Ballan@gmail.com", Posts = new List<Post>() };

        // Arrange: Observation, copied from DbInitializer
        var o3 = new Observation() { PostId = 3, Author = a3, Taxon = tArdeaAlba, Text = "Large white heron in a wet meadow. Yellow bill, black legs.", TimeStamp = DateTime.Parse("2023-08-02 06:45:00") };

        // Arrange: Proposal, copied from DbInitializer
        var p1 = new Proposal() { PostId = 21, Author = a1, Observation = o3, Taxon = tEgrettaGarzetta, Text = "Could it be a little egret? Check for yellow feet.", TimeStamp = DateTime.Parse("2023-08-02 08:10:00") };

        // Arrange: Create manual DTO to check against
        var expectedDTO = new ReadProposalDTO { Username = "Roger Histand", Text = "Could it be a little egret? Check for yellow feet.", Timestamp = "08/02/23 8:10:00", DwcTaxonId = "MSTSNM:Arter:ca4d8cf8-f785-ea11-aa77-501ac539d1ea", TaxonName = "Silkehejre" };

        // Act: Call ToDTO to get actual DTO to test
        var actualDTO = DTOConverter.ToDTO(p1);

        // Assert: That all fields in actual DTO matches expected fields
        Assert.Equal(expectedDTO, actualDTO);
    }
}