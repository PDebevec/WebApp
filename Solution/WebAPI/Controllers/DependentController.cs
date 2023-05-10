using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI;
using WebAPI.Data;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DependentController : ControllerBase
    {
        private readonly WebAPIContext _context;

        public DependentController(WebAPIContext context)
        {
            _context = context;
        }

        // GET: api/Dependent
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Dependent>>> GetDependent()
        {
          if (_context.Dependent == null)
          {
              return NotFound();
          }
            return await _context.Dependent.ToListAsync();
        }

        // GET: api/Dependent/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Dependent>> GetDependent(int id)
        {
          if (_context.Dependent == null)
          {
              return NotFound();
          }
            var dependent = await _context.Dependent.FindAsync(id);

            if (dependent == null)
            {
                return NotFound();
            }

            return dependent;
        }

        // PUT: api/Dependent/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDependent(int id, Dependent dependent)
        {
            if (id != dependent.DependentId)
            {
                return BadRequest();
            }

            _context.Entry(dependent).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DependentExists(id))
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

        // POST: api/Dependent
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Dependent>> PostDependent(Dependent dependent)
        {
          if (_context.Dependent == null)
          {
              return Problem("Entity set 'WebAPIContext.Dependent'  is null.");
          }
            _context.Dependent.Add(dependent);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDependent", new { id = dependent.DependentId }, dependent);
        }

        // DELETE: api/Dependent/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDependent(int id)
        {
            if (_context.Dependent == null)
            {
                return NotFound();
            }
            var dependent = await _context.Dependent.FindAsync(id);
            if (dependent == null)
            {
                return NotFound();
            }

            _context.Dependent.Remove(dependent);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DependentExists(int id)
        {
            return (_context.Dependent?.Any(e => e.DependentId == id)).GetValueOrDefault();
        }
    }
}
