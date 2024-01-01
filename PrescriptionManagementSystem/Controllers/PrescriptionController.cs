using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PrescriptionManagementSystem.Models;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PrescriptionManagementSystem.Controllers
{
    /// <summary>
    /// Handles CRUD operations for prescriptions within the system.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class PrescriptionController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<PrescriptionController> _logger;

        public PrescriptionController(ApplicationDbContext context, ILogger<PrescriptionController> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Retrieves all prescriptions with their related details.
        /// </summary>
        /// <returns>A list of all prescriptions with details.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Prescription>>> GetPrescriptions()
        {
            _logger.LogInformation("Retrieving all prescriptions with details.");
            return await _context.Prescriptions
                                 .Include(p => p.Pharmacy)
                                 .Include(p => p.Patient)
                                 .Include(p => p.Physician)
                                 .Include(p => p.PrescriptionDetails)
                                 .ThenInclude(pd => pd.Medicine)
                                 .ToListAsync();
        }

        /// <summary>
        /// Retrieves a single prescription by its unique identifier with all related details.
        /// </summary>
        /// <param name="id">The ID of the prescription to retrieve.</param>
        /// <returns>The requested prescription with details if found; otherwise, a NotFound result.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Prescription>> GetPrescription(int id)
        {
            _logger.LogInformation($"Retrieving prescription with ID {id} and its details.");
            var prescription = await _context.Prescriptions
                                             .Include(p => p.Pharmacy)
                                             .Include(p => p.Patient)
                                             .Include(p => p.Physician)
                                             .Include(p => p.PrescriptionDetails)
                                             .ThenInclude(pd => pd.Medicine)
                                             .FirstOrDefaultAsync(p => p.PrescriptionId == id);

            if (prescription == null)
            {
                _logger.LogWarning($"Prescription with ID {id} not found.");
                return NotFound();
            }

            return prescription;
        }

        /// <summary>
        /// Updates a specific prescription and its details.
        /// </summary>
        /// <param name="id">The ID of the prescription to update.</param>
        /// <param name="prescription">The updated prescription object.</param>
        /// <returns>A NoContent result if successful; otherwise, appropriate error result.</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePrescription(int id, Prescription prescription)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Update operation called with invalid model state.");
                return BadRequest(ModelState);
            }

            if (id != prescription.PrescriptionId)
            {
                return BadRequest();
            }

            _context.Entry(prescription).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                _logger.LogInformation($"Prescription with ID {id} updated successfully.");
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!PrescriptionExists(id))
                {
                    _logger.LogWarning($"Update failed. Prescription with ID {id} not found.");
                    return NotFound();
                }
                else
                {
                    _logger.LogError(ex, $"An error occurred while updating prescription with ID {id}.");
                    throw;
                }
            }

            return NoContent();
        }

        /// <summary>
        /// Creates a new prescription with its details.
        /// </summary>
        /// <param name="prescription">The prescription to create.</param>
        /// <returns>A CreatedAtActionResult with details of the new prescription.</returns>
        [HttpPost]
        public async Task<ActionResult<Prescription>> CreatePrescription(Prescription prescription)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Create operation called with invalid model state.");
                return BadRequest(ModelState);
            }

            _context.Prescriptions.Add(prescription);
            await _context.SaveChangesAsync();
            _logger.LogInformation($"Prescription created with ID {prescription.PrescriptionId}.");

            return CreatedAtAction("GetPrescription", new { id = prescription.PrescriptionId }, prescription);
        }

        /// <summary>
        /// Deletes a specific prescription and its details.
        /// </summary>
        /// <param name="id">The ID of the prescription to delete.</param>
        /// <returns>A NoContent result if successful; otherwise, NotFound result.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePrescription(int id)
        {
            var prescription = await _context.Prescriptions.FindAsync(id);
            if (prescription == null)
            {
                _logger.LogWarning($"Delete operation failed. Prescription with ID {id} not found.");
                return NotFound();
            }

            _context.Prescriptions.Remove(prescription);
            await _context.SaveChangesAsync();
            _logger.LogInformation($"Prescription with ID {id} deleted.");

            return NoContent();
        }

        private bool PrescriptionExists(int id)
        {
            return _context.Prescriptions.Any(e => e.PrescriptionId == id);
        }
    }
}
