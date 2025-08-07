using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using HealthApp.Razor.Data;
using Microsoft.AspNetCore.Authorization;

namespace HealthApp.Razor.Pages.MedicalPrescriptions
{
    [Authorize(Roles = "Doctor,Admin")]
    // Restringe acesso só para usuários com papel "Doctor"
    public class DetailsModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DetailsModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public MedicalPrescription MedicalPrescription { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null) return NotFound();

            var prescription = await _context.MedicalPrescriptions.FindAsync(id);
            if (prescription == null) return NotFound();

            MedicalPrescription = prescription;
            return Page();
        }
    }
}
