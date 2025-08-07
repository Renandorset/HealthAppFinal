using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using HealthApp.Razor.Data;
using HealthApp.Domain;

namespace HealthApp.Razor.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<MedicalPrescription> MedicalPrescriptions { get; set; }
        public DbSet<Feedback> Feedback { get; set; }
        public DbSet<DoctorPatient> DoctorPatient { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Many-to-many relationship configuration
            builder.Entity<DoctorPatient>()
                .HasKey(dp => new { dp.DoctorId, dp.PatientId }); // Composite primary key

            builder.Entity<DoctorPatient>()
                .HasOne(dp => dp.Doctor)
                .WithMany()
                .HasForeignKey(dp => dp.DoctorId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent cascading deletes

            builder.Entity<DoctorPatient>()
                .HasOne(dp => dp.Patient)
                .WithMany()
                .HasForeignKey(dp => dp.PatientId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent cascading deletes
        }
    }
}
