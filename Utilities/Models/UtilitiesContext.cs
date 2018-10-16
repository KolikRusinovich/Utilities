using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Utilties.Models
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
