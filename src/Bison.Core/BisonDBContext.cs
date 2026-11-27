using Microsoft.EntityFrameworkCore;
using Bison.Core.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Bison.Core;

public class BisonDBContext : IdentityDbContext<Author>
{
    public DbSet<Post> Posts { get; set; }
    public DbSet<Observation> Observations { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<Proposal> Proposals { get; set; }
    public DbSet<Author> Authors { get; set; }
    public DbSet<Taxon> Taxons { get; set; }
    public BisonDBContext(DbContextOptions<BisonDBContext> options) : base(options)
    {

    }

    // This method makes sure that the UpVoters relationship is many to many, to properly define the relationship.
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Observation>()
            .HasMany(o => o.UpVoters)
            .WithMany()
            .UsingEntity("ObservationUpvotes");

        builder.Entity<Proposal>()
            .HasMany(p => p.UpVoters)
            .WithMany()
            .UsingEntity("ProposalUpvotes");
    }
}