using AuctionPlatform.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AuctionPlatform.Application.Categories.Commands.DeleteCategory;

public class DeleteCategoryCommandHandler: IRequestHandler<DeleteCategoryCommand, bool>
{
    private readonly IApplicationDbContext _context;
    
    public DeleteCategoryCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<bool> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
    {
        bool hasActiveAuctions = await _context.AuctionItems
            .AnyAsync(a => a.CategoryId == request.Id, cancellationToken);
        
        if (hasActiveAuctions)
            throw new Exception("Cannot delete category with active auctions.");
        
        var category = await _context.Categories
            .FirstOrDefaultAsync(a => a.Id == request.Id, cancellationToken);
        
        if (category == null)           
            throw new Exception("Category not found.");
        
        _context.Categories.Remove(category);
        await _context.SaveChangesAsync(cancellationToken);
        
        return true;
        
    }
}