using Microsoft.EntityFrameworkCore;
using Domain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Persistence
{
    // Our database session.
    // Derive from identity db context which adds the user tables.
    public class DataContext : IdentityDbContext<AppUser>
    {
        // Register our posts with the db.
        public DbSet<Post> Posts { get; set; }

        // base() is same as calling super() in other languages.
        public DataContext(DbContextOptions options) : base(options)
        {

        }
    }
}