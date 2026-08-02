using AuctionPlatform.Application.Common.Interfaces;
using AuctionPlatform.Application.Common.Models;
using AuctionPlatform.Domain.Entities.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AuctionPlatform.Application.Auctions.Queries.GetActiveAuctions;

public class GetActiveAuctionsQueryHandler : IRequestHandler<GetActiveAuctionsQuery, PaginatedList<AuctionDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly ICacheService _cacheService;

    public GetActiveAuctionsQueryHandler(IApplicationDbContext context, ICacheService cacheService)
    {
        _context = context;
        _cacheService = cacheService;
    }

    public async Task<PaginatedList<AuctionDto>> Handle(GetActiveAuctionsQuery request, CancellationToken cancellationToken)
    {
        var cacheKey = $"auctions:active:{request.PageNumber}:{request.PageSize}:{request.SearchTerm}:{request.CategoryId}:{request.SortColumn}:{request.SortOrder}".ToLower();

        var cachedResult = await _cacheService.GetAsync<PaginatedList<AuctionDto>>(cacheKey, cancellationToken);
        
        if (cachedResult != null)
        {
            return cachedResult; 
        }

        var query = _context.AuctionItems
            .AsNoTracking()
            .Where(a => a.Status == AuctionStatus.Active)
            .AsQueryable();
        
        if (!string.IsNullOrEmpty(request.SearchTerm))
        {
            var searchTerm = request.SearchTerm.ToLower();
            
            query = query.Where(a => 
                EF.Functions.ILike(a.Title, $"%{searchTerm}%") || 
                EF.Functions.ILike(a.Description, $"%{searchTerm}%"));
        }
        
        if (request.CategoryId.HasValue)
        {
            query = query.Where(a => a.CategoryId == request.CategoryId.Value);
        }
        
        if (string.IsNullOrWhiteSpace(request.SortColumn))
        {
            query = query.OrderByDescending(a => a.CreatedAt);
        }
        else
        {
            bool isDesc = request.SortOrder?.ToLower() == "desc";
            
            query = request.SortColumn.ToLower() switch
            {
                "price" => isDesc ? query.OrderByDescending(a => a.CurrentPrice) : query.OrderBy(a => a.CurrentPrice),
                "endsat" => isDesc ? query.OrderByDescending(a => a.EndsAt) : query.OrderBy(a => a.EndsAt),
                "bids" => isDesc ? query.OrderByDescending(a => a.Bids.Count) : query.OrderBy(a => a.Bids.Count),
                _ => query.OrderByDescending(a => a.CreatedAt)
            };
        }
        
        var projectedQuery = query.Select(a => new AuctionDto(
            a.Id,
            a.Title,
            a.CurrentPrice,
            a.EndsAt,
            a.Category.Name,
            a.Bids.Count 
        ));
        
        var result = await PaginatedList<AuctionDto>.CreateAsync(projectedQuery, request.PageNumber, request.PageSize, cancellationToken);

        await _cacheService.SetAsync(cacheKey, result, TimeSpan.FromMinutes(1), cancellationToken);
        
        return result;
    }
}