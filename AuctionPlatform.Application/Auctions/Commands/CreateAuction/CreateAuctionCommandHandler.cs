using AuctionPlatform.Application.Common.Interfaces;
using AuctionPlatform.Domain.Entities;
using MediatR;

namespace AuctionPlatform.Application.Auctions.Commands.CreateAuction;

public class CreateAuctionCommandHandler : IRequestHandler<CreateAuctionCommand, Guid>
{
    private readonly IApplicationDbContext _context;

    public CreateAuctionCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(CreateAuctionCommand request, CancellationToken cancellationToken)
    {
        var auctionItem = new AuctionItem(
            request.Title,
            request.Description,
            request.StartingPrice,
            request.EndsAt,
            request.SellerId
        );

        _context.AuctionItems.Add(auctionItem);

        await _context.SaveChangesAsync(cancellationToken);

        return auctionItem.Id;
    }
}