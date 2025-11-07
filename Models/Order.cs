using System.ComponentModel.DataAnnotations;

namespace NewiPadPOS.Models
{
    public class Order
    {
        public int Id { get; set; }
        
        [Required]
        public string OrderNumber { get; set; } = string.Empty;
        
        public DateTime OrderDate { get; set; } = DateTime.Now;
        
        public List<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
        
        public decimal Subtotal { get; set; }
        
        public decimal Tax { get; set; }
        
        public decimal Total { get; set; }
        
        public OrderStatus Status { get; set; } = OrderStatus.Pending;
        
        public PaymentMethod PaymentMethod { get; set; }
        
        public string? CustomerName { get; set; }
        
        public string? Notes { get; set; }
    }
    
    public class OrderItem
    {
        public int Id { get; set; }
        
        public int OrderId { get; set; }
        
        public Order Order { get; set; } = null!;
        
        public int ProductId { get; set; }
        
        public Product Product { get; set; } = null!;
        
        public int Quantity { get; set; }
        
        public decimal UnitPrice { get; set; }
        
        public decimal TotalPrice => Quantity * UnitPrice;
    }
    
    public enum OrderStatus
    {
        Pending = 1,
        Processing = 2,
        Completed = 3,
        Cancelled = 4
    }
    
    public enum PaymentMethod
    {
        Cash = 1,
        CreditCard = 2,
        DebitCard = 3,
        DigitalPayment = 4,
        Other = 5
    }
}