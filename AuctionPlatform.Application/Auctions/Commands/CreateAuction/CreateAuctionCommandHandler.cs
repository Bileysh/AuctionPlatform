using AuctionPlatform.Application.Common.Interfaces;
using AuctionPlatform.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AuctionPlatform.Application.Auctions.Commands.CreateAuction;

public class CreateAuctionCommandHandler : IRequestHandler<CreateAuctionCommand, Guid>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;
    private readonly IAuctionNotificationService _notificationService;
    public CreateAuctionCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService, IAuctionNotificationService notificationService)
    {
        _context = context;
        _currentUserService = currentUserService;
        _notificationService = notificationService;
    }

    public async Task<Guid> Handle(CreateAuctionCommand request, CancellationToken cancellationToken)
    {
        var auth0Id = _currentUserService.Auth0Id;
        var seller = await _context.Users.FirstOrDefaultAsync(u => u.Auth0Id == auth0Id, cancellationToken);
        
        var auctionItem = new AuctionItem(
            request.Title,
            request.Description,
            request.StartingPrice,
            request.EndsAt,
            seller!.Id,
            request.CategoryId
        );

        _context.AuctionItems.Add(auctionItem);

        await _context.SaveChangesAsync(cancellationToken);
        
        await _notificationService.SendAuctionCreatedAsync(auctionItem.Id, cancellationToken);
        
        return auctionItem.Id;
    }
}