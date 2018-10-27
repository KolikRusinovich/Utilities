using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Utilities.Models
{
    public class UtilitiesContext : DbContext
    {
        public UtilitiesContext(DbContextOptions<UtilitiesContext> options) : base(options)
        {

        }
        public DbSet<Tenant> Tenants { get; set; }
        public DbSet<Rate> Rates { get; set; }
        public DbSet<Reading> Readings { get; set; }
        public DbSet<Payment> Payments { get; set; }
    }
}
