﻿using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebAPI.Data;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly WebAPIContext _context;

        public HomeController(ILogger<HomeController> logger, WebAPIContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            //število zaposlenih
            ViewBag.NofEmployees = _context.Employee.Count();

            //število zaposlenih v vsakem oddelku
            ViewBag.NofEinDepartment = _context.Employee
                .GroupBy(e => e.DepartmentId)
                .Select(g => new { DepartmentId = g.Key, EmployeeCount = g.Count() })
                .ToList()
                .Join(
                    _context.Department.ToList(),
                    e => e.DepartmentId,
                    d => d.DepartmentId,
                    (e, d) => new { DepartmentName = d.DepartmentName, EmployeeCount = e.EmployeeCount })
                .ToList();
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}