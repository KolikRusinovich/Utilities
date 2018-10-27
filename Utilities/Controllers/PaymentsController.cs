using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Utilities.Models;
using Utilities.ViewModels;

namespace Utilities.Controllers
{
    public class PaymentsController : Controller
    {
        private readonly UtilitiesContext context;
        private PaymentViewModel _payment = new PaymentViewModel
        {
            Type = "",
            Surname = ""
        };

        public PaymentsController(UtilitiesContext context)
        {
            this.context = context;
        }
        public IActionResult Index()
        {
            var paymentContext = context.Payments.Include(p => p.Tenant).Include(p => p.Rate);
            PaymentsViewModel payments = new PaymentsViewModel
            {
                Payments = paymentContext.Take(10).ToList(),
                PaymentViewModel = _payment
            };
            return View(payments);
        }
    }
}