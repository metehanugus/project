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
    public class PhysicianController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PhysicianController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Physician
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Physician>>> GetPhysicians()
        {
            return await _context.Physicians.ToListAsync();
        }

        // GET: api/Physician/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Physician>> GetPhysician(int id)
        {
            var physician = await _context.Physicians.FindAsync(id);

            if (physician == null)
            {
                return NotFound();
            }

            return physician;
        }

        // PUT: api/Physician/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePhysician(int id, Physician physician)
        {
            if (id != physician.PhysicianId)
            {
                return BadRequest();
            }

            _context.Entry(physician).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PhysicianExists(id))
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

        // POST: api/Physician
        [HttpPost]
        public async Task<ActionResult<Physician>> CreatePhysician(Physician physician)
        {
            _context.Physicians.Add(physician);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPhysician", new { id = physician.PhysicianId }, physician);
        }

        // DELETE: api/Physician/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePhysician(int id)
        {
            var physician = await _context.Physicians.FindAsync(id);
            if (physician == null)
            {
                return NotFound();
            }

            _context.Physicians.Remove(physician);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PhysicianExists(int id)
        {
            return _context.Physicians.Any(e => e.PhysicianId == id);
        }
    }
}
