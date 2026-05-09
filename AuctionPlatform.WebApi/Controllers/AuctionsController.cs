using AuctionPlatform.Application.Auctions.Commands.CreateAuction;
using AuctionPlatform.Application.Auctions.Commands.PlaceBid;
using AuctionPlatform.Application.Auctions.Queries.GetActiveAuctions;
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
    
    [HttpGet]
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
}