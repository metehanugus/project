namespace PrescriptionManagementSystem.Models
{
    public class Patient
    {
        public int PatientId { get; set; }
        public string? Name { get; set; }
        public DateTime DOB { get; set; }
        public string? Gender { get; set; }
        public ICollection<Prescription>? Prescriptions { get; set; }
    }

}
