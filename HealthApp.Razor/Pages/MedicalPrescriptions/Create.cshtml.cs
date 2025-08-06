using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using HealthApp.Razor.Data;
using HealthApp.Razor.Models;

namespace HealthApp.Razor.Pages.MedicalPrescriptions
{
    [Authorize(Roles = "Doctor,Admin")]
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public CreateModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public MedicalPrescription MedicalPrescription { get; set; } = new MedicalPrescription();

        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.MedicalPrescriptions.Add(MedicalPrescription);
            await _context.SaveChangesAsync();

            return RedirectToPage("Index");
        }
    }
}