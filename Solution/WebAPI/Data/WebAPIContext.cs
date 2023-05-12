using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebAPI;

namespace WebAPI.Data
{
    public class WebAPIContext : DbContext
    {
        public WebAPIContext (DbContextOptions<WebAPIContext> options)
            : base(options)
        {
        }

        public DbSet<WebAPI.Region> Region { get; set; } = default!;

        public DbSet<WebAPI.Country>? Country { get; set; }

        public DbSet<WebAPI.Location>? Location { get; set; }

        public DbSet<WebAPI.Job>? Job { get; set; }

        public DbSet<WebAPI.Department>? Department { get; set; }

        public DbSet<WebAPI.Employee>? Employee { get; set; }

        //public DbSet<WebAPI.Dependent>? Dependent { get; set; }
    }
}
