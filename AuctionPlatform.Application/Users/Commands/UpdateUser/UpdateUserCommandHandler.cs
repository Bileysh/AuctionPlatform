using AuctionPlatform.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AuctionPlatform.Application.Users.Commands.UpdateUser;

public class UpdateUserCommandHandler: IRequestHandler<UpdateUserCommand, bool>
{
    private readonly IApplicationDbContext _context;
    
    public UpdateUserCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<bool> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Id == request.Id, cancellationToken);
        
        if (user == null)
            throw new Exception("User not found.");
        
        user.UpdateProfile(request.Name);
        await _context.SaveChangesAsync(cancellationToken);
        
        return true;
    }
}