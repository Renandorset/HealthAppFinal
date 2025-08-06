using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
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
            // 🔒 Verifica se o usuário está autenticado
            if (!User.Identity?.IsAuthenticated ?? false)
            {
                Console.WriteLine("❌ Usuário não autenticado.");
                return Challenge(); // força redirecionamento para login
            }

            // 🔍 Verifica as roles do usuário logado (debug)
            var roles = User.Claims
                .Where(c => c.Type == ClaimTypes.Role)
                .Select(c => c.Value)
                .ToList();

            Console.WriteLine("🔎 Roles do usuário:");
            foreach (var role in roles)
            {
                Console.WriteLine($" - {role}");
            }

            // ✅ Carrega os dados se o usuário é Patient
            if (roles.Contains("Patient"))
            {
                Appointment = await _context.Appointments.ToListAsync();
                return Page();
            }

            Console.WriteLine("❌ Acesso negado: usuário não tem a role 'Patient'.");
            return Forbid(); // 403 Forbidden
        }
    }
}
