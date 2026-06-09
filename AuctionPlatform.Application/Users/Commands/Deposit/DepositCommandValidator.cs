using FluentValidation;

namespace AuctionPlatform.Application.Users.Commands.Deposit;

public class DepositCommandValidator: AbstractValidator<DepositCommand>
{
    public DepositCommandValidator()
    {
        RuleFor(v => v.Amount)
            .GreaterThan(0).WithMessage("Deposit amount must be strictly greater than zero!");
        RuleFor(v => v.UserId)
            .NotEmpty().WithMessage("User id must be provided!");
    }
}