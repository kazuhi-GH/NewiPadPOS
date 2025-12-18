using Microsoft.EntityFrameworkCore;
using NewiPadPOS.Models;

namespace NewiPadPOS.Data
{
    public class PosDbContext : DbContext
    {
        public PosDbContext(DbContextOptions<PosDbContext> options) : base(options)
        {
        }
        
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            // Product configuration
            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Category).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Price).HasColumnType("decimal(18,2)");
                entity.Property(e => e.Description).HasMaxLength(500);
                entity.Property(e => e.ImageUrl).HasMaxLength(500);
                entity.HasIndex(e => e.Category);
                entity.HasIndex(e => e.IsActive);
            });
            
            // Order configuration
            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.OrderNumber).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Subtotal).HasColumnType("decimal(18,2)");
                entity.Property(e => e.Tax).HasColumnType("decimal(18,2)");
                entity.Property(e => e.Total).HasColumnType("decimal(18,2)");
                entity.Property(e => e.CustomerName).HasMaxLength(200);
                entity.Property(e => e.Notes).HasMaxLength(1000);
                entity.HasIndex(e => e.OrderNumber).IsUnique();
                entity.HasIndex(e => e.OrderDate);
                
                entity.HasMany(e => e.OrderItems)
                    .WithOne(e => e.Order)
                    .HasForeignKey(e => e.OrderId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
            
            // OrderItem configuration
            modelBuilder.Entity<OrderItem>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.UnitPrice).HasColumnType("decimal(18,2)");
                
                entity.HasOne(e => e.Product)
                    .WithMany()
                    .HasForeignKey(e => e.ProductId)
                    .OnDelete(DeleteBehavior.Restrict);
                    
                entity.HasOne(e => e.Order)
                    .WithMany(e => e.OrderItems)
                    .HasForeignKey(e => e.OrderId);
            });
            
            // Seed initial data
            SeedInitialData(modelBuilder);
        }
        
        private static void SeedInitialData(ModelBuilder modelBuilder)
        {
            var products = new[]
            {
                new Product { Id = 1, Name = "ブレンドコーヒー", Category = "ドリンク", Price = 350m, Description = "香り豊かなオリジナルブレンド", ImageUrl = "https://product.starbucks.co.jp/assets/img/item/4524785445048_1_640.png", StockQuantity = 100 },
                new Product { Id = 2, Name = "カフェラテ", Category = "ドリンク", Price = 420m, Description = "エスプレッソとスチームミルク", ImageUrl = "https://product.starbucks.co.jp/assets/img/item/4524785445512_1_640.png", StockQuantity = 80 },
                new Product { Id = 3, Name = "アメリカーノ", Category = "ドリンク", Price = 380m, Description = "エスプレッソをお湯で割った一杯", ImageUrl = "https://product.starbucks.co.jp/assets/img/item/4524785445437_1_640.png", StockQuantity = 90 },
                new Product { Id = 4, Name = "カプチーノ", Category = "ドリンク", Price = 450m, Description = "エスプレッソとフォームミルク", ImageUrl = "https://product.starbucks.co.jp/assets/img/item/4524785445611_1_640.png", StockQuantity = 70 },
                new Product { Id = 5, Name = "ウーロン茶", Category = "ドリンク", Price = 280m, Description = "さっぱりとした中国茶", ImageUrl = "https://product.starbucks.co.jp/assets/img/item/4524785447943_1_640.png", StockQuantity = 60 },
                
                new Product { Id = 6, Name = "クロワッサン", Category = "フード", Price = 320m, Description = "バターの風味豊かな焼きたて", ImageUrl = "https://product.starbucks.co.jp/assets/img/item/4560466460872_1_640.png", StockQuantity = 25 },
                new Product { Id = 7, Name = "サンドイッチ", Category = "フード", Price = 580m, Description = "ハム・レタス・トマトのサンドイッチ", ImageUrl = "https://product.starbucks.co.jp/assets/img/item/4560466460803_1_640.png", StockQuantity = 15 },
                new Product { Id = 8, Name = "サラダ", Category = "フード", Price = 650m, Description = "新鮮野菜のミックスサラダ", ImageUrl = "https://product.starbucks.co.jp/assets/img/item/4560466460773_1_640.png", StockQuantity = 20 },
                new Product { Id = 9, Name = "パスタ", Category = "フード", Price = 880m, Description = "トマトソースのペンネ", ImageUrl = "https://product.starbucks.co.jp/assets/img/item/4560466460780_1_640.png", StockQuantity = 12 },
                
                new Product { Id = 10, Name = "チーズケーキ", Category = "デザート", Price = 480m, Description = "濃厚なニューヨークスタイル", ImageUrl = "https://product.starbucks.co.jp/assets/img/item/4560466461404_1_640.png", StockQuantity = 8 },
                new Product { Id = 11, Name = "ティラミス", Category = "デザート", Price = 520m, Description = "マスカルポーネの本格ティラミス", ImageUrl = "https://product.starbucks.co.jp/assets/img/item/4560466461411_1_640.png", StockQuantity = 6 },
                new Product { Id = 12, Name = "クッキー", Category = "デザート", Price = 250m, Description = "サクサクのバタークッキー", ImageUrl = "https://product.starbucks.co.jp/assets/img/item/4560466460865_1_640.png", StockQuantity = 40 }
            };
            
            modelBuilder.Entity<Product>().HasData(products);
        }
    }
}