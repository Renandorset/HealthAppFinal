using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore; // <- necessário para EntityState e DbUpdateConcurrencyException
using HealthApp.Razor.Data;

namespace HealthApp.Razor.Pages.MedicalPrescriptions
{

    [Authorize(Roles = "Doctor,Admin")]
    // Restringe o acesso apenas a médicos
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public EditModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public MedicalPrescription MedicalPrescription { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
                return NotFound();

            var prescription = await _context.MedicalPrescriptions.FindAsync(id);

            if (prescription == null)
                return NotFound();

            MedicalPrescription = prescription;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            _context.Attach(MedicalPrescription).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.MedicalPrescriptions.Any(e => e.Id == MedicalPrescription.Id))
                    return NotFound();
                else
                    throw;
            }

            return RedirectToPage("Index");
        }
    }
}
