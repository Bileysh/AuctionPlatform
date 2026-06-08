using AuctionPlatform.Application.Transactions.Queries.GetAllTransaction;
using AuctionPlatform.Application.Transactions.Queries.GetTransactionById;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AuctionPlatform.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TransactionsController : ControllerBase
{
    private readonly ISender _sender;

    public TransactionsController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllTransactions()
    {
        var query = new GetAllTransactionsQuery();
        var transactions = await _sender.Send(query);
        return Ok(transactions);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetTransactionById(Guid id)
    {
        var query = new GetTransactionByIdQuery(id);
        var transaction = await _sender.Send(query);
        return Ok(transaction);
    }
}
