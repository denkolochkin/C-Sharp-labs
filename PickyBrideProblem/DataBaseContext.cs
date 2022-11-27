using System.Xml;
using Microsoft.EntityFrameworkCore;

using PickyBrideProblem.Entity;

public class DataBaseContext : DbContext
{
    public DbSet<Contender> Contender { get; set; } = null!;

    public DbSet<Attempt> Attempt { get; set; } = null!;

    public DataBaseContext()
    {
        Database.EnsureCreated();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(
            "Host=localhost;Port=5432;Database=bride-db;Username=postgres;Password=postgres"
            );
    }
}