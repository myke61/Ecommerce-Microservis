namespace Basket.API.Entities
{
    public class Basket
    {
        public decimal BasketAmount { get; set; }

        public IList<BasketItem> BasketItems { get; private set; } = new List<BasketItem>();

        public void AddItem(Guid productId, int quantity, decimal unitPrice, string name)
        {
            var existingItem = BasketItems.FirstOrDefault(x => x.ProductId == productId);
            if (existingItem != null)
            {
                existingItem.IncreaseQuantity();
                this.BasketAmount = this.BasketAmount + quantity * unitPrice;
            }
            else
            {
                BasketItems.Add(new BasketItem(productId, 1, unitPrice, name));
                this.BasketAmount = this.BasketAmount + quantity * unitPrice;

            }
        }

        public void RemoveItem(Guid productId)
        {
            var existingItem = BasketItems.FirstOrDefault(x => x.ProductId == productId);
            if (existingItem != null)
            {
                existingItem.DecreaseQuantity();
                this.BasketAmount = this.BasketAmount - existingItem.UnitPrice;
            }
        }
    }
}
