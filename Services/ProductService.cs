using Microsoft.EntityFrameworkCore;
using NewiPadPOS.Data;
using NewiPadPOS.Models;

namespace NewiPadPOS.Services
{
    public interface IProductService
    {
        Task<List<Product>> GetAllProductsAsync();
        Task<List<Product>> GetProductsByCategoryAsync(string category);
        Task<Product?> GetProductByIdAsync(int id);
        Task<List<string>> GetCategoriesAsync();
        Task<bool> UpdateStockAsync(int productId, int newStock);
        Task<bool> IsProductAvailableAsync(int productId, int requestedQuantity);
    }
    
    public class ProductService : IProductService
    {
        private readonly PosDbContext _context;
        
        public ProductService(PosDbContext context)
        {
            _context = context;
        }
        
        public async Task<List<Product>> GetAllProductsAsync()
        {
            return await _context.Products
                .Where(p => p.IsActive)
                .OrderBy(p => p.Category)
                .ThenBy(p => p.Name)
                .ToListAsync();
        }
        
        public async Task<List<Product>> GetProductsByCategoryAsync(string category)
        {
            return await _context.Products
                .Where(p => p.IsActive && p.Category == category)
                .OrderBy(p => p.Name)
                .ToListAsync();
        }
        
        public async Task<Product?> GetProductByIdAsync(int id)
        {
            return await _context.Products
                .FirstOrDefaultAsync(p => p.Id == id && p.IsActive);
        }
        
        public async Task<List<string>> GetCategoriesAsync()
        {
            return await _context.Products
                .Where(p => p.IsActive)
                .Select(p => p.Category)
                .Distinct()
                .OrderBy(c => c)
                .ToListAsync();
        }
        
        public async Task<bool> UpdateStockAsync(int productId, int newStock)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null) return false;
            
            product.StockQuantity = newStock;
            product.UpdatedAt = DateTime.Now;
            
            await _context.SaveChangesAsync();
            return true;
        }
        
        public async Task<bool> IsProductAvailableAsync(int productId, int requestedQuantity)
        {
            var product = await _context.Products.FindAsync(productId);
            return product != null && product.IsActive && product.StockQuantity >= requestedQuantity;
        }
    }
}