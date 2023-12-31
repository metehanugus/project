namespace PrescriptionManagementSystem.Models
{
    public class Pharmacy
    {
        public int PharmacyId { get; set; }
        public string? Name { get; set; }
        public string? Address { get; set; }
        public string? Email { get; set; }
        public ICollection<Prescription>? Prescriptions { get; set; }
    }

}
