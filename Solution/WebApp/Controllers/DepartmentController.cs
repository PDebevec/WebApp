using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebAPI;
using WebAPI.Data;

namespace WebApp.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly WebAPIContext _context;

        public DepartmentController(WebAPIContext context)
        {
            _context = context;
        }

        // GET: Department
        public async Task<IActionResult> Index()
        {
            var webAPIContext = _context.Department.Include(d => d.Location);
            return View(await webAPIContext.ToListAsync());
        }

        // GET: Department/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Department == null)
            {
                return NotFound();
            }

            var department = await _context.Department
                .Include(d => d.Location)
                .FirstOrDefaultAsync(m => m.DepartmentId == id);
            if (department == null)
            {
                return NotFound();
            }

            return View(department);
        }

        // GET: Department/Create
        public IActionResult Create()
        {
            //ViewData["LocationId"] = new SelectList(_context.Location, "LocationId", "LocationId");
            var data = _context.Location
                .Select(location => new { LocationId = location.LocationId, DisplayName = $"{location.CountryId}: {location.StateProvince} {location.StreetAddress}" })
                .ToList();
            ViewBag.dropdown = new SelectList(data, "LocationId", "DisplayName");
            return View();
        }

        // POST: Department/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DepartmentId,DepartmentName,LocationId")] Department department)
        {
            if (ModelState.IsValid)
            {
                _context.Add(department);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["LocationId"] = new SelectList(_context.Location, "LocationId", "LocationId", department.LocationId);
            return View(department);
        }

        // GET: Department/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Department == null)
            {
                return NotFound();
            }

            var department = await _context.Department.FindAsync(id);
            if (department == null)
            {
                return NotFound();
            }
            //ViewData["LocationId"] = new SelectList(_context.Location, "LocationId", "LocationId", department.LocationId);
            var data = _context.Location
                .Select(location => new { LocationId = location.LocationId, DisplayName = $"{location.LocationId}, {location.City}, {location.StreetAddress}" })
                .ToList();
            ViewBag.dropdown = new SelectList(data, "LocationId", "DisplayName");
            return View(department);
        }

        // POST: Department/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("DepartmentId,DepartmentName,LocationId")] Department department)
        {
            if (id != department.DepartmentId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(department);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DepartmentExists(department.DepartmentId))
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
            ViewData["LocationId"] = new SelectList(_context.Location, "LocationId", "LocationId", department.LocationId);
            return View(department);
        }

		// GET: Department/Delete/5
		public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Department == null)
            {
                return NotFound();
            }

            var department = await _context.Department
                .Include(d => d.Location)
                .FirstOrDefaultAsync(m => m.DepartmentId == id);
            if (department == null)
            {
                return NotFound();
            }

            await DeleteConfirmed(id.Value);

			return RedirectToAction(nameof(Index));
		}

        // POST: Department/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Department == null)
            {
                return Problem("Entity set 'WebAPIContext.Department'  is null.");
            }
            var department = await _context.Department.FindAsync(id);
            if (department != null)
            {
                _context.Department.Remove(department);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DepartmentExists(int id)
        {
          return (_context.Department?.Any(e => e.DepartmentId == id)).GetValueOrDefault();
        }
    }
}
