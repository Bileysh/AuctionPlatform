using MediatR;

namespace AuctionPlatform.Application.Users.Commands.Deposit;

public record DepositCommand(Guid UserId, decimal Amount) : IRequest<bool>;