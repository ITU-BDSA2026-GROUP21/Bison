using System.Data;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

public class Observation
{
    public required int ObservationID { get; set; }
    public required int AuthorID { get; set; }
    public required string Text { get; set; }
    public required long PubDate { get; set; }
}
public class User
{
    public required int UserID { get; set; }
    public required string Username { get; set; }
    public required string Email { get; set; }
}

public class BisonDbContext : DbContext
{
    public BisonDbContext(DbContextOptions<BisonDbContext> options) : base(options) { }

    public DbSet<Observation> Observations { get; set; }
    public DbSet<User> Users { get; set; }
}

public class DBFacade
{
    private readonly BisonDbContext _dbContext;

    public DBFacade(BisonDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<List<ObservationViewModel>> GetObservations()
    {
        var query = from o in _dbContext.Observations
                    join u in _dbContext.Users
                        on o.AuthorID equals u.UserID
                    orderby o.PubDate descending
                    select new ObservationViewModel(
                        u.Username,
                        o.Text,
                        UnixTimeStampToDateTimeString(o.PubDate)
                    );
        var result = await query.ToListAsync();
        return result;
    }

    private static string UnixTimeStampToDateTimeString(double unixTimeStamp)
    {
        // Unix timestamp is seconds past epoch
        DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        dateTime = dateTime.AddSeconds(unixTimeStamp);
        return dateTime.ToString("MM/dd/yy H:mm:ss");
    }
}