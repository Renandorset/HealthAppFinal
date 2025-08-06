using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore; // ✅ ESSA LINHA É ESSENCIAL
using HealthApp.Razor.Data;
using System.Security.Claims;

namespace HealthApp.Razor.Pages.Appointments
{
    [Authorize(Roles = "Patient,Admin")]
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Appointment> Appointment { get; set; } = new List<Appointment>();

        public async Task<IActionResult> OnGetAsync()
        {
            if (!User.Identity?.IsAuthenticated ?? false)
            {
                return Challenge();
            }

            var roles = User.Claims
                .Where(c => c.Type == ClaimTypes.Role)
                .Select(c => c.Value)
                .ToList();

            if (roles.Contains("Admin"))
            {
                Appointment = await _context.Appointments.ToListAsync();
                return Page();
            }
            else if (roles.Contains("Patient"))
            {
                var userEmail = User.Identity?.Name ?? "";
                Appointment = await _context.Appointments
                    .Where(a => a.PatientEmail == userEmail)
                    .ToListAsync();
                return Page();
            }

            return Forbid();
        }
    }
}
