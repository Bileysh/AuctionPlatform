using AuctionPlatform.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AuctionPlatform.Application.Categories.Commands.UpdateCategory;

public class UpdateCategoryCommandHandler: IRequestHandler<UpdateCategoryCommand, bool>
{
    private readonly IApplicationDbContext _context;
    
    public UpdateCategoryCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<bool> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = await _context.Categories
            .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);
       
        if (category == null)
            throw new Exception("Category not found.");
        
        category.UpdateName(category.Name);
        
        await _context.SaveChangesAsync(cancellationToken);
        
        return true;
    }
}