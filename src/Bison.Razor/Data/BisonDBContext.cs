using Microsoft.EntityFrameworkCore;

public class BisonDBContext : DbContext
{
    public DbSet<Observation> Observation { get; set; }

    public DbSet<Author> Author { get; set; }

    public DbSet<Comment> Comment { get; set; }

    public DbSet<Proposal> Proposal { get; set; }

    public DbSet<Taxon> Taxon { get; set; }

    public BisonDBContext(DbContextOptions<BisonDBContext> options) : base(options)
    {

    }
}