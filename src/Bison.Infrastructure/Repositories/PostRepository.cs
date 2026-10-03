using Microsoft.EntityFrameworkCore;

public class PostRepository : IPostRepository
{
    private readonly BisonDBContext _dbContext;
    public PostRepository(BisonDBContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<int> CreatePost(Post newPost)
    {
        var authorExists = await _dbContext.Authors.AnyAsync(author => author.AuthorId == newPost.Author.AuthorId);

        if (!authorExists)
        {
            throw new ArgumentException("The given Post's AuthorId is not associated with an existing AuthorId");
        }
        var queryResult = await _dbContext.Posts.AddAsync(newPost);

        await _dbContext.SaveChangesAsync();
        return queryResult.Entity.PostId;
    }

    // The method below could return posts from multiple users if they share username, 
    // but searching by Id's is not really relevant at this time, so this should be fine for now
    public async Task<List<Post>> ReadPostsFromUser(string username)
    {
        var query = _dbContext.Posts.Where(post => post.Author.Name == username);

        var result = await query.ToListAsync();

        return result;
    }
    public async Task UpdatePost(Post alteredPost)
    {
        await _dbContext.Posts.Where(post => post.PostId == alteredPost.PostId)
                              .ExecuteUpdateAsync(post => post.SetProperty(p => p.Text, alteredPost.Text));
    }
}