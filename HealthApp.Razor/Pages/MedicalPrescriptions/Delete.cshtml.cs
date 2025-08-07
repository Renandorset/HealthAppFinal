using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using HealthApp.Razor.Data;
using Microsoft.AspNetCore.Authorization;

namespace HealthApp.Razor.Pages.MedicalPrescriptions
{
    [Authorize(Roles = "Doctor,Admin")]
    public class DeleteModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DeleteModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public MedicalPrescription MedicalPrescription { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null) return NotFound();

            var prescription = await _context.MedicalPrescriptions.FindAsync(id);
            if (prescription == null) return NotFound();

            MedicalPrescription = prescription;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null) return NotFound();

            var prescription = await _context.MedicalPrescriptions.FindAsync(id);
            if (prescription != null)
            {
                _context.MedicalPrescriptions.Remove(prescription);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("Index");
        }
    }
}
