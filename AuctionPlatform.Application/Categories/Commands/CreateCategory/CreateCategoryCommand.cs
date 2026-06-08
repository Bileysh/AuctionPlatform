using MediatR;

namespace AuctionPlatform.Application.Categories.Commands.CreateCategory;

public record CreateCategoryCommand(string Name): IRequest<int>;