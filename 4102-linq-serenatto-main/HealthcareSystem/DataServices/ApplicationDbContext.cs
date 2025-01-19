using HealthcareSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace HealthcareSystem.DataServices
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
          : base(options)
        {
        }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Claim> Claims { get; set; }
        public DbSet<InsurancePolicy>? InsurancePolicies { get; set; }
        public DbSet<Doctors>? Doctors { get; set; }
        public DbSet<DoctorClaim>? DoctorClaims { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configuring the many-to-many relationship between Doctors and Claims
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Patient>()
                        .HasMany(p => p.Claims)
                        .WithOne(c => c.Patient)
                        .HasForeignKey(c => c.PatientID);

            modelBuilder.Entity<DoctorClaim>()
                .HasKey(dc => new { dc.DoctorID, dc.ClaimID });

            modelBuilder.Entity<DoctorClaim>()
                .HasOne(dc => dc.Doctor)
                .WithMany(d => d.DoctorClaims)
                .HasForeignKey(dc => dc.DoctorID);

            modelBuilder.Entity<DoctorClaim>()
                .HasOne(dc => dc.Claim)
                .WithMany(c => c.DoctorClaims)
                .HasForeignKey(dc => dc.ClaimID);

        }
    }
}
