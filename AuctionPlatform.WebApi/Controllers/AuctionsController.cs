using AuctionPlatform.Application.Auctions.Commands.AddComment;
using AuctionPlatform.Application.Auctions.Commands.CancelAuction;
using AuctionPlatform.Application.Auctions.Commands.CreateAuction;
using AuctionPlatform.Application.Auctions.Commands.DeleteComment;
using AuctionPlatform.Application.Auctions.Commands.PlaceBid;
using AuctionPlatform.Application.Auctions.Commands.UpdateAuction;
using AuctionPlatform.Application.Auctions.Queries.GetActiveAuctions;
using AuctionPlatform.Application.Auctions.Queries.GetAuctionById;
using AuctionPlatform.WebApi.DTO;
using MediatR;
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
    
    [HttpPost]
    public async Task<IActionResult> CreateAuction([FromBody] CreateAuctionCommand command, CancellationToken cancellationToken)
    {
        var auctionId = await _sender.Send(command, cancellationToken);
        return CreatedAtAction(nameof(CreateAuction), new { id = auctionId }, auctionId);
    }
    
    [HttpGet("active")]
    public async Task<IActionResult> GetActiveAuctions()
    {
        var query = new GetActiveAuctionsQuery();
        var auctions = await _sender.Send(query);
        return Ok(auctions); 
    }
    
    [HttpPost("bid")]
    public async Task<IActionResult> PlaceBid([FromBody] PlaceBidCommand command)
    {
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
    
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateAuction(Guid id, [FromBody] UpdateAuctionRequest request)
    {
        var command = new UpdateAuctionCommand(id, request.Title, request.Description, request.EndsAt, request.CategoryId);
        var result = await _sender.Send(command);
        return Ok(new { Success = result, Message = "Auction updated successfully" });
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> CancelAuction(Guid id)
    {
        var command = new CancelAuctionCommand(id);
        var result = await _sender.Send(command);
        return Ok(new { Success = result, Message = "Auction canceled successfully" });
    }
    
    [HttpPost("{id:guid}/comments")]
    public async Task<IActionResult> AddComment(Guid id, [FromBody] AddCommentRequest request)
    {
        var command = new AddCommentCommand(id, request.AuthorId, request.Text);
        var commentId = await _sender.Send(command);
        return Ok(new { CommentId = commentId, Message = "Comment added successfully" });
    }
    
    [HttpDelete("comments/{commentId:guid}")]
    public async Task<IActionResult> DeleteComment(Guid commentId)
    {
        var command = new DeleteCommentCommand(commentId);
        var result = await _sender.Send(command);
        return Ok(new { Success = result, Message = "Comment deleted successfully" });
    }
}