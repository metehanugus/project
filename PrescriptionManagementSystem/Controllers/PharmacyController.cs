using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using global::PrescriptionManagementSystem.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrescriptionManagementSystem.Controllers
    {
        [Route("api/[controller]")]
        [ApiController]
        public class PharmacyController : ControllerBase
        {
            private readonly ApplicationDbContext _context;

            public PharmacyController(ApplicationDbContext context)
            {
                _context = context;
            }

            // GET: api/Pharmacy
            [HttpGet]
            public async Task<ActionResult<IEnumerable<Pharmacy>>> GetPharmacies()
            {
                return await _context.Pharmacies.ToListAsync();
            }

            // GET: api/Pharmacy/5
            [HttpGet("{id}")]
            public async Task<ActionResult<Pharmacy>> GetPharmacy(int id)
            {
                var pharmacy = await _context.Pharmacies.FindAsync(id);

                if (pharmacy == null)
                {
                    return NotFound();
                }

                return pharmacy;
            }

            // PUT: api/Pharmacy/5
            [HttpPut("{id}")]
            public async Task<IActionResult> UpdatePharmacy(int id, Pharmacy pharmacy)
            {
                if (id != pharmacy.PharmacyId)
                {
                    return BadRequest();
                }

                _context.Entry(pharmacy).State = EntityState.Modified;

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PharmacyExists(id))
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

            // POST: api/Pharmacy
            [HttpPost]
            public async Task<ActionResult<Pharmacy>> CreatePharmacy(Pharmacy pharmacy)
            {
                _context.Pharmacies.Add(pharmacy);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetPharmacy), new { id = pharmacy.PharmacyId }, pharmacy);
            }

            // DELETE: api/Pharmacy/5
            [HttpDelete("{id}")]
            public async Task<IActionResult> DeletePharmacy(int id)
            {
                var pharmacy = await _context.Pharmacies.FindAsync(id);
                if (pharmacy == null)
                {
                    return NotFound();
                }

                _context.Pharmacies.Remove(pharmacy);
                await _context.SaveChangesAsync();

                return NoContent();
            }

            private bool PharmacyExists(int id)
            {
                return _context.Pharmacies.Any(e => e.PharmacyId == id);
            }
        }
 }
