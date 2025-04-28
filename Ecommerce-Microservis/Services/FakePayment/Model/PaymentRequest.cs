namespace FakePayment.Model
{
    public class PaymentRequest
    {
        public Guid UserId { get; set; }
        public required string CardNumber { get; set; }
        public required string CardHolder { get; set; }
        public required string ExpirationDate { get; set; }
        public required string CVV { get; set; }
        public required decimal Amount { get; set; }
    }
}
