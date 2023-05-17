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
    public class EmployeeController : Controller
    {
        private readonly WebAPIContext _context;

        public EmployeeController(WebAPIContext context)
        {
            _context = context;
        }

        // GET: Employee
        public async Task<IActionResult> Index()
        {
            var webAPIContext = _context.Employee.Include(e => e.Department).Include(e => e.Job).Include(e => e.Manager);
            return View(await webAPIContext.ToListAsync());
        }

        // GET: Employee/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Employee == null)
            {
                return NotFound();
            }

            var employee = await _context.Employee
                .Include(e => e.Department)
                .Include(e => e.Job)
                .Include(e => e.Manager)
                .FirstOrDefaultAsync(m => m.EmployeeId == id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // GET: Employee/Create
        public IActionResult Create()
        {
            //ViewData["DepartmentId"] = new SelectList(_context.Department, "DepartmentId", "DepartmentId");
            ViewBag.dropdownDepartment = new SelectList(_context.Department
                .Select(department => new { DepartmentId = department.DepartmentId, DisplayName = $"{department.DepartmentId}: {department.DepartmentName}"})
                .ToList(),
				"DepartmentId", "DisplayName"
				);
            //ViewData["JobId"] = new SelectList(_context.Job, "JobId", "JobId");
			ViewBag.dropdownJob= new SelectList(_context.Job
				.Select(job => new { JobId = job.JobId, DisplayName = $"{job.JobId}: {job.JobTitle}" })
				.ToList(),
				"JobId", "DisplayName"
				);
			//ViewData["ManagerId"] = new SelectList(_context.Employee, "EmployeeId", "EmployeeId");
			ViewBag.dropdownManager= new SelectList(_context.Employee
				.Select(manager => new { ManagerId = manager.EmployeeId, DisplayName = $"{manager.EmployeeId}: {manager.FirstName} {manager.LastName}, {manager.Job.JobTitle}" })
				.ToList(),
				"ManagerId", "DisplayName"
				);
			return View();
        }

        // POST: Employee/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EmployeeId,FirstName,LastName,Email,PhoneNumber,HireDate,JobId,Salary,ManagerId,DepartmentId")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                _context.Add(employee);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DepartmentId"] = new SelectList(_context.Department, "DepartmentId", "DepartmentId", employee.DepartmentId);
            ViewData["JobId"] = new SelectList(_context.Job, "JobId", "JobId", employee.JobId);
            ViewData["ManagerId"] = new SelectList(_context.Employee, "EmployeeId", "EmployeeId", employee.ManagerId);
            return View(employee);
        }

        // GET: Employee/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Employee == null)
            {
                return NotFound();
            }

            var employee = await _context.Employee.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }
			//ViewData["DepartmentId"] = new SelectList(_context.Department, "DepartmentId", "DepartmentId");
			ViewBag.dropdownDepartment = new SelectList(_context.Department
				.Select(department => new { DepartmentId = department.DepartmentId, DisplayName = $"{department.DepartmentId}: {department.DepartmentName}" })
				.ToList(),
				"DepartmentId", "DisplayName"
				);
			//ViewData["JobId"] = new SelectList(_context.Job, "JobId", "JobId");
			ViewBag.dropdownJob = new SelectList(_context.Job
				.Select(job => new { JobId = job.JobId, DisplayName = $"{job.JobId}: {job.JobTitle}" })
				.ToList(),
				"JobId", "DisplayName"
				);
			//ViewData["ManagerId"] = new SelectList(_context.Employee, "EmployeeId", "EmployeeId");
			ViewBag.dropdownManager = new SelectList(_context.Employee
				.Select(manager => new { ManagerId = manager.EmployeeId, DisplayName = $"{manager.EmployeeId}: {manager.FirstName} {manager.LastName}, {manager.Job.JobTitle}" })
				.ToList(),
				"ManagerId", "DisplayName"
				);
			return View(employee);
        }

        // POST: Employee/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("EmployeeId,FirstName,LastName,Email,PhoneNumber,HireDate,JobId,Salary,ManagerId,DepartmentId")] Employee employee)
        {
            if (id != employee.EmployeeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(employee);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmployeeExists(employee.EmployeeId))
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
            ViewData["DepartmentId"] = new SelectList(_context.Department, "DepartmentId", "DepartmentId", employee.DepartmentId);
            ViewData["JobId"] = new SelectList(_context.Job, "JobId", "JobId", employee.JobId);
            ViewData["ManagerId"] = new SelectList(_context.Employee, "EmployeeId", "EmployeeId", employee.ManagerId);
            return View(employee);
        }

        // GET: Employee/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Employee == null)
            {
                return NotFound();
            }

            var employee = await _context.Employee
                .Include(e => e.Department)
                .Include(e => e.Job)
                .Include(e => e.Manager)
                .FirstOrDefaultAsync(m => m.EmployeeId == id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // POST: Employee/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Employee == null)
            {
                return Problem("Entity set 'WebAPIContext.Employee'  is null.");
            }
            var employee = await _context.Employee.FindAsync(id);
            if (employee != null)
            {
                _context.Employee.Remove(employee);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EmployeeExists(int id)
        {
          return (_context.Employee?.Any(e => e.EmployeeId == id)).GetValueOrDefault();
        }
    }
}
