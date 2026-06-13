using System.Linq.Expressions;
using AuctionPlatform.Application.Common.Interfaces;
using AuctionPlatform.Domain.Entities;
using AuctionPlatform.Domain.Entities.Interfaces;
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
    public DbSet<Transaction> Transactions => Set<Transaction>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Comment> Comments => Set<Comment>();
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Transaction>()
            .HasOne(t => t.User)
            .WithMany(u => u.Transactions)
            .HasForeignKey(t => t.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<Category>().HasData(
            new { Id = 1, Name = "Електроніка" },
            new { Id = 2, Name = "Автомобілі" },
            new { Id = 3, Name = "Мистецтво" }
        );
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
            entity.HasOne(e => e.Category)
                .WithMany(c => c.Auctions) 
                .HasForeignKey(e => e.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Bid>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasOne(e => e.AuctionItem)
                .WithMany(a => a.Bids)
                .HasForeignKey(e => e.AuctionItemId)
                .OnDelete(DeleteBehavior.Cascade); 
            entity.HasOne(e => e.Bidder)
                .WithMany()
                .HasForeignKey(e => e.BidderId)
                .OnDelete(DeleteBehavior.Restrict);
        });
        
        modelBuilder.Entity<Comment>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasOne(e => e.AuctionItem)
                .WithMany(a => a.Comments) 
                .HasForeignKey(e => e.AuctionItemId)
                .OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(e => e.Author)
                .WithMany()
                .HasForeignKey(e => e.AuthorId)
                .OnDelete(DeleteBehavior.Restrict);
        });
        
        modelBuilder.Entity<Transaction>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.HasOne(e => e.User)
                .WithMany(u => u.Transactions)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (typeof(ISoftDeletable).IsAssignableFrom(entityType.ClrType))
            {
                var parameter = Expression.Parameter(entityType.ClrType, "e");
                var property = Expression.PropertyOrField(parameter, nameof(ISoftDeletable.IsDeleted));
                var condition = Expression.Equal(property, Expression.Constant(false));
                var lambda = Expression.Lambda(condition, parameter);
                
                modelBuilder.Entity(entityType.ClrType).HasQueryFilter(lambda);
            }
        }
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        foreach (var entry in ChangeTracker.Entries<ISoftDeletable>())
        {
            if (entry.State == EntityState.Deleted)
            {
                entry.State = EntityState.Modified;
                entry.Entity.IsDeleted = true;
            }            
        }

        return base.SaveChangesAsync(cancellationToken);
    }
}