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
    public class PrescriptionDetailController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PrescriptionDetailController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/PrescriptionDetail
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PrescriptionDetail>>> GetPrescriptionDetails()
        {
            return await _context.PrescriptionDetails
                                 .Include(pd => pd.Prescription)
                                 .Include(pd => pd.Medicine)
                                 .ToListAsync();
        }

        // GET: api/PrescriptionDetail/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PrescriptionDetail>> GetPrescriptionDetail(int id)
        {
            var prescriptionDetail = await _context.PrescriptionDetails
                                                  .Include(pd => pd.Prescription)
                                                  .Include(pd => pd.Medicine)
                                                  .FirstOrDefaultAsync(pd => pd.PrescriptionDetailId == id);

            if (prescriptionDetail == null)
            {
                return NotFound();
            }

            return prescriptionDetail;
        }

        // PUT: api/PrescriptionDetail/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePrescriptionDetail(int id, PrescriptionDetail prescriptionDetail)
        {
            if (id != prescriptionDetail.PrescriptionDetailId)
            {
                return BadRequest();
            }

            _context.Entry(prescriptionDetail).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PrescriptionDetailExists(id))
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

        // POST: api/PrescriptionDetail
        [HttpPost]
        public async Task<ActionResult<PrescriptionDetail>> CreatePrescriptionDetail(PrescriptionDetail prescriptionDetail)
        {
            _context.PrescriptionDetails.Add(prescriptionDetail);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPrescriptionDetail", new { id = prescriptionDetail.PrescriptionDetailId }, prescriptionDetail);
        }

        // DELETE: api/PrescriptionDetail/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePrescriptionDetail(int id)
        {
            var prescriptionDetail = await _context.PrescriptionDetails.FindAsync(id);
            if (prescriptionDetail == null)
            {
                return NotFound();
            }

            _context.PrescriptionDetails.Remove(prescriptionDetail);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PrescriptionDetailExists(int id)
        {
            return _context.PrescriptionDetails.Any(e => e.PrescriptionDetailId == id);
        }
    }
}
