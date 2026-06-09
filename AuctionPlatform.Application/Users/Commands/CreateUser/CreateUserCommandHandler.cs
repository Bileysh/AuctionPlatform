using AuctionPlatform.Application.Common.Interfaces;
using AuctionPlatform.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AuctionPlatform.Application.Users.Commands.CreateUser;

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Guid>
{
    private readonly IApplicationDbContext _context;
    
    public CreateUserCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<Guid> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var existingUser = await _context.Users
            .FirstOrDefaultAsync(u => u.UserName == request.Username);
        
        if (existingUser != null)
            throw new Exception("Username already exists.");
        
        var user = new User(
            userName: request.Username, 
            auth0Id: $"local_{Guid.NewGuid()}" 
        );
        
        _context.Users.Add(user);
        await _context.SaveChangesAsync(cancellationToken);
        
        return user.Id;
    }
}