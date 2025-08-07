using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;
using Bogus;
using HealthApp.Razor.Data;

namespace HealthApp.Razor.Data
{
    public static class HealthAppRoles
    {
        public const string Admin = "Admin";
        public const string Doctor = "Doctor";
        public const string Patient = "Patient";
    }

    public static class SeedData
    {
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var services = scope.ServiceProvider;
            var context = services.GetRequiredService<ApplicationDbContext>();
            var userManager = services.GetRequiredService<UserManager<IdentityUser>>();
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

            string genericPassword = "ClinicPass123!";

            // 👉 1. Seed Doctors
            if (!context.Doctors.Any())
            {
                var doctorFaker = new Faker<Doctor>()
                    .RuleFor(d => d.FullName, f => f.Name.FullName())
                    .RuleFor(d => d.Specialty, f => f.PickRandom("Cardiology", "Pediatrics", "Neurology", "Dermatology", "Oncology"))
                    .RuleFor(d => d.Location, f => f.Address.City());

                var doctors = doctorFaker.Generate(10);
                context.Doctors.AddRange(doctors);
                await context.SaveChangesAsync();
            }

            var doctorList = context.Doctors.ToList();

            // 👉 2. Seed Appointments
            if (!context.Appointments.Any())
            {
                var appointmentFaker = new Faker<Appointment>()
                    .RuleFor(a => a.PatientName, f => f.Name.FullName())
                    .RuleFor(a => a.PatientEmail, f => f.Internet.Email())
                    .RuleFor(a => a.DoctorEmail, f => f.PickRandom(doctorList).FullName + "@clinicmail.org")
                    .RuleFor(a => a.Date, f => f.Date.Soon(30))
                    .RuleFor(a => a.Status, f => f.PickRandom("Pending", "Approved", "Rejected", "Completed"))
                    .RuleFor(a => a.Notes, f => f.Lorem.Sentence());

                context.Appointments.AddRange(appointmentFaker.Generate(10));
                await context.SaveChangesAsync();
            }

            // 👉 3. Seed MedicalPrescriptions
            if (!context.MedicalPrescriptions.Any())
            {
                var prescriptionFaker = new Faker<MedicalPrescription>()
                    .RuleFor(p => p.DoctorName, f => f.PickRandom(doctorList).FullName)
                    .RuleFor(p => p.PatientName, f => f.Name.FullName())
                    .RuleFor(p => p.Medication, f => f.Commerce.ProductName())
                    .RuleFor(p => p.Dosage, f => $"{f.Random.Int(1, 3)}x per day")
                    .RuleFor(p => p.Instructions, f => f.Lorem.Sentence());

                context.MedicalPrescriptions.AddRange(prescriptionFaker.Generate(10));
                await context.SaveChangesAsync();
            }

            // 👉 4. Seed Feedback
            if (!context.Feedback.Any())
            {
                var feedbackFaker = new Faker<Feedback>()
                    .RuleFor(f => f.DoctorName, f => f.PickRandom(doctorList).FullName)
                    .RuleFor(f => f.PatientName, f => f.Name.FullName())
                    .RuleFor(f => f.Rating, f => f.Random.Int(1, 5))
                    .RuleFor(f => f.Comment, f => f.Lorem.Sentence());

                context.Feedback.AddRange(feedbackFaker.Generate(10));
                await context.SaveChangesAsync();
            }

            // 👉 5. Create Roles
            string[] roles = { HealthAppRoles.Admin, HealthAppRoles.Doctor, HealthAppRoles.Patient };
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            // 👉 6. Seed Admin
            string adminEmail = "admin@clinicmail.org";
            if (await userManager.FindByEmailAsync(adminEmail) == null)
            {
                var admin = new IdentityUser { UserName = adminEmail, Email = adminEmail, EmailConfirmed = true };
                await userManager.CreateAsync(admin, genericPassword);
                await userManager.AddToRoleAsync(admin, HealthAppRoles.Admin);
            }

            // 👉 7. Seed test Doctor/Patient accounts
            await CreateDoctorAndPatientPair(userManager, context, "doctor01@clinicmail.org", "patient01@clinicmail.org", genericPassword);
            await CreateDoctorAndPatientPair(userManager, context, "doctor02@clinicmail.org", "patient02@clinicmail.org", genericPassword);
        }

        private static async Task CreateDoctorAndPatientPair(UserManager<IdentityUser> userManager, ApplicationDbContext context, string doctorEmail, string patientEmail, string password)
        {
            var doctorUser = await userManager.FindByEmailAsync(doctorEmail);
            if (doctorUser == null)
            {
                doctorUser = new IdentityUser { UserName = doctorEmail, Email = doctorEmail, EmailConfirmed = true };
                await userManager.CreateAsync(doctorUser, password);
            }

            if (!await userManager.IsInRoleAsync(doctorUser, HealthAppRoles.Doctor))
                await userManager.AddToRoleAsync(doctorUser, HealthAppRoles.Doctor);

            var patientUser = await userManager.FindByEmailAsync(patientEmail);
            if (patientUser == null)
            {
                patientUser = new IdentityUser { UserName = patientEmail, Email = patientEmail, EmailConfirmed = true };
                await userManager.CreateAsync(patientUser, password);
            }

            if (!await userManager.IsInRoleAsync(patientUser, HealthAppRoles.Patient))
                await userManager.AddToRoleAsync(patientUser, HealthAppRoles.Patient);

            // Link doctor and patient
            bool alreadyLinked = await context.DoctorPatient.AnyAsync(dp =>
                dp.DoctorId == doctorUser.Id && dp.PatientId == patientUser.Id);

            if (!alreadyLinked)
            {
                context.DoctorPatient.Add(new DoctorPatient
                {
                    DoctorId = doctorUser.Id,
                    PatientId = patientUser.Id
                });
                await context.SaveChangesAsync();
            }

            // Ensure appointment exists
            bool appointmentExists = await context.Appointments.AnyAsync(a => a.PatientEmail == patientEmail);
            if (!appointmentExists)
            {
                context.Appointments.Add(new Appointment
                {
                    PatientName = "João da Silva",
                    PatientEmail = patientEmail,
                    DoctorEmail = doctorEmail,
                    Date = DateTime.Now.AddDays(1),
                    Status = "Pending",
                    Notes = "Consulta de rotina"
                });
                await context.SaveChangesAsync();
            }
        }
    }
}
