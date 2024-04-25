using Microsoft.EntityFrameworkCore;
using RehabApp.Backend.Models;

namespace RehabApp.Backend.Models;

public class InjuriesContext : DbContext
{
    public DbSet<Injury> Injuries { get; set; }

    public string DbPath { get; }

    public InjuriesContext(DbContextOptions<InjuriesContext> options)
    : base(options)
    {
        var folder = Environment.SpecialFolder.LocalApplicationData;
        var path = Environment.GetFolderPath(folder);
        DbPath = Path.Join(path, "injuries.db");
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite($"Data Source={DbPath}");
}
