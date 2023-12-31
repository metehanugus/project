using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PrescriptionManagementSystem.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrescriptionManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MedicineController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public MedicineController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Medicine
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Medicine>>> GetMedicines()
        {
            return await _context.Medicines.ToListAsync();
        }

        // GET: api/Medicine/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Medicine>> GetMedicine(int id)
        {
            var medicine = await _context.Medicines.FindAsync(id);

            if (medicine == null)
            {
                return NotFound();
            }

            return medicine;
        }

        // PUT: api/Medicine/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMedicine(int id, Medicine medicine)
        {
            if (id != medicine.MedicineId)
            {
                return BadRequest();
            }

            _context.Entry(medicine).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MedicineExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Medicine
        [HttpPost]
        public async Task<ActionResult<Medicine>> CreateMedicine(Medicine medicine)
        {
            _context.Medicines.Add(medicine);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMedicine", new { id = medicine.MedicineId }, medicine);
        }

        // DELETE: api/Medicine/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMedicine(int id)
        {
            var medicine = await _context.Medicines.FindAsync(id);
            if (medicine == null)
            {
                return NotFound();
            }

            _context.Medicines.Remove(medicine);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MedicineExists(int id)
        {
            return _context.Medicines.Any(e => e.MedicineId == id);
        }
    }
}
