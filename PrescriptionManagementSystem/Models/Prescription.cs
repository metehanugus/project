namespace PrescriptionManagementSystem.Models
{

    public class Prescription
    {
        public Prescription()
        {
            PrescriptionDetails = new List<PrescriptionDetail>();
        }

        public int PrescriptionId { get; set; }
        public DateTime Date { get; set; }
        public decimal TotalCost { get; set; }
        public int PatientId { get; set; }
        public int PhysicianId { get; set; }
        public int PharmacyId { get; set; }

        public Patient? Patient { get; set; }
        public Physician? Physician { get; set; }
        public Pharmacy? Pharmacy { get; set; }
        public List<PrescriptionDetail> PrescriptionDetails { get; set; }
    }

}
