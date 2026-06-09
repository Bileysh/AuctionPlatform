using AuctionPlatform.Application.Auctions.Commands.AddComment;
using AuctionPlatform.Application.Common.Exceptions;
using AuctionPlatform.Application.Common.Interfaces;
using AuctionPlatform.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AuctionPlatform.Application.Auctions.Commands.DeleteComment;

public class DeleteCommentCommandHandler: IRequestHandler<DeleteCommentCommand, bool>
{
    private readonly IApplicationDbContext _context;
    
    public DeleteCommentCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<bool> Handle(DeleteCommentCommand request, CancellationToken cancellationToken)
    {
        var comment = await _context.Comments
            .FirstOrDefaultAsync(a => a.Id == request.Id, cancellationToken);
            
        if (comment == null)
            throw new NotFoundException(nameof(Comment), request.Id);
        
        _context.Comments.Remove(comment);
        await _context.SaveChangesAsync(cancellationToken);
        
        return true;
    }
}