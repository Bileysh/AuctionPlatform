using AuctionPlatform.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AuctionPlatform.Application.Categories.Queries.GetCategories;

public class GetAllCategoriesQueryHandler : IRequestHandler<GetAllCategoriesQuery, List<CategoryDto>>
{
    private readonly IApplicationDbContext _context;
    
    public GetAllCategoriesQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<CategoryDto>> Handle(GetAllCategoriesQuery request, CancellationToken cancellationToken)
    {
        var categories = await _context.Categories
            .AsNoTracking()
            .Select(c => new CategoryDto(c.Id, c.Name))
            .ToListAsync(cancellationToken);

        if (categories == null)
            throw new Exception("Category not found.");
        
        return categories;
    }

}