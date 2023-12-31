namespace PrescriptionManagementSystem.Models
{
    public class Physician
    {
        public int PhysicianId { get; set; }
        public string? Name { get; set; }
        public string? Specialty { get; set; }
        public string? ContactInfo { get; set; }
        public ICollection<Prescription>? Prescriptions { get; set; }
    }

}
