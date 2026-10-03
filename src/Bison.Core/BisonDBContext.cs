using Microsoft.EntityFrameworkCore;

public class BisonDBContext : DbContext
{
    public DbSet<Post> Posts { get; set; }

    public DbSet<Author> Authors { get; set; }

    public DbSet<Taxon> Taxons { get; set; }

    public BisonDBContext(DbContextOptions<BisonDBContext> options) : base(options)
    {

    }
}