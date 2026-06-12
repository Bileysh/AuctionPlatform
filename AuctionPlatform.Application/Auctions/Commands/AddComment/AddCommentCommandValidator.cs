using FluentValidation;

namespace AuctionPlatform.Application.Auctions.Commands.AddComment;

public class AddCommentCommandValidator: AbstractValidator<AddCommentCommand>
{
    public AddCommentCommandValidator()
    {
        RuleFor(v => v.AuctionId)
            .NotEmpty().WithMessage("Auction ID is required.");
        
        RuleFor(v => v.Text)
            .NotEmpty().WithMessage("Comment text is required.")
            .MaximumLength(500).WithMessage("Comment text must not exceed 500 characters.");
    }
}