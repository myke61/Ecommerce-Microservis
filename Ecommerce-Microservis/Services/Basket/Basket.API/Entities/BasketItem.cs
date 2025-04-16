namespace Basket.API.Entities
{
    public class BasketItem
    {
        public BasketItem(Guid productId, int quantity, decimal unitPrice, string productName)
        {
            this.ProductId = productId;
            this.Quantity = quantity;
            this.UnitPrice = unitPrice;
            this.ProductName = productName;
        }

        public Guid ProductId { get; private set; }

        public int Quantity { get; private set; }

        public decimal UnitPrice { get; private set; }

        public string ProductName { get; private set; }

        public void IncreaseQuantity()
        {
            this.Quantity = this.Quantity + 1;
        }

        public void DecreaseQuantity()
        {
            this.Quantity = this.Quantity - 1;
        }
    }
}
