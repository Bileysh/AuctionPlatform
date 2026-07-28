using AuctionPlatform.Application.Auctions.Commands.AddComment;
using AuctionPlatform.Application.Auctions.Commands.CancelAuction;
using AuctionPlatform.Application.Auctions.Commands.CreateAuction;
using AuctionPlatform.Application.Auctions.Commands.DeleteComment;
using AuctionPlatform.Application.Auctions.Commands.PlaceBid;
using AuctionPlatform.Application.Auctions.Commands.UpdateAuction;
using AuctionPlatform.Application.Auctions.Queries.GetActiveAuctions;
using AuctionPlatform.Application.Auctions.Queries.GetAuctionById;
using AuctionPlatform.Application.Auctions.Queries.GetMyAuctions;
using AuctionPlatform.Application.Common.Models;
using AuctionPlatform.WebApi.DTO;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuctionPlatform.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuctionsController: ControllerBase
{
    private readonly ISender _sender;
    
    public AuctionsController(ISender sender)
    {
        _sender = sender;
    }
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> CreateAuction([FromBody] CreateAuctionCommand command, CancellationToken cancellationToken)
    {
        var auctionId = await _sender.Send(command, cancellationToken);
        return CreatedAtAction(nameof(CreateAuction), new { id = auctionId }, auctionId);
    }
    
    [HttpGet("active")]
    public async Task<IActionResult> GetActiveAuctions([FromQuery] GetActiveAuctionsQuery query)
    {
        var auctions = await _sender.Send(query);
        return Ok(auctions); 
    }
    
    [HttpGet("my")]
    public async Task<IActionResult> GetMyAuctions([FromQuery] GetMyAuctionsQuery query)
    {
        var auctions = await _sender.Send(query);
        return Ok(auctions);
    }
    
    [Authorize]
    [HttpPost("{id}/bid")]
    public async Task<IActionResult> PlaceBid(Guid id,[FromBody] PlaceBidCommand command)
    {
        if (id != command.AuctionId)
        {
            command = command with { AuctionId = id }; 
        }
        var result = await _sender.Send(command);
        return Ok(new { Success = result, Message = "Bid placed successfully" });
    }
    
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetAuctionById(Guid id)
    {
        var query = new GetAuctionByIdQuery(id);
        var result = await _sender.Send(query);
        return Ok(result);
    }
    
    [Authorize]
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateAuction(Guid id, [FromBody] UpdateAuctionRequest request)
    {
        var command = new UpdateAuctionCommand(id, request.Title, request.Description, request.EndsAt, request.CategoryId);
        var result = await _sender.Send(command);
        return Ok(new { Success = result, Message = "Auction updated successfully" });
    }
    
    [Authorize]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> CancelAuction(Guid id)
    {
        var command = new CancelAuctionCommand(id);
        var result = await _sender.Send(command);
        return Ok(new { Success = result, Message = "Auction canceled successfully" });
    }
    
    [Authorize]
    [HttpPost("comments")]
    public async Task<IActionResult> AddComment([FromBody] AddCommentCommand command)
    {
        var commentId = await _sender.Send(command);
        return Ok(new { CommentId = commentId, Message = "Comment added successfully" });
    }
    
    [Authorize(Roles = "Admin")]
    [HttpDelete("comments/{commentId:guid}")]
    public async Task<IActionResult> DeleteComment(Guid commentId)
    {
        var command = new DeleteCommentCommand(commentId);
        var result = await _sender.Send(command);
        return Ok(new { Success = result, Message = "Comment deleted successfully" });
    }
    
}