using Microsoft.EntityFrameworkCore;
using NewiPadPOS.Data;
using NewiPadPOS.Models;

namespace NewiPadPOS.Services
{
    public interface IOrderService
    {
        Task<Order> CreateOrderAsync(Cart cart, string? customerName = null, PaymentMethod paymentMethod = PaymentMethod.Cash, string? notes = null);
        Task<List<Order>> GetOrdersAsync(DateTime? fromDate = null, DateTime? toDate = null);
        Task<Order?> GetOrderByIdAsync(int id);
        Task<Order?> GetOrderByOrderNumberAsync(string orderNumber);
        Task<bool> UpdateOrderStatusAsync(int orderId, OrderStatus status);
        Task<decimal> GetTodaysSalesAsync();
        Task<int> GetTodaysOrderCountAsync();
    }
    
    public class OrderService : IOrderService
    {
        private readonly PosDbContext _context;
        
        public OrderService(PosDbContext context)
        {
            _context = context;
        }
        
        public async Task<Order> CreateOrderAsync(Cart cart, string? customerName = null, PaymentMethod paymentMethod = PaymentMethod.Cash, string? notes = null)
        {
            if (cart.IsEmpty)
                throw new InvalidOperationException("カートが空です");
            
            var order = new Order
            {
                OrderNumber = await GenerateOrderNumberAsync(),
                CustomerName = customerName,
                PaymentMethod = paymentMethod,
                Notes = notes,
                Subtotal = cart.Subtotal,
                Tax = cart.Tax,
                Total = cart.Total,
                OrderDate = DateTime.Now,
                Status = OrderStatus.Pending
            };
            
            foreach (var cartItem in cart.Items)
            {
                order.OrderItems.Add(new OrderItem
                {
                    ProductId = cartItem.Product.Id,
                    Quantity = cartItem.Quantity,
                    UnitPrice = cartItem.UnitPrice
                });
                
                // 在庫を減らす
                var product = await _context.Products.FindAsync(cartItem.Product.Id);
                if (product != null && product.StockQuantity >= cartItem.Quantity)
                {
                    product.StockQuantity -= cartItem.Quantity;
                    product.UpdatedAt = DateTime.Now;
                }
            }
            
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            
            return order;
        }
        
        public async Task<List<Order>> GetOrdersAsync(DateTime? fromDate = null, DateTime? toDate = null)
        {
            var query = _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .AsQueryable();
            
            if (fromDate.HasValue)
                query = query.Where(o => o.OrderDate >= fromDate.Value);
                
            if (toDate.HasValue)
                query = query.Where(o => o.OrderDate <= toDate.Value);
            
            return await query
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();
        }
        
        public async Task<Order?> GetOrderByIdAsync(int id)
        {
            return await _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .FirstOrDefaultAsync(o => o.Id == id);
        }
        
        public async Task<Order?> GetOrderByOrderNumberAsync(string orderNumber)
        {
            return await _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .FirstOrDefaultAsync(o => o.OrderNumber == orderNumber);
        }
        
        public async Task<bool> UpdateOrderStatusAsync(int orderId, OrderStatus status)
        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order == null) return false;
            
            order.Status = status;
            await _context.SaveChangesAsync();
            return true;
        }
        
        public async Task<decimal> GetTodaysSalesAsync()
        {
            var today = DateTime.Today;
            var tomorrow = today.AddDays(1);
            
            return await _context.Orders
                .Where(o => o.OrderDate >= today && o.OrderDate < tomorrow && o.Status == OrderStatus.Completed)
                .SumAsync(o => o.Total);
        }
        
        public async Task<int> GetTodaysOrderCountAsync()
        {
            var today = DateTime.Today;
            var tomorrow = today.AddDays(1);
            
            return await _context.Orders
                .CountAsync(o => o.OrderDate >= today && o.OrderDate < tomorrow);
        }
        
        private async Task<string> GenerateOrderNumberAsync()
        {
            var today = DateTime.Today;
            var tomorrow = today.AddDays(1);
            
            var todayOrderCount = await _context.Orders
                .CountAsync(o => o.OrderDate >= today && o.OrderDate < tomorrow);
            
            return $"POS{DateTime.Now:yyyyMMdd}{(todayOrderCount + 1):D4}";
        }
    }
}