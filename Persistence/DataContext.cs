using Microsoft.EntityFrameworkCore;
using Domain;

namespace Persistence
{
    // Our database session.
    public class DataContext : DbContext
    {
        // Register our posts with the db.
        public DbSet<Post> Posts { get; set; }

        // base() is same as calling super() in other languages.
        public DataContext(DbContextOptions options) : base(options)
        {

        }
        
    }
}