using FluentValidation;

namespace AuctionPlatform.Application.Users.Commands.CreateUser;

public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator()
    {
        RuleFor(v => v.Username)
            .NotEmpty().WithMessage("Username is required.")
            .MinimumLength(3).WithMessage("Username must be at least 3 characters long.")
            .MaximumLength(50).WithMessage("Username must not exceed 50 characters.");
    }
}