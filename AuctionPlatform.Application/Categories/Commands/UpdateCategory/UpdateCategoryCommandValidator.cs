using FluentValidation;

namespace AuctionPlatform.Application.Categories.Commands.UpdateCategory;

public class UpdateCategoryCommandValidator: AbstractValidator<UpdateCategoryCommand>
{
    public UpdateCategoryCommandValidator()
    {
        RuleFor(v => v.Id)
            .NotEmpty().WithMessage("Category id must be provided!");
        RuleFor(v => v.Name)
            .NotEmpty().WithMessage("Category name must be provided!")
            .MaximumLength(100).WithMessage("Category name must not exceed 100 characters!");
    }
}