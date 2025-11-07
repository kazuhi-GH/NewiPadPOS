using System.ComponentModel.DataAnnotations;

namespace NewiPadPOS.Models
{
    public class Product
    {
        public int Id { get; set; }
        
        [Required(ErrorMessage = "商品名は必須です")]
        [StringLength(200, ErrorMessage = "商品名は200文字以内で入力してください")]
        public string Name { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "カテゴリは必須です")]
        [StringLength(100, ErrorMessage = "カテゴリは100文字以内で入力してください")]
        public string Category { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "価格は必須です")]
        [Range(0, 999999, ErrorMessage = "価格は0円以上999,999円以下で入力してください")]
        public decimal Price { get; set; }
        
        [StringLength(500, ErrorMessage = "説明は500文字以内で入力してください")]
        public string? Description { get; set; }
        
        public string? ImageUrl { get; set; }
        
        public bool IsActive { get; set; } = true;
        
        [Range(0, int.MaxValue, ErrorMessage = "在庫数は0以上で入力してください")]
        public int StockQuantity { get; set; } = 0;
        
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
    }
}