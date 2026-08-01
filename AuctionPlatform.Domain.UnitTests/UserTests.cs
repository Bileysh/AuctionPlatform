using System;
using System.Linq;
using AuctionPlatform.Domain.Entities;
using AuctionPlatform.Domain.Entities.Enums;
using AuctionPlatform.Domain.Exceptions;
using FluentAssertions;
using Xunit;

namespace AuctionPlatform.Domain.UnitTests;

public class UserTests
{
    [Fact]
    public void HoldFunds_WithSufficientBalance_ShouldDecreaseAvailableBalance()
    {
        var user = new User("auth0|123", "TestUser");
        user.Deposit(1000m); 
        
        var auctionId = Guid.NewGuid();
        var availableBalance = user.GetAvailableBalance();
        var amountToHold = 200m;

        user.HoldFunds(amountToHold, auctionId, availableBalance);

        user.Balance.Should().Be(1000m); 
        user.GetAvailableBalance().Should().Be(800m); 
        user.Transactions.Should().ContainSingle(t => t.Type == TransactionType.Hold && t.Amount == amountToHold);
    }

    [Fact]
    public void HoldFunds_WithInsufficientBalance_ShouldThrowBusinessRuleException()
    {
        var user = new User("auth0|123", "TestUser");
        user.Deposit(100m); 
        
        var auctionId = Guid.NewGuid();
        var availableBalance = user.GetAvailableBalance();
        var amountToHold = 200m; 

        Action action = () => user.HoldFunds(amountToHold, auctionId, availableBalance);
        
        action.Should().Throw<BusinessRuleException>()
              .WithMessage("Insufficient available funds to place this bid.");
    }

    [Fact]
    public void ReleaseFunds_ShouldRestoreAvailableBalance()
    {
        var user = new User("auth0|123", "TestUser");
        user.Deposit(500m);
        var auctionId = Guid.NewGuid();
        
        user.HoldFunds(300m, auctionId, user.GetAvailableBalance());

        user.ReleaseFunds(300m, auctionId);

        user.Balance.Should().Be(500m);
        user.GetAvailableBalance().Should().Be(500m); 
        user.Transactions.Should().Contain(t => t.Type == TransactionType.Release && t.Amount == 300m);
    }
}