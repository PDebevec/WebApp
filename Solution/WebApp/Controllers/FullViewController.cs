using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI.Data;

namespace WebApp.Controllers
{
    public class FullViewController : Controller
    {
        private readonly WebAPIContext _context;

        public FullViewController(WebAPIContext context) { _context = context; }

        // GET: FullViewController
        public async Task<IActionResult> Index()
        {
            ViewBag.data = await _context.Department
                .Select(d => new
                {
                    htmlId = d.DepartmentName.Replace(" ", ""),
                    Department = d.DepartmentName,
                    Employees = _context.Employee
                        .Where(e => e.DepartmentId == d.DepartmentId)
                        .Select(e => new
                        {
                            htmlId = e.FirstName + e.LastName,
                            FirstName = e.FirstName,
                            LastName = e.LastName,
                            JobTitle = e.Job.JobTitle,
                            Manager = new
                            {
                                FirstName = e.Manager.FirstName,
                                LastName = e.Manager.LastName,
                                JobTitle = e.Manager.Job.JobTitle,
                            },
                            Location = new
                            {
                                Country = d.Location.Country.CountryName,
                                Province = d.Location.StateProvince,
                                City = d.Location.City,
                                PostalCode = d.Location.PostalCode,
                                Address = d.Location.StreetAddress
                            }
                        })
                        .ToList()
                })
                .ToListAsync();
            return View();
        }
    }
}
