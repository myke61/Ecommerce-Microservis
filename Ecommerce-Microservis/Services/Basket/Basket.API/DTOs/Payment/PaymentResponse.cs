namespace Basket.API.DTOs.Payment
{
    public class PaymentResponse
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}
