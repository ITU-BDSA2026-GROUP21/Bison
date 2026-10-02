using Microsoft.EntityFrameworkCore;
using System.Numerics;

public class PostRepository : IPostRepository
{
    private readonly BisonDBContext _dbContext;
    public PostRepository(BisonDBContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task CreatePost(PostDTO post)
    {
        Post newPost = new() { Author = _dbContext.Authors.Where(), Text = post.Text, };
        var queryResult = await _dbContext.Posts.AddAsync(newPost);
    }
    public async Task<List<PostDTO>> ReadPostsFromUser(string username)
    {
        var query = _dbContext.Posts.Where(post => post.Author.Name == username)
                                    .Select(post => new PostDTO
                                    {
                                        Username = post.Author.Name,
                                        Text = post.Text,
                                        Timestamp = post.TimeStamp.ToString("dd/MM/yy H:mm:ss")
                                    });

        var result = await query.ToListAsync();

        return result;
    }
    public Task UpdatePost(PostDTO alteredPost)
    {

    }
}