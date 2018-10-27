using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Utilities.Models;

namespace Utilities.Controllers
{
    public class TenantsController : Controller
    {
        private UtilitiesContext context;

        public TenantsController(UtilitiesContext context)
        {
            this.context = context;
        }
        public IActionResult Index()
        {
            var tenants = context.Tenants.ToList();
            return View(tenants);
        }
    }
}