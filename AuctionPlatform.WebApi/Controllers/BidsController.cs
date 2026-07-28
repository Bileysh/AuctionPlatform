using AuctionPlatform.Application.Bids.Queries.GetAllBids;
using AuctionPlatform.Application.Bids.Queries.GetBidById;
using AuctionPlatform.Application.Bids.Queries.GetMyBiddedAuctions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuctionPlatform.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BidsController : ControllerBase
{
    private readonly ISender _sender;
    
    public BidsController(ISender sender)
    {
        _sender = sender;
    }
    [Authorize(Roles = "Admin")]
    [HttpGet]
    public async Task<IActionResult> GetAllBids()
    {
        var query = new GetAllBidsQuery();
        var bids = await _sender.Send(query);
        return Ok(bids);
    }
    [Authorize]
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetBidById(Guid id)
    {
        var query = new GetBidByIdQuery(id);
        var bid = await _sender.Send(query);
        return Ok(bid);
    }
    
    [HttpGet("my")]
    public async Task<IActionResult> GetMyBiddedAuctions([FromQuery] GetMyBiddedAuctionsQuery query)
    {
        var auctions = await _sender.Send(query);
        return Ok(auctions);
    }
}