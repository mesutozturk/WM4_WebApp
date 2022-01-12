using ItServiceApp.Models.Payment;

namespace ItServiceApp.Services
{
    public interface IPaymentService
    {
        public void Pay(PaymentModel model);
    }
}
