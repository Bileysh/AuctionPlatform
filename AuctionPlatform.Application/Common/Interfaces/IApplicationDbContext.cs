using AuctionPlatform.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AuctionPlatform.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<User> Users { get; }
    DbSet<AuctionItem> AuctionItems { get; }
    DbSet<Bid> Bids { get; }
    
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}