using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PrescriptionManagementSystem.Models
{
    /// <summary>
    /// Represents a physician with contact details and associated prescriptions.
    /// </summary>
    public class Physician
    {
        /// <summary>
        /// Gets or sets the unique identifier for the physician.
        /// </summary>
        public int PhysicianId { get; set; }

        /// <summary>
        /// Gets or sets the name of the physician.
        /// </summary>
        [Required(ErrorMessage = "Name is required")]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string? Name { get; set; }

        /// <summary>
        /// Gets or sets the specialty of the physician.
        /// </summary>
        [StringLength(100, ErrorMessage = "Specialty cannot exceed 100 characters")]
        public string? Specialty { get; set; }

        /// <summary>
        /// Gets or sets the contact information for the physician.
        /// </summary>
        [StringLength(255, ErrorMessage = "Contact Information cannot exceed 255 characters")]
        public string? ContactInfo { get; set; }

        /// <summary>
        /// Gets or sets the collection of prescriptions associated with this physician.
        /// </summary>
        public ICollection<Prescription>? Prescriptions { get; set; }
    }
}
