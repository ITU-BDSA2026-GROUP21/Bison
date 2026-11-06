using System.ComponentModel.DataAnnotations;

namespace Bison.Core.Models;

public abstract class Post
{
    public required string Text { get; set; }
    public DateTime TimeStamp { get; set; }
    public required Author Author { get; set; }
    [Key] //For autoincrementation when making post
    public int PostId { get; set; }
}