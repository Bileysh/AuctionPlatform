using AuctionPlatform.Application.Categories.Queries.GetCategories;
using AuctionPlatform.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AuctionPlatform.Application.Categories.Queries.GetCategoriesById;

public class GetCategoriesByIdQueryHandler : IRequestHandler<GetCategoriesByIdQuery, CategoryDto>
{
    private readonly IApplicationDbContext _context;
    
    public GetCategoriesByIdQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<CategoryDto> Handle(GetCategoriesByIdQuery request, CancellationToken cancellationToken)
    {
        var categories = await _context.Categories
            .AsNoTracking()
            .Select(c => new CategoryDto(c.Id, c.Name))
            .FirstOrDefaultAsync(a => a.Id == request.Id, cancellationToken);
        
        if (categories == null)
            throw new Exception("Category not found.");
        
        return categories;
    }

}