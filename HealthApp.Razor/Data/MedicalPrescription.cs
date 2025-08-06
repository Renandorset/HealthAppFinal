    using System;
    using System.ComponentModel.DataAnnotations;


// Models/MedicalPrescription.cs
namespace HealthApp.Razor.Models
{
    public class MedicalPrescription
    {
        public int Id { get; set; }
        public string PatientName { get; set; } = string.Empty;
        public string Medication { get; set; } = string.Empty;
        public string Dosage { get; set; } = string.Empty;
        public DateTime DatePrescribed { get; set; }
    }
}
