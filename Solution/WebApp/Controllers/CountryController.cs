using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebAPI;
using WebAPI.Data;

namespace WebApp.Controllers
{
    public class CountryController : Controller
    {
        private readonly WebAPIContext _context;

        public CountryController(WebAPIContext context)
        {
            _context = context;
        }

        // GET: Country
        public async Task<IActionResult> Index()
        {
            var webAPIContext = _context.Country!.Include(c => c.Region);
            return View(await webAPIContext.ToListAsync());
        }

        // GET: Country/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.Country == null)
            {
                return NotFound();
            }

            var country = await _context.Country
                .Include(c => c.Region)
                .FirstOrDefaultAsync(m => m.CountryId == id);
            if (country == null)
            {
                return NotFound();
            }

            return View(country);
        }

        // GET: Country/Create
        public IActionResult Create()
        {
            //ViewData["RegionId"] = new SelectList(_context.Region, "RegionId", "RegionId");
            ViewBag.dropdown = new SelectList(_context.Region
                .Select(region => new { RegionId = region.RegionId, DropDown = $"{region.RegionId}: {region.RegionName}"})
                .ToList(), "RegionId", "DropDown");
            return View();
        }

        // POST: Country/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CountryId,CountryName,RegionId")] Country country)
        {
            if (ModelState.IsValid)
            {
                _context.Add(country);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["RegionId"] = new SelectList(_context.Region, "RegionId", "RegionId", country.RegionId);
            return View(country);
        }

        // GET: Country/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.Country == null)
            {
                return NotFound();
            }

            var country = await _context.Country.FindAsync(id);
            if (country == null)
            {
                return NotFound();
            }
            //ViewData["RegionId"] = new SelectList(_context.Region, "RegionId", "RegionId");
            ViewBag.dropdown = new SelectList(_context.Region
                .Select(region => new { RegionId = region.RegionId, DropDown = $"{region.RegionId}: {region.RegionName}" })
                .ToList(), "RegionId", "DropDown");
            return View(country);
        }

        // POST: Country/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("CountryId,CountryName,RegionId")] Country country)
        {
            if (id != country.CountryId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(country);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CountryExists(country.CountryId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["RegionId"] = new SelectList(_context.Region, "RegionId", "RegionId", country.RegionId);
            return View(country);
        }

        // GET: Country/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.Country == null)
            {
                return NotFound();
            }

            var country = await _context.Country
                .Include(c => c.Region)
                .FirstOrDefaultAsync(m => m.CountryId == id);
            if (country == null)
            {
                return NotFound();
            }

			await DeleteConfirmed(id);

			return RedirectToAction(nameof(Index));
		}

        // POST: Country/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.Country == null)
            {
                return Problem("Entity set 'WebAPIContext.Country'  is null.");
            }
            var country = await _context.Country.FindAsync(id);
            if (country != null)
            {
                _context.Country.Remove(country);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CountryExists(string id)
        {
          return (_context.Country?.Any(e => e.CountryId == id)).GetValueOrDefault();
        }
    }
}
