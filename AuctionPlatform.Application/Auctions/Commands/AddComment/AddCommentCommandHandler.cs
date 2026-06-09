using AuctionPlatform.Application.Common.Exceptions;
using AuctionPlatform.Application.Common.Interfaces;
using AuctionPlatform.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AuctionPlatform.Application.Auctions.Commands.AddComment;

public class AddCommentCommandHandler : IRequestHandler<AddCommentCommand, Guid >
{
    private readonly IApplicationDbContext _context;
    public AddCommentCommandHandler(IApplicationDbContext context) => _context = context;

    public async Task<Guid> Handle(AddCommentCommand request, CancellationToken cancellationToken)
    {
        var auction = await _context.AuctionItems
            .FirstOrDefaultAsync(a => a.Id == request.AuctionId, cancellationToken);
        
        if (auction == null) throw new NotFoundException(nameof(AuctionItem), request.AuctionId);

        var user = await _context.Users
            .FirstOrDefaultAsync(a => a.Id == request.AuthorId, cancellationToken);
        
        if (user == null) throw new NotFoundException(nameof(User), request.AuthorId);

        var comment = new Comment(request.AuctionId, request.AuthorId, request.Text);
       
        _context.Comments.Add(comment);
        await _context.SaveChangesAsync(cancellationToken);
        return comment.Id;
    }
}