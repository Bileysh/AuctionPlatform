using AuctionPlatform.Application.Common.Exceptions;
using AuctionPlatform.Application.Common.Interfaces;
using AuctionPlatform.Domain.Entities;
using AuctionPlatform.Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AuctionPlatform.Application.Auctions.Commands.CancelAuction;

public class CancelAuctionCommandHandler : IRequestHandler<CancelAuctionCommand, bool>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;
    
    public CancelAuctionCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }
    
    public async Task<bool> Handle(CancelAuctionCommand request, CancellationToken cancellationToken)
    {
        var auth0Id = _currentUserService.Auth0Id;
        if(string.IsNullOrEmpty(auth0Id))
            throw new UnauthorizedAccessException("You must be logged in to cancel an auction.");
        
        var currentUser = await _context.Users
            .FirstOrDefaultAsync(u => u.Auth0Id == auth0Id, cancellationToken);
        
        if (currentUser == null)
            throw new NotFoundException(nameof(User), auth0Id);
        
        var auction = await _context.AuctionItems
            .Include(a => a.Bids)
            .FirstOrDefaultAsync(a => a.Id == request.Id, cancellationToken);

        if (auction == null)
            throw new NotFoundException(nameof(AuctionItem), request.Id);
        
        if (auction.SellerId != currentUser.Id)
            throw new BusinessRuleException("Ви не можете скасувати чужий аукціон.");       
        
        auction.Cancel();
        
        await _context.SaveChangesAsync(cancellationToken);
        
        return true;
    }
}