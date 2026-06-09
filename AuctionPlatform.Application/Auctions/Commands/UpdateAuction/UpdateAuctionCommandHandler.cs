using AuctionPlatform.Application.Common.Exceptions;
using AuctionPlatform.Application.Common.Interfaces;
using AuctionPlatform.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AuctionPlatform.Application.Auctions.Commands.UpdateAuction;

public class UpdateAuctionCommandHandler : IRequestHandler<UpdateAuctionCommand, bool>
{
    private readonly IApplicationDbContext _context;
    
    public UpdateAuctionCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(UpdateAuctionCommand request, CancellationToken cancellationToken)
    {
        var auction = await _context.AuctionItems
            .Include(a => a.Bids)
            .FirstOrDefaultAsync(a => a.Id == request.Id, cancellationToken);
        
        if (auction == null)
            throw new NotFoundException(nameof(AuctionItem), request.Id);
        
        auction.UpdateDetails(request.Title, request.Description, request.EndsAt, request.CategoryId);
        
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
}