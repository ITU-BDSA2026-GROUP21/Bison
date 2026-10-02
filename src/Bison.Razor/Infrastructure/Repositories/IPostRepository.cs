public interface IPostRepository
{
    public Task CreatePost(PostDTO newPost);
    public List<Task> ReadPostsFromUser(string username);
    public Task UpdatePost(PostDTO alteredPost);
}