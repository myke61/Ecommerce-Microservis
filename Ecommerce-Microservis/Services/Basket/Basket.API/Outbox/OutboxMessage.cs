using Ecommerce.Base.Entities;

namespace Basket.API.Outbox
{
    public class OutboxMessage : BaseEntity
    {
        public string Type { get; set; } = string.Empty;
        public string Payload { get; set; } = string.Empty;
        public DateTime OccurredOn { get; set; } = DateTime.UtcNow;
        public bool Processed { get; set; } = false;
    }
}
