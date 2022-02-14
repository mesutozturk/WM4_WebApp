using System.Collections.Generic;
using ItServiceApp.Core.Payment;

namespace ItServiceApp.Business.Services.Payment
{
    public interface IPaymentService
    {
        public InstallmentModel CheckInstallments(string binNumber, decimal price);
        public PaymentResponseModel Pay(PaymentModel model);
    }
}