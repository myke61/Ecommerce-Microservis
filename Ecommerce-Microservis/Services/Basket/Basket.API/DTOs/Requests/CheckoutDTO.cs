using Basket.API.Entities;

namespace Basket.API.DTOs.Requests
{
    public class CheckoutDTO
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Email { get; set; }
        public required string PhoneNumber { get; set; }
        public required string Address { get; set; }

        public required string CardNumber { get; set; }
        public required string CardHolderName { get; set; }
        public required string ExpirationDate { get; set; }
        public required string CVC { get; set; }
    }
}
