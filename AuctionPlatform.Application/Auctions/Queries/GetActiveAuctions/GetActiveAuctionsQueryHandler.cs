using AuctionPlatform.Application.Common.Interfaces;
using AuctionPlatform.Domain.Entities.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AuctionPlatform.Application.Auctions.Queries.GetActiveAuctions;

public class GetActiveAuctionsQueryHandler : IRequestHandler<GetActiveAuctionsQuery, List<AuctionDto>>
    {
    private readonly IApplicationDbContext _context;

    public GetActiveAuctionsQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<AuctionDto>> Handle(GetActiveAuctionsQuery request, CancellationToken cancellationToken)
    {
        return await _context.AuctionItems
            .Where(a => a.Status == AuctionStatus.Active &&a.EndsAt > DateTime.UtcNow)
            .OrderBy(a => a.EndsAt)
            .Select(a => new AuctionDto(
                a.Id,
                a.Title,
                a.CurrentPrice,
                a.EndsAt,
                a.Category.Name))
            .ToListAsync(cancellationToken);}
}