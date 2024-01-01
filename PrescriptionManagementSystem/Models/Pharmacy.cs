using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PrescriptionManagementSystem.Models
{
    /// <summary>
    /// Represents a pharmacy with details and associated prescriptions.
    /// </summary>
    public class Pharmacy
    {
        /// <summary>
        /// Gets or sets the unique identifier for the pharmacy.
        /// </summary>
        public int PharmacyId { get; set; }

        /// <summary>
        /// Gets or sets the name of the pharmacy.
        /// </summary>
        [Required(ErrorMessage = "Name is required")]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string? Name { get; set; }

        /// <summary>
        /// Gets or sets the address of the pharmacy.
        /// </summary>
        [StringLength(255, ErrorMessage = "Address cannot exceed 255 characters")]
        public string? Address { get; set; }

        /// <summary>
        /// Gets or sets the email of the pharmacy.
        /// </summary>
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string? Email { get; set; }

        /// <summary>
        /// Gets or sets the collection of prescriptions associated with this pharmacy.
        /// </summary>
        public ICollection<Prescription>? Prescriptions { get; set; }
    }
}
