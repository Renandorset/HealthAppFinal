namespace HealthApp.Razor.Data
{
    public class Feedback
    {
        public int Id { get; set; }
        public string DoctorName { get; set; }
        public string PatientName { get; set; }
        public int Rating { get; set; } // 1 a 5
        public string Comment { get; set; }
    }
}
