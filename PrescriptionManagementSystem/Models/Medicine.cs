namespace PrescriptionManagementSystem.Models
{
    public class Medicine
    {
        public int MedicineId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public ICollection<PrescriptionDetail>? PrescriptionDetails { get; set; }
    }

}
