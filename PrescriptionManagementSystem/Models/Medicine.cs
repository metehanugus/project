using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PrescriptionManagementSystem.Models
{
    /// <summary>
    /// Represents a medicine with properties for name, description, and price.
    /// </summary>
    public class Medicine
    {
        /// <summary>
        /// Gets or sets the unique identifier for the medicine.
        /// </summary>
        public int MedicineId { get; set; }

        /// <summary>
        /// Gets or sets the name of the medicine.
        /// </summary>
        [Required(ErrorMessage = "Medicine name is required")]
        [StringLength(100, ErrorMessage = "Medicine name cannot exceed 100 characters")]
        public string? Name { get; set; }

        /// <summary>
        /// Gets or sets a description of the medicine.
        /// </summary>
        [StringLength(255, ErrorMessage = "Description cannot exceed 255 characters")]
        public string? Description { get; set; }

        /// <summary>
        /// Gets or sets the price of the medicine.
        /// </summary>
        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Price must be a positive number")]
        public decimal Price { get; set; }

        /// <summary>
        /// Gets or sets the collection of prescription details associated with this medicine.
        /// </summary>
        public ICollection<PrescriptionDetail>? PrescriptionDetails { get; set; }
    }
}
