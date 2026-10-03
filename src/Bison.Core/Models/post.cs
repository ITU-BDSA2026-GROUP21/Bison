public abstract class Post
{
    public required string Text { get; set; }
    public DateTime TimeStamp { get; set; }
    public required Author Author { get; set; }
    public int PostId { get; set; }
}