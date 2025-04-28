using Basket.API.DTOs.Payment;
using Newtonsoft.Json;
using System.Text;

namespace Basket.API.Services.PaymentService
{
    public class PaymentService : IPaymentService
    {
        private readonly HttpClient client;
        public PaymentService(HttpClient client)
        {
            this.client = client;
        }
        public async Task<PaymentResponse> ProcessPayment(PaymentRequest paymentRequest)
        {
            var jsonContent = JsonConvert.SerializeObject(paymentRequest);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            var response = await client.PostAsync("/api/payment/", content);
            var jsonResponse = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<PaymentResponse>(jsonResponse);
        }
    }
}
