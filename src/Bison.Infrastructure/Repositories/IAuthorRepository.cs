using Bison.Core.Models;

public interface IAuthorRepository
{
    public Task<Author> GetAuthorByUsernameAsync(string username);
}