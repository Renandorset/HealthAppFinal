using System;
using System.ComponentModel.DataAnnotations;

namespace HealthApp.Razor.Data
{
    public class MedicalPrescription
    {
        public int Id { get; set; }

        [Required]
        public string DoctorName { get; set; }

        [Required]
        public string PatientName { get; set; }

        [Required]
        public string Medication { get; set; }

        [Required]
        public string Dosage { get; set; }

        [Required]
        public string Instructions { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Date Prescribed")]
        public DateTime DatePrescribed { get; set; } = DateTime.Now;
    }
}
