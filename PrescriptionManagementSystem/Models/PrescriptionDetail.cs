namespace PrescriptionManagementSystem.Models
{
    public class PrescriptionDetail
    {
        public int PrescriptionDetailId { get; set; }
        public int PrescriptionId { get; set; }
        public int MedicineId { get; set; }
        public int Quantity { get; set; }
        public string? Dosage { get; set; }

        public Prescription? Prescription { get; set; }
        public Medicine? Medicine { get; set; }
    }

}
