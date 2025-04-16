namespace Basket.API.Entities
{
    public class Checkout
    {
        public Guid UserId { get; set; }
        public Decimal TotalPrice { get; set; }
        public required BasketItem[] BasketItems { get; set; }
        public required CheckoutUserInformation UserInformation { get; set; }
        public required CheckoutCardInformation CardInformation { get; set; }
    }

    public class CheckoutUserInformation
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Email { get; set; }
        public required string PhoneNumber { get; set; }
        public required string Address { get; set; }
    }

    public class CheckoutCardInformation
    {
        public required string CardNumber { get; set; }
        public required string CardHolderName { get; set; }
        public required string ExpirationDate { get; set; }
        public required string CVC { get; set; }
    }
}
