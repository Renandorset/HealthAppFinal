using System;
using System.ComponentModel.DataAnnotations;

namespace HealthApp.Razor.Data
{
    public class Appointment
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Patient Name")]
        public string PatientName { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Patient Email")]
        public string PatientEmail { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Doctor Email")]
        public string DoctorEmail { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Appointment Date")]
        public DateTime Date { get; set; }

        [Display(Name = "Notes")]
        public string? Notes { get; set; }

        [Required]
        [Display(Name = "Status")]
        public string Status { get; set; } = "Pending";
    }
}
