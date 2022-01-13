using System.Collections.Generic;
using ItServiceApp.Models.Payment;
using ItServiceApp.Services;
using ItServiceApp.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ItServiceApp.Controllers
{
    public class PaymentController : Controller
    {
        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }
        [Authorize]
        public IActionResult Index()
        {

            return View();
        }

        [Authorize]
        [HttpPost]
        public IActionResult CheckInstallment(string binNumber)
        {
            if (binNumber.Length != 6)
                return BadRequest(new
                {
                    Message = "Bad req."
                });

            var result = _paymentService.CheckInstallments(binNumber, 1000);
            return Ok(result);
        }

        [Authorize]
        [HttpPost]
        public IActionResult Index(PaymentViewModel model)
        {
            var paymentModel = new PaymentModel()
            {
                Installment = model.Installment,
                Address = new AddressModel(),
                BasketList = new List<BasketModel>() { model.BasketModel },
                Customer = new CustomerModel(),
                CardModel = model.CardModel,
                Price = 1000,
            };

            var result = _paymentService.Pay(paymentModel);
            return View();
        }
    }
}
