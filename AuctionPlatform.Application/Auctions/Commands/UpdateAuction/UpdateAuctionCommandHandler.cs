using AuctionPlatform.Application.Common.Exceptions;
using AuctionPlatform.Application.Common.Interfaces;
using AuctionPlatform.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AuctionPlatform.Application.Auctions.Commands.UpdateAuction;

public class UpdateAuctionCommandHandler : IRequestHandler<UpdateAuctionCommand, bool>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;
    
    public UpdateAuctionCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<bool> Handle(UpdateAuctionCommand request, CancellationToken cancellationToken)
    {
        var currentUser = await _context.Users
            .FirstOrDefaultAsync(u => u.Auth0Id == _currentUserService.Auth0Id, cancellationToken);
        
        if (currentUser == null)
            throw new UnauthorizedAccessException("Користувача не знайдено в системі.");
        
        var auction = await _context.AuctionItems
            .Include(a => a.Bids)
            .FirstOrDefaultAsync(a => a.Id == request.Id, cancellationToken);
        
        if (auction == null)
            throw new NotFoundException(nameof(AuctionItem), request.Id);
        
        if (auction.SellerId != currentUser.Id)
            throw new UnauthorizedAccessException("Ви не маєте прав для редагування чужого лота.");
        
        auction.UpdateDetails(request.Title, request.Description, request.EndsAt, request.CategoryId);
        
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
}