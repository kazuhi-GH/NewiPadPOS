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
                new Product { Id = 1, Name = "ドリップコーヒー", Category = "ドリンク", Price = 350m, Description = "香り豊かなオリジナルブレンド", StockQuantity = 100, ImageUrl = "https://images.unsplash.com/photo-1509042239860-f550ce710b93?w=400" },
                new Product { Id = 2, Name = "カフェラテ", Category = "ドリンク", Price = 420m, Description = "エスプレッソとスチームミルク", StockQuantity = 80, ImageUrl = "https://images.unsplash.com/photo-1561882468-9110e03e0f78?w=400" },
                new Product { Id = 3, Name = "カフェアメリカーノ", Category = "ドリンク", Price = 380m, Description = "エスプレッソをお湯で割った一杯", StockQuantity = 90, ImageUrl = "https://images.unsplash.com/photo-1514432324607-a09d9b4aefdd?w=400" },
                new Product { Id = 4, Name = "カプチーノ", Category = "ドリンク", Price = 450m, Description = "エスプレッソとフォームミルク", StockQuantity = 70, ImageUrl = "https://images.unsplash.com/photo-1572442388796-11668a67e53d?w=400" },
                new Product { Id = 5, Name = "キャラメルマキアート", Category = "ドリンク", Price = 480m, Description = "バニラシロップとキャラメルソース", StockQuantity = 60, ImageUrl = "https://images.unsplash.com/photo-1599639957043-f3aa5c986398?w=400" },
                
                new Product { Id = 6, Name = "クロワッサン", Category = "フード", Price = 320m, Description = "バターの風味豊かな焼きたて", StockQuantity = 25, ImageUrl = "https://images.unsplash.com/photo-1555507036-ab1f4038808a?w=400" },
                new Product { Id = 7, Name = "ハム&マリボーチーズ 石窯フィローネ", Category = "フード", Price = 580m, Description = "パルミジャーノレジャーノ入りソース", StockQuantity = 15, ImageUrl = "https://images.unsplash.com/photo-1509722747041-616f39b57569?w=400" },
                new Product { Id = 8, Name = "サラダラップ", Category = "フード", Price = 520m, Description = "新鮮野菜のサラダラップ", StockQuantity = 20, ImageUrl = "https://images.unsplash.com/photo-1626700051175-6818013e1d4f?w=400" },
                new Product { Id = 9, Name = "アメリカンワッフル", Category = "フード", Price = 340m, Description = "シロップがしみこんだワッフル", StockQuantity = 12, ImageUrl = "https://images.unsplash.com/photo-1568051243851-f9b136146e97?w=400" },
                
                new Product { Id = 10, Name = "ニューヨークチーズケーキ", Category = "デザート", Price = 480m, Description = "濃厚なニューヨークスタイル", StockQuantity = 8, ImageUrl = "https://images.unsplash.com/photo-1524351199678-941a58a3df50?w=400" },
                new Product { Id = 11, Name = "チョコレートチャンクスコーン", Category = "デザート", Price = 300m, Description = "チョコレートたっぷりのスコーン", StockQuantity = 6, ImageUrl = "https://images.unsplash.com/photo-1588195538326-c5b1e5b027ab?w=400" },
                new Product { Id = 12, Name = "ダブルチョコレートクッキー", Category = "デザート", Price = 250m, Description = "チョコレートチップ入りクッキー", StockQuantity = 40, ImageUrl = "https://images.unsplash.com/photo-1499636136210-6f4ee915583e?w=400" }
            };
            
            modelBuilder.Entity<Product>().HasData(products);
        }
    }
}