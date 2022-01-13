using System.Collections.Generic;
using ItServiceApp.Models.Payment;

namespace ItServiceApp.Services
{
    public interface IPaymentService
    {
        public InstallmentModel CheckInstallments(string binNumber, decimal price);
        public PaymentResponseModel Pay(PaymentModel model);
    }
}