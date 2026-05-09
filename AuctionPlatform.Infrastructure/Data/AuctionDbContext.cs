using AuctionPlatform.Application.Common.Interfaces;
using AuctionPlatform.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AuctionPlatform.Infrastructure.Data;

public class AuctionDbContext : DbContext, IApplicationDbContext
{
    public AuctionDbContext(DbContextOptions<AuctionDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<AuctionItem> AuctionItems => Set<AuctionItem>();
    public DbSet<Bid> Bids => Set<Bid>();
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.Auth0Id).IsUnique();
        });

        modelBuilder.Entity<AuctionItem>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Version).IsRowVersion();
            entity.HasOne(e => e.Seller)
                .WithMany()
                .HasForeignKey(e => e.SellerId)
                .OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(e => e.Winner)
                .WithMany()
                .HasForeignKey(e => e.WinnerId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Bid>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasOne(e => e.AuctionItem)
                .WithMany() 
                .HasForeignKey(e => e.AuctionItemId)
                .OnDelete(DeleteBehavior.Cascade); 
            entity.HasOne(e => e.Bidder)
                .WithMany()
                .HasForeignKey(e => e.BidderId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}