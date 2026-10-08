// This interface may be obsolete with the addition of IObservationRepository

public interface IPostRepository
{
    public Task<int> CreatePost(Post newPost);
    public Task<List<Post>> ReadPostsFromUser(string username);
    public Task UpdatePost(Post alteredPost);
}