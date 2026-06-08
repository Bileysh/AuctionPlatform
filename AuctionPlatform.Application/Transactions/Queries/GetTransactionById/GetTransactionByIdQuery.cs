using AuctionPlatform.Application.Transactions.Queries.GetAllTransaction;
using MediatR;

namespace AuctionPlatform.Application.Transactions.Queries.GetTransactionById;

public record GetTransactionByIdQuery(Guid Id) : IRequest<TransactionDto>;