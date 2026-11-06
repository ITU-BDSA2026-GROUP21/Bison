namespace Bison.Core.Models;

using Microsoft.AspNetCore.Identity;

public class Author : IdentityUser
{
    public ICollection<Post> Posts { get; set; } = new List<Post>();
}