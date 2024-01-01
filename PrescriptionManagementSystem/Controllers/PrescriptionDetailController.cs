using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PrescriptionManagementSystem.Models;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PrescriptionManagementSystem.Controllers
{
    /// <summary>
    /// Handles CRUD operations for prescription details within the system.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class PrescriptionDetailController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<PrescriptionDetailController> _logger;

        public PrescriptionDetailController(ApplicationDbContext context, ILogger<PrescriptionDetailController> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Retrieves all prescription details.
        /// </summary>
        /// <returns>A list of all prescription details.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PrescriptionDetail>>> GetPrescriptionDetails()
        {
            _logger.LogInformation("Retrieving all prescription details.");
            return await _context.PrescriptionDetails
                                 .Include(pd => pd.Prescription)
                                 .Include(pd => pd.Medicine)
                                 .ToListAsync();
        }

        /// <summary>
        /// Retrieves a single prescription detail by its unique identifier.
        /// </summary>
        /// <param name="id">The ID of the prescription detail to retrieve.</param>
        /// <returns>The requested prescription detail if found; otherwise, a NotFound result.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<PrescriptionDetail>> GetPrescriptionDetail(int id)
        {
            _logger.LogInformation($"Retrieving prescription detail with ID {id}.");
            var prescriptionDetail = await _context.PrescriptionDetails
                                                  .Include(pd => pd.Prescription)
                                                  .Include(pd => pd.Medicine)
                                                  .FirstOrDefaultAsync(pd => pd.PrescriptionDetailId == id);

            if (prescriptionDetail == null)
            {
                _logger.LogWarning($"Prescription detail with ID {id} not found.");
                return NotFound();
            }

            return prescriptionDetail;
        }

        /// <summary>
        /// Updates a specific prescription detail.
        /// </summary>
        /// <param name="id">The ID of the prescription detail to update.</param>
        /// <param name="prescriptionDetail">The updated prescription detail object.</param>
        /// <returns>A NoContent result if successful; otherwise, appropriate error result.</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePrescriptionDetail(int id, PrescriptionDetail prescriptionDetail)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Update operation called with invalid model state.");
                return BadRequest(ModelState);
            }

            if (id != prescriptionDetail.PrescriptionDetailId)
            {
                return BadRequest();
            }

            _context.Entry(prescriptionDetail).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                _logger.LogInformation($"Prescription detail with ID {id} updated successfully.");
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!PrescriptionDetailExists(id))
                {
                    _logger.LogWarning($"Update failed. Prescription detail with ID {id} not found.");
                    return NotFound();
                }
                else
                {
                    _logger.LogError(ex, $"An error occurred while updating prescription detail with ID {id}.");
                    throw;
                }
            }

            return NoContent();
        }

        /// <summary>
        /// Creates a new prescription detail.
        /// </summary>
        /// <param name="prescriptionDetail">The prescription detail to create.</param>
        /// <returns>A CreatedAtActionResult with details of the new prescription detail.</returns>
        [HttpPost]
        public async Task<ActionResult<PrescriptionDetail>> CreatePrescriptionDetail(PrescriptionDetail prescriptionDetail)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Create operation called with invalid model state.");
                return BadRequest(ModelState);
            }

            _context.PrescriptionDetails.Add(prescriptionDetail);
            await _context.SaveChangesAsync();
            _logger.LogInformation($"Prescription detail created with ID {prescriptionDetail.PrescriptionDetailId}.");

            return CreatedAtAction("GetPrescriptionDetail", new { id = prescriptionDetail.PrescriptionDetailId }, prescriptionDetail);
        }

        /// <summary>
        /// Deletes a specific prescription detail.
        /// </summary>
        /// <param name="id">The ID of the prescription detail to delete.</param>
        /// <returns>A NoContent result if successful; otherwise, NotFound result.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePrescriptionDetail(int id)
        {
            var prescriptionDetail = await _context.PrescriptionDetails.FindAsync(id);
            if (prescriptionDetail == null)
            {
                _logger.LogWarning($"Delete operation failed. Prescription detail with ID {id} not found.");
                return NotFound();
            }

            _context.PrescriptionDetails.Remove(prescriptionDetail);
            await _context.SaveChangesAsync();
            _logger.LogInformation($"Prescription detail with ID {id} deleted.");

            return NoContent();
        }

        private bool PrescriptionDetailExists(int id)
        {
            return _context.PrescriptionDetails.Any(e => e.PrescriptionDetailId == id);
        }
    }
}
