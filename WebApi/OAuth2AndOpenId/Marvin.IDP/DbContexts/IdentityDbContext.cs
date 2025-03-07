using Marvin.IDP.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Marvin.IDP.DbContexts
{
    public class IdentityDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public DbSet<UserClaim> UserClaims { get; set; }         

        public IdentityDbContext(
          DbContextOptions<IdentityDbContext> options)
        : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.ConfigureWarnings( warningsConfigurationBuilderAction => warningsConfigurationBuilderAction.Ignore(RelationalEventId.PendingModelChangesWarning));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
            .HasIndex(u => u.Subject)
            .IsUnique();

            modelBuilder.Entity<User>()
            .HasIndex(u => u.UserName)
            .IsUnique();

            modelBuilder.Entity<User>().HasData(
                new User()
                {
                    Id = new Guid("13229d33-99e0-41b3-b18d-4f72127e3971"),
                    Password = "password",
                    Subject = "d860efca-22d9-47fd-8249-791ba61b07c7",
                    UserName = "David",
                    Email = "David@gmail.com",
                    ConcurrencyStamp = new Guid("314e0d20-208f-4e3c-a131-deae789a4fdc").ToString(),
                    Active = true
                },
                new User()
                {
                    Id = new Guid("96053525-f4a5-47ee-855e-0ea77fa6c55a"),
                    Password = "password",
                    Subject = "b7539694-97e7-4dfe-84da-b4256e1ff5c7",
                    UserName = "Emma",
                    Email = "David@gmail.com",
                    ConcurrencyStamp = new Guid("c198a000-7f99-4ef5-b4a9-8a18abab2281").ToString(),
                    Active = true
                }); ;

            modelBuilder.Entity<UserClaim>().HasData(
             new UserClaim()
             {
                 Id = new Guid("ccca788f-bdec-4d9f-9caf-b149616d6867"),
                 UserId = new Guid("13229d33-99e0-41b3-b18d-4f72127e3971"),
                 Type = "given_name",
                 Value = "David"
             },
             new UserClaim()
             {
                 Id = new Guid("41f72664-5378-426b-8871-0b8cb852bf22"),
                 UserId = new Guid("13229d33-99e0-41b3-b18d-4f72127e3971"),
                 Type = "family_name",
                 Value = "Flagg"
             }, 
             new UserClaim()
             {
                 Id = new Guid("961a83d0-1263-4b96-899b-059e168b03a5"),
                 UserId = new Guid("13229d33-99e0-41b3-b18d-4f72127e3971"),
                 Type = "country",
                 Value = "nl"
             },
             new UserClaim()
             {
                 Id = new Guid("4a337664-2555-40c6-aad6-53aab772d766"),
                 UserId = new Guid("13229d33-99e0-41b3-b18d-4f72127e3971"),
                 Type = "role",
                 Value = "FreeUser"
             },
             new UserClaim()
             {
                 Id = new Guid("508b5a3c-ad01-4376-ade9-2aae53990c0b"),
                 UserId = new Guid("96053525-f4a5-47ee-855e-0ea77fa6c55a"),
                 Type = "given_name",
                 Value = "Emma"
             },
             new UserClaim()
             {
                 Id = new Guid("a15ee06c-a2fe-4036-8a70-63eb4b7dc646"),
                 UserId = new Guid("96053525-f4a5-47ee-855e-0ea77fa6c55a"),
                 Type = "family_name",
                 Value = "Flagg"
             }, 
             new UserClaim()
             {
                 Id = new Guid("63647ad2-2557-4b0b-9065-f95bfbcd07cb"),
                 UserId = new Guid("96053525-f4a5-47ee-855e-0ea77fa6c55a"),
                 Type = "country",
                 Value = "be"
             }, 
             new UserClaim()
             {
                 Id = new Guid("772648ab-ff21-4684-a959-f7cc55633c70"),
                 UserId = new Guid("96053525-f4a5-47ee-855e-0ea77fa6c55a"),
                 Type = "role",
                 Value = "PayingUser"
             });
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            // get updated entries
            var updatedConcurrencyAwareEntries = ChangeTracker.Entries()
                    .Where(e => e.State == EntityState.Modified)
                    .OfType<IConcurrencyAware>();

            foreach (var entry in updatedConcurrencyAwareEntries)
            {
                entry.ConcurrencyStamp = Guid.NewGuid().ToString();
            }

            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
