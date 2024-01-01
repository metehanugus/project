using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PrescriptionManagementSystem.Models;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PrescriptionManagementSystem.Controllers
{
    /// <summary>
    /// Handles CRUD operations for physicians within the system.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class PhysicianController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<PhysicianController> _logger;

        public PhysicianController(ApplicationDbContext context, ILogger<PhysicianController> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Retrieves all physicians.
        /// </summary>
        /// <returns>A list of all physicians.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Physician>>> GetPhysicians()
        {
            _logger.LogInformation("Retrieving all physicians.");
            return await _context.Physicians.ToListAsync();
        }

        /// <summary>
        /// Retrieves a single physician by their unique identifier.
        /// </summary>
        /// <param name="id">The ID of the physician to retrieve.</param>
        /// <returns>The requested physician if found; otherwise, a NotFound result.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Physician>> GetPhysician(int id)
        {
            _logger.LogInformation($"Retrieving physician with ID {id}.");
            var physician = await _context.Physicians.FindAsync(id);

            if (physician == null)
            {
                _logger.LogWarning($"Physician with ID {id} not found.");
                return NotFound();
            }

            return physician;
        }

        /// <summary>
        /// Updates a specific physician.
        /// </summary>
        /// <param name="id">The ID of the physician to update.</param>
        /// <param name="physician">The updated physician object.</param>
        /// <returns>A NoContent result if successful; otherwise, appropriate error result.</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePhysician(int id, Physician physician)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Update operation called with invalid model state.");
                return BadRequest(ModelState);
            }

            if (id != physician.PhysicianId)
            {
                return BadRequest();
            }

            _context.Entry(physician).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                _logger.LogInformation($"Physician with ID {id} updated successfully.");
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!PhysicianExists(id))
                {
                    _logger.LogWarning($"Update failed. Physician with ID {id} not found.");
                    return NotFound();
                }
                else
                {
                    _logger.LogError(ex, $"An error occurred while updating physician with ID {id}.");
                    throw;
                }
            }

            return NoContent();
        }

        /// <summary>
        /// Creates a new physician.
        /// </summary>
        /// <param name="physician">The physician to create.</param>
        /// <returns>A CreatedAtActionResult with details of the new physician.</returns>
        [HttpPost]
        public async Task<ActionResult<Physician>> CreatePhysician(Physician physician)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Create operation called with invalid model state.");
                return BadRequest(ModelState);
            }

            _context.Physicians.Add(physician);
            await _context.SaveChangesAsync();
            _logger.LogInformation($"Physician {physician.Name} created with ID {physician.PhysicianId}.");

            return CreatedAtAction("GetPhysician", new { id = physician.PhysicianId }, physician);
        }

        /// <summary>
        /// Deletes a specific physician.
        /// </summary>
        /// <param name="id">The ID of the physician to delete.</param>
        /// <returns>A NoContent result if successful; otherwise, NotFound result.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePhysician(int id)
        {
            var physician = await _context.Physicians.FindAsync(id);
            if (physician == null)
            {
                _logger.LogWarning($"Delete operation failed. Physician with ID {id} not found.");
                return NotFound();
            }

            _context.Physicians.Remove(physician);
            await _context.SaveChangesAsync();
            _logger.LogInformation($"Physician with ID {id} deleted.");

            return NoContent();
        }

        private bool PhysicianExists(int id)
        {
            return _context.Physicians.Any(e => e.PhysicianId == id);
        }
    }
}
