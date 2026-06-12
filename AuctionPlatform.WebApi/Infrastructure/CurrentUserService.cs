using System.Security.Claims;
using AuctionPlatform.Application.Common.Interfaces;

namespace AuctionPlatform.WebApi.Infrastructure;

public class CurrentUserService: ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    
    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }
    
    public string? Auth0Id => _httpContextAccessor.HttpContext?.User?
        .FindFirst("sub")?.Value;
    
    public string? UserName => _httpContextAccessor.HttpContext?.User?
                                   .FindFirst("https://auction-api.com/username")?.Value 
                               ?? "Anonymous Bidder";
    
    public bool IsAuthenticated => Auth0Id != null;
}