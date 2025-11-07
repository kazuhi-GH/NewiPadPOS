namespace NewiPadPOS.Models
{
    public class Cart
    {
        public List<CartItem> Items { get; set; } = new List<CartItem>();
        
        public decimal Subtotal => Items.Sum(item => item.TotalPrice);
        
        public decimal TaxRate { get; set; } = 0.1m; // 10%の消費税
        
        public decimal Tax => Math.Round(Subtotal * TaxRate, 0);
        
        public decimal Total => Subtotal + Tax;
        
        public int ItemCount => Items.Sum(item => item.Quantity);
        
        public void AddItem(Product product, int quantity = 1)
        {
            if (product == null) throw new ArgumentNullException(nameof(product));
            if (quantity <= 0) throw new ArgumentException("数量は1以上である必要があります", nameof(quantity));
            
            var existingItem = Items.FirstOrDefault(item => item.Product.Id == product.Id);
            
            if (existingItem != null)
            {
                existingItem.Quantity += quantity;
            }
            else
            {
                Items.Add(new CartItem
                {
                    Product = product,
                    Quantity = quantity,
                    UnitPrice = product.Price
                });
            }
        }
        
        public void RemoveItem(int productId)
        {
            var item = Items.FirstOrDefault(item => item.Product.Id == productId);
            if (item != null)
            {
                Items.Remove(item);
            }
        }
        
        public void UpdateQuantity(int productId, int quantity)
        {
            if (quantity < 0) throw new ArgumentException("数量は0以上である必要があります", nameof(quantity));
            
            var item = Items.FirstOrDefault(item => item.Product.Id == productId);
            if (item != null)
            {
                if (quantity == 0)
                {
                    Items.Remove(item);
                }
                else
                {
                    item.Quantity = quantity;
                }
            }
        }
        
        public void Clear()
        {
            Items.Clear();
        }
        
        public bool IsEmpty => !Items.Any();
    }
    
    public class CartItem
    {
        public Product Product { get; set; } = null!;
        
        public int Quantity { get; set; }
        
        public decimal UnitPrice { get; set; }
        
        public decimal TotalPrice => Quantity * UnitPrice;
    }
}