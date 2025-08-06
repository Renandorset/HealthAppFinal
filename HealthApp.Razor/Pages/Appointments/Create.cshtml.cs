using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using HealthApp.Razor.Data;
using System.Threading.Tasks;

namespace HealthApp.Razor.Pages.Appointments
{
    [Authorize(Roles = "Patient,Admin")]


    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public CreateModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Appointment Appointment { get; set; } = new Appointment();

        public IActionResult OnGet()
        {
            if (!User.IsInRole("Patient") && !User.IsInRole("Admin"))
            {
                return Forbid();
            }


            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Paciente não pode alterar email para outro
            if (User.IsInRole("Patient"))
            {
                Appointment.PatientEmail = User.Identity?.Name ?? Appointment.PatientEmail;
            }

            _context.Appointments.Add(Appointment);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
