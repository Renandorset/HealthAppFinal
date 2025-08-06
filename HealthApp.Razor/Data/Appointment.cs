using System;
using System.ComponentModel.DataAnnotations;

namespace HealthApp.Razor.Data
{
    public class Appointment
    {
        public int Id { get; set; }

        [Required]
        public string PatientName { get; set; } = string.Empty;

        [Required]
        public string PatientEmail { get; set; } = string.Empty;

        [Required]
        public string DoctorEmail { get; set; } = string.Empty;

        [Required]
        public DateTime Date { get; set; }

        public string? Notes { get; set; }

        [Required]
        public string Status { get; set; } = "Pending";  // Pode ser Pending, Approved, Rejected, etc.
    }
}
