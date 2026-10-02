public class Author
{
    public string Username { get; set; }
    public string Email { get; set; }

    public ICollection<Post> Posts { get; set; }

    public int Id { get; set; }
}