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
        public DbSet<PostUser> PostUsers { get; set; }

        // base() is same as calling super() in other languages.
        public DataContext(DbContextOptions options) : base(options)
        {

        }

        // Override and configure how certain models are created.
        // Especially useful for many-to-many relationships and join tables.
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // specify the PK for the join table. `HasKey` is specific to ef that tells the entity that it has "this key"
            // Which of course here we have set as a combination of the two entity PKs using new array syntax.
            builder.Entity<PostUser>(x => x.HasKey(pu => new { pu.AppUserId, pu.PostId }));
            builder.Entity<PostUser>()
                // Post user has one app user.
                .HasOne(pu => pu.AppUser)
                // Post user has many post users.
                .WithMany(au => au.PostUsers)
                // The foreign key for the app user is AppUserId.
                .HasForeignKey(pu => pu.AppUserId);

            builder.Entity<PostUser>()
                .HasOne(pu => pu.Post)
                .WithMany(au => au.PostUsers)
                .HasForeignKey(pu => pu.PostId);
        }
    }
}