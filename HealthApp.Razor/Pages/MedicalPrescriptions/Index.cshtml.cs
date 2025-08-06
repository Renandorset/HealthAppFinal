using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using HealthApp.Razor.Models;
using HealthApp.Razor.Data;

namespace HealthApp.Razor.Pages.MedicalPrescriptions
{
    [Authorize(Roles = "Doctor,Admin")] // ← AQUI você protege a página
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<MedicalPrescription> MedicalPrescriptions { get; set; } = default!;

        public async Task OnGetAsync()
        {
            MedicalPrescriptions = await _context.MedicalPrescriptions.ToListAsync();
        }
    }
}
