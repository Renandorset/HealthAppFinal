using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using HealthApp.Razor.Data;

namespace HealthApp.Razor.Pages.Doctor
{
    [Authorize(Roles = "Doctor,Admin")]
    public class ManageDoctorAppointmentsModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public ManageDoctorAppointmentsModel(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IList<Appointment> Appointments { get; set; } = [];

        [BindProperty]
        public int AppointmentId { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return Forbid();

            Appointments = await _context.Appointments
                .Where(a => a.DoctorEmail == user.Email)
                .OrderByDescending(a => a.Date)
                .ToListAsync();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string action)
        {
            var appointment = await _context.Appointments.FindAsync(AppointmentId);

            if (appointment == null || appointment.Status != "Pending")
                return RedirectToPage();

            switch (action)
            {
                case "Approve":
                    appointment.Status = "Approved";
                    break;
                case "Reject":
                    appointment.Status = "Rejected";
                    break;
            }

            await _context.SaveChangesAsync();
            return RedirectToPage();
        }
    }
}
