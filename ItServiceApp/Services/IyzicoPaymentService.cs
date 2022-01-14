using System;
using System.Collections.Generic;
using System.Globalization;
using AutoMapper;
using ItServiceApp.Models;
using ItServiceApp.Models.Identity;
using ItServiceApp.Models.Payment;
using Iyzipay.Model;
using Iyzipay.Request;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using MUsefulMethods;

namespace ItServiceApp.Services
{
    public class IyzicoPaymentService : IPaymentService
    {
        private readonly IConfiguration _configuration;
        private readonly IyzicoPaymentOptions _options;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;
        public IyzicoPaymentService(IConfiguration configuration, IMapper mapper, UserManager<ApplicationUser> userManager)
        {
            _configuration = configuration;
            _mapper = mapper;
            _userManager = userManager;
            var section = _configuration.GetSection(IyzicoPaymentOptions.Key);
            _options = new IyzicoPaymentOptions()
            {
                ApiKey = section["ApiKey"],
                SecretKey = section["SecretKey"],
                BaseUrl = section["BaseUrl"],
                ThreedsCallbackUrl = section["ThreedsCallbackUrl"],
            };
        }

        private string GenerateConversationId()
        {
            return StringHelpers.GenerateUniqueCode();
        }

        private CreatePaymentRequest InitialPaymentRequest(PaymentModel model)
        {
            var paymentRequest = new CreatePaymentRequest();

            paymentRequest.Installment = model.Installment;
            paymentRequest.Locale = Locale.TR.ToString();
            paymentRequest.ConversationId = GenerateConversationId();
            paymentRequest.Price = model.Price.ToString(new CultureInfo("en-US"));
            paymentRequest.PaidPrice = model.PaidPrice.ToString(new CultureInfo("en-US"));
            paymentRequest.Currency = Currency.TRY.ToString();
            paymentRequest.BasketId = StringHelpers.GenerateUniqueCode();
            paymentRequest.PaymentChannel = PaymentChannel.WEB.ToString();
            paymentRequest.PaymentGroup = PaymentGroup.SUBSCRIPTION.ToString();

            var buyer = new Buyer
            {
                Id = "BY789",
                Name = "John",
                Surname = "Doe",
                GsmNumber = "+905350000000",
                Email = "email@email.com",
                IdentityNumber = "74300864791",
                LastLoginDate = "2015-10-05 12:43:35",
                RegistrationDate = "2013-04-21 15:12:09",
                RegistrationAddress = "Nidakule Göztepe, Merdivenköy Mah. Bora Sok. No:1",
                Ip = "85.34.78.112",
                City = "Istanbul",
                Country = "Turkey",
                ZipCode = "34732"
            };
            paymentRequest.Buyer = buyer;

            return paymentRequest;
        }

        public InstallmentModel CheckInstallments(string binNumber, decimal price)
        {
            var conversationId = GenerateConversationId();
            var request = new RetrieveInstallmentInfoRequest
            {
                Locale = Locale.TR.ToString(),
                ConversationId = conversationId,
                BinNumber = binNumber,
                Price = price.ToString(new CultureInfo("en-US")),
            };

            var result = InstallmentInfo.Retrieve(request, _options);
            if (result.Status == "failure")
            {
                throw new Exception(result.ErrorMessage);
            }

            if (result.ConversationId != conversationId)
            {
                throw new Exception("Hatalı istek oluturuldu");
            }

            var resultModel = _mapper.Map<InstallmentModel>(result.InstallmentDetails[0]);

            return resultModel;
        }

        public PaymentResponseModel Pay(PaymentModel model)
        {
            var request = this.InitialPaymentRequest(model);
            var payment = Payment.Create(request, _options);
            return null;
        }
    }
}