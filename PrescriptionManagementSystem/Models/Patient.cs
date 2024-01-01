using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PrescriptionManagementSystem.Models
{
    /// <summary>
    /// Represents a patient with personal details and associated prescriptions.
    /// </summary>
    public class Patient
    {
        /// <summary>
        /// Gets or sets the unique identifier for the patient.
        /// </summary>
        public int PatientId { get; set; }

        /// <summary>
        /// Gets or sets the name of the patient.
        /// </summary>
        [Required(ErrorMessage = "Name is required")]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string? Name { get; set; }

        /// <summary>
        /// Gets or sets the date of birth of the patient.
        /// </summary>
        [Required(ErrorMessage = "Date of Birth is required")]
        public DateTime DOB { get; set; }

        /// <summary>
        /// Gets or sets the gender of the patient.
        /// </summary>
        [StringLength(20, ErrorMessage = "Gender cannot exceed 20 characters")]
        public string? Gender { get; set; }

        /// <summary>
        /// Gets or sets the collection of prescriptions associated with this patient.
        /// </summary>
        public ICollection<Prescription>? Prescriptions { get; set; }
    }
}
