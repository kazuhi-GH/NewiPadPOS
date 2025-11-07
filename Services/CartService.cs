using NewiPadPOS.Models;

namespace NewiPadPOS.Services
{
    public interface ICartService
    {
        Cart GetCart();
        void AddToCart(Product product, int quantity = 1);
        void RemoveFromCart(int productId);
        void UpdateQuantity(int productId, int quantity);
        void ClearCart();
        decimal GetTotal();
        int GetItemCount();
        event Action? OnCartChanged;
    }
    
    public class CartService : ICartService
    {
        private readonly Cart _cart = new Cart();
        
        public event Action? OnCartChanged;
        
        public Cart GetCart() => _cart;
        
        public void AddToCart(Product product, int quantity = 1)
        {
            _cart.AddItem(product, quantity);
            OnCartChanged?.Invoke();
        }
        
        public void RemoveFromCart(int productId)
        {
            _cart.RemoveItem(productId);
            OnCartChanged?.Invoke();
        }
        
        public void UpdateQuantity(int productId, int quantity)
        {
            _cart.UpdateQuantity(productId, quantity);
            OnCartChanged?.Invoke();
        }
        
        public void ClearCart()
        {
            _cart.Clear();
            OnCartChanged?.Invoke();
        }
        
        public decimal GetTotal()
        {
            return _cart.Total;
        }
        
        public int GetItemCount()
        {
            return _cart.ItemCount;
        }
    }
}