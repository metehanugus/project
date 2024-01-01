using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PrescriptionManagementSystem.Models;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PrescriptionManagementSystem.Controllers
{
    /// <summary>
    /// Handles CRUD operations for pharmacies within the system.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class PharmacyController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<PharmacyController> _logger;

        public PharmacyController(ApplicationDbContext context, ILogger<PharmacyController> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Retrieves all pharmacies.
        /// </summary>
        /// <returns>A list of all pharmacies.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Pharmacy>>> GetPharmacies()
        {
            _logger.LogInformation("Retrieving all pharmacies.");
            return await _context.Pharmacies.ToListAsync();
        }

        /// <summary>
        /// Retrieves a single pharmacy by its unique identifier.
        /// </summary>
        /// <param name="id">The ID of the pharmacy to retrieve.</param>
        /// <returns>The requested pharmacy if found; otherwise, a NotFound result.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Pharmacy>> GetPharmacy(int id)
        {
            _logger.LogInformation($"Retrieving pharmacy with ID {id}.");
            var pharmacy = await _context.Pharmacies.FindAsync(id);

            if (pharmacy == null)
            {
                _logger.LogWarning($"Pharmacy with ID {id} not found.");
                return NotFound();
            }

            return pharmacy;
        }

        /// <summary>
        /// Updates a specific pharmacy.
        /// </summary>
        /// <param name="id">The ID of the pharmacy to update.</param>
        /// <param name="pharmacy">The updated pharmacy object.</param>
        /// <returns>A NoContent result if successful; otherwise, appropriate error result.</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePharmacy(int id, Pharmacy pharmacy)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Update operation called with invalid model state.");
                return BadRequest(ModelState);
            }

            if (id != pharmacy.PharmacyId)
            {
                return BadRequest();
            }

            _context.Entry(pharmacy).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                _logger.LogInformation($"Pharmacy with ID {id} updated successfully.");
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!PharmacyExists(id))
                {
                    _logger.LogWarning($"Update failed. Pharmacy with ID {id} not found.");
                    return NotFound();
                }
                else
                {
                    _logger.LogError(ex, $"An error occurred while updating pharmacy with ID {id}.");
                    throw;
                }
            }

            return NoContent();
        }

        /// <summary>
        /// Creates a new pharmacy.
        /// </summary>
        /// <param name="pharmacy">The pharmacy to create.</param>
        /// <returns>A CreatedAtActionResult with details of the new pharmacy.</returns>
        [HttpPost]
        public async Task<ActionResult<Pharmacy>> CreatePharmacy(Pharmacy pharmacy)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Create operation called with invalid model state.");
                return BadRequest(ModelState);
            }

            _context.Pharmacies.Add(pharmacy);
            await _context.SaveChangesAsync();
            _logger.LogInformation($"Pharmacy {pharmacy.Name} created with ID {pharmacy.PharmacyId}.");

            return CreatedAtAction("GetPharmacy", new { id = pharmacy.PharmacyId }, pharmacy);
        }

        /// <summary>
        /// Deletes a specific pharmacy.
        /// </summary>
        /// <param name="id">The ID of the pharmacy to delete.</param>
        /// <returns>A NoContent result if successful; otherwise, NotFound result.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePharmacy(int id)
        {
            var pharmacy = await _context.Pharmacies.FindAsync(id);
            if (pharmacy == null)
            {
                _logger.LogWarning($"Delete operation failed. Pharmacy with ID {id} not found.");
                return NotFound();
            }

            _context.Pharmacies.Remove(pharmacy);
            await _context.SaveChangesAsync();
            _logger.LogInformation($"Pharmacy with ID {id} deleted.");

            return NoContent();
        }

        private bool PharmacyExists(int id)
        {
            return _context.Pharmacies.Any(e => e.PharmacyId == id);
        }
    }
}
