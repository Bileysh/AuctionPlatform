using AuctionPlatform.Application.Common.Interfaces;
using AuctionPlatform.Application.Common.Models;
using AuctionPlatform.Domain.Entities.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AuctionPlatform.Application.Auctions.Queries.GetActiveAuctions;

public class GetActiveAuctionsQueryHandler : IRequestHandler<GetActiveAuctionsQuery, PaginatedList<AuctionDto>>
    {
    private readonly IApplicationDbContext _context;

    public GetActiveAuctionsQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PaginatedList<AuctionDto>> Handle(GetActiveAuctionsQuery request, CancellationToken cancellationToken)
    {
        var query = _context.AuctionItems
            .AsNoTracking()
            .Where(a => a.Status == AuctionStatus.Active)
            .AsQueryable();
        
        if (!string.IsNullOrEmpty(request.SearchTerm) )
        {
            var searchTerm = request.SearchTerm.ToLower();
            query = query
                .Where(a => a.Title.ToLower()
                    .Contains(searchTerm) || a.Description.ToLower().Contains(searchTerm));
        }
        if (request.CategoryId.HasValue)
        {
            query = query.Where(a => a.CategoryId == request.CategoryId.Value);
        }
        
        query = query.OrderByDescending(a => a.CreatedAt);
        
        var projectedQuery = query.Select(a => new AuctionDto(
            a.Id,
            a.Title,
            a.CurrentPrice,
            a.EndsAt,
            a.Category.Name
        ));
        
        return await PaginatedList<AuctionDto>.CreateAsync(projectedQuery, request.PageNumber, request.PageSize, cancellationToken);
    }
}