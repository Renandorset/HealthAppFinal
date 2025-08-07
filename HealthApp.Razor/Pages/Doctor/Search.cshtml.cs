using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using HealthApp.Razor.Data;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using DoctorEntity = HealthApp.Razor.Data.Doctor;

namespace HealthApp.Razor.Pages.Doctor
{
    [Authorize(Policy = "PatientOrAdmin")] // ✅ agora compatível com Program.cs
    public class SearchModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public SearchModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty(SupportsGet = true)]
        public string Query { get; set; }

        public List<DoctorEntity> Doctors { get; set; } = new();

        public void OnGet()
        {
            var query = _context.Doctors.AsQueryable();

            if (!string.IsNullOrWhiteSpace(Query))
            {
                query = query.Where(d =>
                    d.FullName.Contains(Query) ||
                    d.Specialty.Contains(Query) ||
                    d.Location.Contains(Query));
            }

            Doctors = query.ToList();
        }
    }
}
