using AuctionPlatform.Application.Common.Interfaces;
using MediatR;

namespace AuctionPlatform.Application.Common.Behaviors;

public class ResourceOwnershipBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IOwnedResourceRequest
{
    private readonly ICurrentUserService _currentUserService;
    private readonly IOwnershipChecker _ownershipChecker;

    public ResourceOwnershipBehavior(ICurrentUserService currentUserService, IOwnershipChecker ownershipChecker)
    {
        _currentUserService = currentUserService;
        _ownershipChecker = ownershipChecker;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var auth0Id = _currentUserService.Auth0Id;

        if (string.IsNullOrEmpty(auth0Id))
            throw new UnauthorizedAccessException("Ви повинні бути авторизовані.");

        var isOwner = await _ownershipChecker.IsOwnerAsync(request.ResourceId, request.Type, auth0Id, cancellationToken);

        if (!isOwner)
            throw new UnauthorizedAccessException("Ви не маєте прав доступу до цього ресурсу.");

        return await next();
    }
}