using AuctionPlatform.Application.Common.Exceptions;
using AuctionPlatform.Application.Common.Interfaces;
using AuctionPlatform.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AuctionPlatform.Application.Users.Commands.DeleteUser;

public class DeleteUserCommandHandler: IRequestHandler<DeleteUserCommand, bool>
{
    private readonly IApplicationDbContext _context;
    
    public DeleteUserCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == request.Id, cancellationToken);
        
        if (user == null)
            throw new NotFoundException(nameof(User), request.Id);
        
        _context.Users.Remove(user);
        
        await _context.SaveChangesAsync(cancellationToken);
        
        return true;
    }
    
}