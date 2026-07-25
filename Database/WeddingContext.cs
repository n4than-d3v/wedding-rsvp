using Microsoft.EntityFrameworkCore;
using Wedding.Database.Models;

namespace Wedding.Database;
 
public class WeddingContext(DbContextOptions<WeddingContext> options) : DbContext(options)
{
    public DbSet<Party> Parties { get; set; }
    public DbSet<Guest> Guests { get; set; }
}
