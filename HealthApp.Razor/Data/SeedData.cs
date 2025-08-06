using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;

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

            // Create roles if they don't exist
            string[] roles = new[] { HealthAppRoles.Admin, HealthAppRoles.Doctor, HealthAppRoles.Patient };
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            // Admin user
            string adminEmail = "admin@clinicmail.org";
            if (await userManager.FindByEmailAsync(adminEmail) == null)
            {
                var adminUser = new IdentityUser { UserName = adminEmail, Email = adminEmail, EmailConfirmed = true };
                await userManager.CreateAsync(adminUser, genericPassword);
                await userManager.AddToRoleAsync(adminUser, HealthAppRoles.Admin);
            }

            // Create Doctor/Patient pairs
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
                await userManager.AddToRoleAsync(doctorUser, HealthAppRoles.Doctor);
            }

            var patientUser = await userManager.FindByEmailAsync(patientEmail);
            if (patientUser == null)
            {
                patientUser = new IdentityUser { UserName = patientEmail, Email = patientEmail, EmailConfirmed = true };
                await userManager.CreateAsync(patientUser, password);
                await userManager.AddToRoleAsync(patientUser, HealthAppRoles.Patient);
            }

            // Link doctor-patient if not already linked
            bool alreadyLinked = await context.DoctorPatient.AnyAsync(dp => dp.DoctorId == doctorUser.Id && dp.PatientId == patientUser.Id);
            if (!alreadyLinked)
            {
                context.DoctorPatient.Add(new DoctorPatient { DoctorId = doctorUser.Id, PatientId = patientUser.Id });
                await context.SaveChangesAsync();
            }

            // Add a test appointment
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