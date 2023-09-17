using CityInfo.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace CityInfo.API.DbContexts
{
    public class CityInfoContext : DbContext
    {
        public DbSet<City> Cities { get; set; } = null!;
        public DbSet<PointOfInterest> PointsOfInterest { get; set; } = null!;

        public CityInfoContext(DbContextOptions<CityInfoContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<City>().HasData(
                new City("New York City")
                {
                    Id = 1,
                    Description = "The one with the big park."
                },
                new City("Antwerp")
                {
                    Id = 2,
                    Description = "The one with the unfinished cathedral."
                },
                new City("Paris")
                {
                    Id = 3,
                    Description = "The one with the big tower."
                });  
            
            modelBuilder.Entity<PointOfInterest>().HasData(
                new PointOfInterest("Central Park")
                {
                    Id = 1,
                    CityId = 1,
                    Description = "The big park."
                },
                new PointOfInterest("Cathedral")
                {
                    Id = 2,
                    CityId = 2,
                    Description = "The unfinished cathedral."
                },
                new PointOfInterest("Eiffel Tower")
                {
                    Id = 3,
                    CityId = 3,
                    Description = "The big tower."
                });

            base.OnModelCreating(modelBuilder);
        }
    }
}
