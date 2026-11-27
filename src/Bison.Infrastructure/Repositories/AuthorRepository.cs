using Microsoft.EntityFrameworkCore;
using Bison.Core.Models;
using Bison.Core;

public class AuthorRepository : IAuthorRepository
{
    private readonly BisonDBContext _dbContext;

    public AuthorRepository(BisonDBContext dBContext)
    {
        _dbContext = dBContext;
    }
    public async Task<Author> GetAuthorByUsernameAsync(string username)
    {
        var query = _dbContext.Authors.Where(a => a.UserName == username);

        var result = await query.FirstOrDefaultAsync();

        return result;
    }
}