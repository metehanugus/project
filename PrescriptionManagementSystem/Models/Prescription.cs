using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PrescriptionManagementSystem.Models
{
    /// <summary>
    /// Represents a prescription with details and associated patient, physician, and pharmacy.
    /// </summary>
    public class Prescription
    {
        /// <summary>
        /// Gets or sets the unique identifier for the prescription.
        /// </summary>
        public int PrescriptionId { get; set; }

        /// <summary>
        /// Gets or sets the date when the prescription was issued.
        /// </summary>
        [Required(ErrorMessage = "Date is required")]
        public DateTime Date { get; set; }

        /// <summary>
        /// Gets or sets the total cost of the prescription.
        /// </summary>
        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Total Cost must be a positive number")]
        public decimal TotalCost { get; set; }

        /// <summary>
        /// Gets or sets the patient associated with this prescription.
        /// </summary>
        public int? PatientId { get; set; }
        public Patient? Patient { get; set; }

        /// <summary>
        /// Gets or sets the physician who issued this prescription.
        /// </summary>
        public int? PhysicianId { get; set; }
        public Physician? Physician { get; set; }

        /// <summary>
        /// Gets or sets the pharmacy where this prescription is registered.
        /// </summary>
        public int? PharmacyId { get; set; }
        public Pharmacy? Pharmacy { get; set; }

        /// <summary>
        /// Gets or sets the collection of prescription details associated with this prescription.
        /// </summary>
        public List<PrescriptionDetail> PrescriptionDetails { get; set; } = new List<PrescriptionDetail>();
    }
}
