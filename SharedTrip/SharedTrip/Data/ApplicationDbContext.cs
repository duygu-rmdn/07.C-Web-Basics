namespace SharedTrip.Data
{
    using Microsoft.EntityFrameworkCore;
    using SharedTrip.Models;

    public class ApplicationDbContext : DbContext
    {
        public DbSet<User> Users { get; init; }
        public DbSet<Trip> Trips { get; init; }
        public DbSet<UserTrip> UsersTrips { get; init; }
        public ApplicationDbContext()
        {
            
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            optionsBuilder.UseSqlServer(DatabaseConfiguration.ConnectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserTrip>()
                .HasKey(x => new { x.UserId, x.TripId });
            modelBuilder.Entity<UserTrip>()
                .HasOne(x => x.User)
                .WithMany(x => x.UserTrips)
                .HasForeignKey(x => x.UserId);
            modelBuilder.Entity<UserTrip>()
                .HasOne(x => x.Trip)
                .WithMany(x => x.UserTrips)
                .HasForeignKey(x => x.TripId);
        }
    }
}
