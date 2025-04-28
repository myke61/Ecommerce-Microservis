using FakePayment.Model;

namespace FakePayment.Services
{
    public class PaymentService : IPaymentService
    {
        public Task<PaymentResponse> ProcessPaymentAsync(PaymentRequest request)
        {
            PaymentResponse? response;
            if (request.Amount > 120000)
            {
                response = new PaymentResponse
                {
                    IsSuccess = false,
                    Message = "Payment Failed"
                };
            }
            else
            {
                response = new PaymentResponse
                {
                    IsSuccess = true,
                    Message = "Payment Success"
                };
            }

            return Task.FromResult(response);
        }
    }
}
