using AuctionPlatform.Application.Common.Exceptions;
using AuctionPlatform.Application.Common.Interfaces;
using AuctionPlatform.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AuctionPlatform.Application.Auctions.Commands.AddComment;

public class AddCommentCommandHandler : IRequestHandler<AddCommentCommand, Guid >
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;
    public AddCommentCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<Guid> Handle(AddCommentCommand request, CancellationToken cancellationToken)
    {
        var auth0Id = _currentUserService.Auth0Id;
        
        var auction = await _context.AuctionItems
            .FirstOrDefaultAsync(a => a.Id == request.AuctionId, cancellationToken);
        
        if (auction == null) throw new NotFoundException(nameof(AuctionItem), request.AuctionId);

        var user = await _context.Users
            .FirstOrDefaultAsync(a => a.Auth0Id == auth0Id, cancellationToken);
        
        if (user == null) throw new NotFoundException(nameof(User), user!.Id);

        var comment = new Comment(request.AuctionId, user.Id , request.Text);
       
        _context.Comments.Add(comment);
        await _context.SaveChangesAsync(cancellationToken);
        return comment.Id;
    }
}