namespace Bison.Core.Models;

public class Comment : Post
{
    public required Observation Observation { get; set; }
}