using AuctionPlatform.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AuctionPlatform.Application.Common.Services;

public class OwnershipChecker : IOwnershipChecker
{
    private readonly IApplicationDbContext _context;

    public OwnershipChecker(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> IsOwnerAsync(Guid resourceId, ResourceType type, string auth0Id, CancellationToken cancellationToken)
    {
        var user = await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Auth0Id == auth0Id, cancellationToken);
            
        if (user == null) return false;

        return type switch
        {
            ResourceType.Auction => await _context.AuctionItems
                .AnyAsync(a => a.Id == resourceId && a.SellerId == user.Id, cancellationToken),
                
            ResourceType.Comment => await _context.Comments.AnyAsync(c => c.Id == resourceId && c.AuthorId == user.Id, cancellationToken),
            
            _ => false
        };
    }
}