using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PrescriptionManagementSystem.Models;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PrescriptionManagementSystem.Controllers
{
    /// <summary>
    /// Handles CRUD operations for patients within the system.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class PatientController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<PatientController> _logger;

        public PatientController(ApplicationDbContext context, ILogger<PatientController> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Retrieves all patients.
        /// </summary>
        /// <returns>A list of all patients.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Patient>>> GetPatients()
        {
            _logger.LogInformation("Retrieving all patients.");
            return await _context.Patients.ToListAsync();
        }

        /// <summary>
        /// Retrieves a single patient by their unique identifier.
        /// </summary>
        /// <param name="id">The ID of the patient to retrieve.</param>
        /// <returns>The requested patient if found; otherwise, a NotFound result.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Patient>> GetPatient(int id)
        {
            _logger.LogInformation($"Retrieving patient with ID {id}.");
            var patient = await _context.Patients.FindAsync(id);

            if (patient == null)
            {
                _logger.LogWarning($"Patient with ID {id} not found.");
                return NotFound();
            }

            return patient;
        }

        /// <summary>
        /// Updates a specific patient.
        /// </summary>
        /// <param name="id">The ID of the patient to update.</param>
        /// <param name="patient">The updated patient object.</param>
        /// <returns>A NoContent result if successful; otherwise, appropriate error result.</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePatient(int id, Patient patient)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Update operation called with invalid model state.");
                return BadRequest(ModelState);
            }

            if (id != patient.PatientId)
            {
                return BadRequest();
            }

            _context.Entry(patient).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                _logger.LogInformation($"Patient with ID {id} updated successfully.");
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!PatientExists(id))
                {
                    _logger.LogWarning($"Update failed. Patient with ID {id} not found.");
                    return NotFound();
                }
                else
                {
                    _logger.LogError(ex, $"An error occurred while updating patient with ID {id}.");
                    throw;
                }
            }

            return NoContent();
        }

        /// <summary>
        /// Creates a new patient.
        /// </summary>
        /// <param name="patient">The patient to create.</param>
        /// <returns>A CreatedAtActionResult with details of the new patient.</returns>
        [HttpPost]
        public async Task<ActionResult<Patient>> CreatePatient(Patient patient)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Create operation called with invalid model state.");
                return BadRequest(ModelState);
            }

            _context.Patients.Add(patient);
            await _context.SaveChangesAsync();
            _logger.LogInformation($"Patient {patient.Name} created with ID {patient.PatientId}.");

            return CreatedAtAction("GetPatient", new { id = patient.PatientId }, patient);
        }

        /// <summary>
        /// Deletes a specific patient.
        /// </summary>
        /// <param name="id">The ID of the patient to delete.</param>
        /// <returns>A NoContent result if successful; otherwise, NotFound result.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePatient(int id)
        {
            var patient = await _context.Patients.FindAsync(id);
            if (patient == null)
            {
                _logger.LogWarning($"Delete operation failed. Patient with ID {id} not found.");
                return NotFound();
            }

            _context.Patients.Remove(patient);
            await _context.SaveChangesAsync();
            _logger.LogInformation($"Patient with ID {id} deleted.");

            return NoContent();
        }

        private bool PatientExists(int id)
        {
            return _context.Patients.Any(e => e.PatientId == id);
        }
    }
}
