using Microsoft.EntityFrameworkCore;

namespace Tech_Assessment_Feature_Switches.Models
{
    public class FeatureAccessContext : DbContext
    {
        public FeatureAccessContext(DbContextOptions<FeatureAccessContext> options)
            : base(options)
        {
        }

        public DbSet<FeatureAccess> FeatureAccess { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FeatureAccess>()
                .HasKey(c => new { c.Email, c.FeatureName });
        }
    }
}