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
    public class PrescriptionController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PrescriptionController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Prescription
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Prescription>>> GetPrescriptions()
        {
            return await _context.Prescriptions
                                 .Include(p => p.Pharmacy)
                                 .Include(p => p.Patient)
                                 .Include(p => p.Physician)
                                 .Include(p => p.PrescriptionDetails)
                                 .ThenInclude(pd => pd.Medicine)
                                 .ToListAsync();
        }

        // GET: api/Prescription/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Prescription>> GetPrescription(int id)
        {
            var prescription = await _context.Prescriptions
                                             .Include(p => p.Pharmacy)
                                             .Include(p => p.Patient)
                                             .Include(p => p.Physician)
                                             .Include(p => p.PrescriptionDetails)
                                             .ThenInclude(pd => pd.Medicine)
                                             .FirstOrDefaultAsync(p => p.PrescriptionId == id);

            if (prescription == null)
            {
                return NotFound();
            }

            return prescription;
        }

        // PUT: api/Prescription/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePrescription(int id, Prescription prescription)
        {
            if (id != prescription.PrescriptionId)
            {
                return BadRequest();
            }

            _context.Entry(prescription).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PrescriptionExists(id))
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

        // POST: api/Prescription
        [HttpPost]
        public async Task<ActionResult<Prescription>> CreatePrescription(Prescription prescription)
        {
            _context.Prescriptions.Add(prescription);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPrescription", new { id = prescription.PrescriptionId }, prescription);
        }

        // DELETE: api/Prescription/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePrescription(int id)
        {
            var prescription = await _context.Prescriptions.FindAsync(id);
            if (prescription == null)
            {
                return NotFound();
            }

            _context.Prescriptions.Remove(prescription);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PrescriptionExists(int id)
        {
            return _context.Prescriptions.Any(e => e.PrescriptionId == id);
        }
    }
}
