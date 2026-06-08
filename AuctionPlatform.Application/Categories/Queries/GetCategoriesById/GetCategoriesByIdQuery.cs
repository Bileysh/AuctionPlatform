using AuctionPlatform.Application.Categories.Queries.GetCategories;
using MediatR;

namespace AuctionPlatform.Application.Categories.Queries.GetCategoriesById;

public record GetCategoriesByIdQuery(int Id): IRequest<CategoryDto>;