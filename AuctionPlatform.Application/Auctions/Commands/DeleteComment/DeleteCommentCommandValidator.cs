using FluentValidation;

namespace AuctionPlatform.Application.Auctions.Commands.DeleteComment;

public class DeleteCommentCommandValidator: AbstractValidator<DeleteCommentCommand>
{
    public DeleteCommentCommandValidator()
    {
        RuleFor(v => v.Id)
            .NotEmpty().WithMessage("Comment ID is required.");
    }
}