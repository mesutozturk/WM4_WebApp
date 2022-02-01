using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using AutoMapper;
using ItServiceApp.Data;
using ItServiceApp.Extensions;
using ItServiceApp.Models.Payment;
using ItServiceApp.Services;
using ItServiceApp.ViewModels;
using Iyzipay.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ItServiceApp.Controllers
{
    public class PaymentController : Controller
    {
        private readonly IPaymentService _paymentService;
        private readonly MyContext _dbContext;
        private readonly IMapper _mapper;

        public PaymentController(IPaymentService paymentService, MyContext dbContext, IMapper mapper)
        {
            _paymentService = paymentService;
            _dbContext = dbContext;
            _mapper = mapper;
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
                BasketList = new List<BasketModel>(),
                Customer = new CustomerModel(),
                CardModel = model.CardModel,
                Price = 1000,
                UserId = HttpContext.GetUserId(),
                Ip = Request.HttpContext.Connection.RemoteIpAddress?.ToString()
            };

            var installmentInfo = _paymentService.CheckInstallments(paymentModel.CardModel.CardNumber.Substring(0, 6), paymentModel.Price);

            var installmentNumber =
                installmentInfo.InstallmentPrices.FirstOrDefault(x => x.InstallmentNumber == model.Installment);

            paymentModel.PaidPrice = decimal.Parse(installmentNumber != null ? installmentNumber.TotalPrice.Replace('.', ',') : installmentInfo.InstallmentPrices[0].TotalPrice.Replace('.', ','));

            //legacy code

            var result = _paymentService.Pay(paymentModel);
            return View();
        }

        [Authorize]
        public IActionResult Purchase(Guid id)
        {
            var data = _dbContext.SubscriptionTypes.Find(id);
            if (data == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var model = _mapper.Map<SubscriptionTypeViewModel>(data);

            ViewBag.Subs = model;

            //var model = new PaymentViewModel()
            //{
            //    BasketModel = new BasketModel()
            //    {
            //        Category1 = data.Name,
            //        ItemType = BasketItemType.VIRTUAL.ToString(),
            //        Id = data.Id.ToString(),
            //        Name = data.Name,
            //        Price = data.Price.ToString(new CultureInfo("en-us"))
            //    }
            //};

            return View();
        }
    }
}
