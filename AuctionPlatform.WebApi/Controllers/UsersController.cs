using AuctionPlatform.Application.Users.Commands.CreateUser;
using AuctionPlatform.Application.Users.Commands.DeleteUser;
using AuctionPlatform.Application.Users.Commands.Deposit;
using AuctionPlatform.Application.Users.Commands.UpdateUser;
using AuctionPlatform.Application.Users.Queries.GetAllUsers;
using AuctionPlatform.Application.Users.Queries.GetUserById;
using AuctionPlatform.WebApi.DTO;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AuctionPlatform.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly ISender _sender;

    public UsersController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserCommand command)
    {
        var userId = await _sender.Send(command);
        return CreatedAtAction(nameof(CreateUser), new { id = userId }, userId);
    }

    [HttpPost("deposit")]

    public async Task<IActionResult> DepositCommand(Guid id, [FromBody] DepositRequest request)
    {
        var command = new DepositCommand(id, request.Amount);
        var result = await _sender.Send(command);
        
        return Ok(new { Success = result, Message = "Balance replenished successfully" });
    }
    
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetUser(Guid id)
    {
        var query = new GetUserByIdQuery(id);
        var result = await _sender.Send(query);
        return Ok(result);
    }

    public record UpdateUserRequest(string Username);

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateUser(Guid id, [FromBody] UpdateUserRequest request)
    {
        var command = new UpdateUserCommand(id, request.Username);
        var result = await _sender.Send(command);
        return Ok(new { Success = result, Message = "User updated successfully" });
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteUser(Guid id)
    {
        var command = new DeleteUserCommand(id);
        var result = await _sender.Send(command);
        return Ok(new { Success = result, Message = "User deleted successfully" });
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAllUsers()
    {
        var query = new GetAllUsersQuery();
        var result = await _sender.Send(query);
        return Ok(result);
    }
}