using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PrescriptionManagementSystem.Models;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PrescriptionManagementSystem.Controllers
{
    /// <summary>
    /// Handles CRUD operations for medicines within the system.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class MedicineController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<MedicineController> _logger;

        public MedicineController(ApplicationDbContext context, ILogger<MedicineController> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Retrieves all medicines.
        /// </summary>
        /// <returns>A list of all medicines.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Medicine>>> GetMedicines()
        {
            _logger.LogInformation("Retrieving all medicines.");
            return await _context.Medicines.ToListAsync();
        }

        /// <summary>
        /// Retrieves a single medicine by its unique identifier.
        /// </summary>
        /// <param name="id">The ID of the medicine to retrieve.</param>
        /// <returns>The requested medicine if found; otherwise, a NotFound result.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Medicine>> GetMedicine(int id)
        {
            _logger.LogInformation($"Retrieving medicine with ID {id}.");
            var medicine = await _context.Medicines.FindAsync(id);

            if (medicine == null)
            {
                _logger.LogWarning($"Medicine with ID {id} not found.");
                return NotFound();
            }

            return medicine;
        }

        /// <summary>
        /// Updates a specific medicine.
        /// </summary>
        /// <param name="id">The ID of the medicine to update.</param>
        /// <param name="medicine">The updated medicine object.</param>
        /// <returns>A NoContent result if successful; otherwise, appropriate error result.</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMedicine(int id, Medicine medicine)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Update operation called with invalid model state.");
                return BadRequest(ModelState);
            }

            if (id != medicine.MedicineId)
            {
                return BadRequest();
            }

            _context.Entry(medicine).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                _logger.LogInformation($"Medicine with ID {id} updated successfully.");
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!MedicineExists(id))
                {
                    _logger.LogWarning($"Update failed. Medicine with ID {id} not found.");
                    return NotFound();
                }
                else
                {
                    _logger.LogError(ex, $"An error occurred while updating medicine with ID {id}.");
                    throw;
                }
            }

            return NoContent();
        }

        /// <summary>
        /// Creates a new medicine.
        /// </summary>
        /// <param name="medicine">The medicine to create.</param>
        /// <returns>A CreatedAtActionResult with details of the new medicine.</returns>
        [HttpPost]
        public async Task<ActionResult<Medicine>> CreateMedicine(Medicine medicine)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Create operation called with invalid model state.");
                return BadRequest(ModelState);
            }

            _context.Medicines.Add(medicine);
            await _context.SaveChangesAsync();
            _logger.LogInformation($"Medicine {medicine.Name} created with ID {medicine.MedicineId}.");

            return CreatedAtAction("GetMedicine", new { id = medicine.MedicineId }, medicine);
        }

        /// <summary>
        /// Deletes a specific medicine.
        /// </summary>
        /// <param name="id">The ID of the medicine to delete.</param>
        /// <returns>A NoContent result if successful; otherwise, NotFound result.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMedicine(int id)
        {
            var medicine = await _context.Medicines.FindAsync(id);
            if (medicine == null)
            {
                _logger.LogWarning($"Delete operation failed. Medicine with ID {id} not found.");
                return NotFound();
            }

            _context.Medicines.Remove(medicine);
            await _context.SaveChangesAsync();
            _logger.LogInformation($"Medicine with ID {id} deleted.");

            return NoContent();
        }

        private bool MedicineExists(int id)
        {
            return _context.Medicines.Any(e => e.MedicineId == id);
        }
    }
}
