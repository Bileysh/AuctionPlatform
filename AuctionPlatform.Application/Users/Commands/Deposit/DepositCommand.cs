using MediatR;

namespace AuctionPlatform.Application.Users.Commands.Deposit;

public record DepositCommand(decimal Amount) : IRequest<bool>;