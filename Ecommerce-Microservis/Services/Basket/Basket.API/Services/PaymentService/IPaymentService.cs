using Basket.API.DTOs.Payment;
using Basket.API.DTOs.Product;

namespace Basket.API.Services.PaymentService
{
    public interface IPaymentService
    {
        Task<PaymentResponse> ProcessPayment(PaymentRequest paymentRequest);
    }
}
