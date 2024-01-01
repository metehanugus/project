using System.ComponentModel.DataAnnotations;

namespace PrescriptionManagementSystem.Models
{
    /// <summary>
    /// Represents the details of a prescription including the prescribed medicine and dosage.
    /// </summary>
    public class PrescriptionDetail
    {
        /// <summary>
        /// Gets or sets the unique identifier for the prescription detail.
        /// </summary>
        public int PrescriptionDetailId { get; set; }

        /// <summary>
        /// Gets or sets the quantity of the medicine prescribed.
        /// </summary>
        [Required(ErrorMessage = "Quantity is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1")]
        public int Quantity { get; set; }

        /// <summary>
        /// Gets or sets the dosage instructions for the medicine.
        /// </summary>
        [StringLength(255, ErrorMessage = "Dosage instructions cannot exceed 255 characters")]
        public string? Dosage { get; set; }

        /// <summary>
        /// Gets or sets the ID of the associated prescription.
        /// </summary>
        public int? PrescriptionId { get; set; }
        public Prescription? Prescription { get; set; }

        /// <summary>
        /// Gets or sets the ID of the medicine prescribed.
        /// </summary>
        public int? MedicineId { get; set; }
        public Medicine? Medicine { get; set; }
    }
}
