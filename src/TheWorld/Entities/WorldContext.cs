using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace TheWorld.Entities
{
    public class WorldContext : IdentityDbContext<WorldUser>
    {
        private IConfigurationRoot configuration;

        public WorldContext(IConfigurationRoot configuration, DbContextOptions options)
            : base(options)
        {
            this.configuration = configuration;
        }

        public DbSet<Trip> Trips { get; set; }
        public DbSet<Stop> Stops { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer(configuration["ConnectionStrings:WorldContextConnection"]);
        }
    }
}
