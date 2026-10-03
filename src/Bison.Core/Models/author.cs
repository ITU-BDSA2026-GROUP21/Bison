public class Author
{
    public required string Name { get; set; }
    public required string Email { get; set; }
    public ICollection<Post> Posts { get; set; } = new List<Post>();
    public int AuthorId { get; set; }
}