using Microsoft.EntityFrameworkCore;
using Sonata.Models;
namespace Sonata.Data;
public class MusicContext : DbContext
{
    public DbSet<Music> Musics { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=musics.db");
    }
}
