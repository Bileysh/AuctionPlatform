using AuctionPlatform.Application.Users.Commands.UpdateUser;
using FluentValidation;

namespace AuctionPlatform.Application.Categories.Commands.CreateCategory;

public class CreateCategoryCommandValidator: AbstractValidator<CreateCategoryCommand>
{
    public CreateCategoryCommandValidator()
    {
        RuleFor(v => v.Name)
            .NotEmpty().WithMessage("Category name must be provided!")
            .MaximumLength(100).WithMessage("Category name must not exceed 100 characters!");
    }
}