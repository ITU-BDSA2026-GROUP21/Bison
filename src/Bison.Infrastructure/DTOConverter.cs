using System.Globalization;
using Bison.Core.Models;

// This class converts Data Model objects into DTOs for the services.
public static class DTOConverter
{
    private const string DTFormat = "MM/dd/yy H:mm:ss";
    public static ReadObservationDTO ToDTO(Observation o)
    {
        ReadObservationDTO dto = new ReadObservationDTO
        {
            Id = o.PostId,
            Username = o.Author.UserName,
            Text = o.Text,
            Timestamp = DateTimeToString(o.TimeStamp)
        };

        return dto;
    }
    public static ReadCommentDTO ToDTO(Comment c)
    {
        ReadCommentDTO dto = new ReadCommentDTO
        {
            Username = c.Author.UserName,
            Text = c.Text,
            Timestamp = DateTimeToString(c.TimeStamp)
        };

        return dto;
    }
    public static ReadProposalDTO ToDTO(Proposal p)
    {
        string name;

        if (p.Taxon.VernacularName == null || p.Taxon.VernacularName == "")
        {
            name = "No vernacular name";
        }
        else
        {
            name = p.Taxon.VernacularName;
        }

        ReadProposalDTO dto = new ReadProposalDTO
        {
            Username = p.Author.UserName,
            Text = p.Text,
            Timestamp = DateTimeToString(p.TimeStamp),
            DwcTaxonId = p.Taxon.dwc_TaxonID,
            TaxonName = name
        };

        return dto;
    }

    private static string DateTimeToString(DateTime t)
    {
        return t.ToString(DTFormat, CultureInfo.InvariantCulture);
    }
}