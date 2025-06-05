using Microsoft.EntityFrameworkCore;
using CatFactsGiphyAPI.Models;

namespace CatFactsGiphyAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<SearchHistory> SearchHistories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<SearchHistory>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.Property(e => e.SearchDate).IsRequired();
                entity.Property(e => e.FactText).IsRequired();
                entity.Property(e => e.QueryWords).IsRequired().HasMaxLength(500);
                entity.Property(e => e.GifUrl).IsRequired().HasMaxLength(500);
            });
        }
    }
}