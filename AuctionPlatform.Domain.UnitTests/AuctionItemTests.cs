using AuctionPlatform.Domain.Entities;
using AuctionPlatform.Domain.Exceptions;
using FluentAssertions;

namespace AuctionPlatform.Domain.UnitTests;

public class AuctionItemTests
{
    [Fact]
    public void UpdatePriceAndWinner_WithHigherBid_ShouldUpdateSuccessfully()
    {
        var auction = new AuctionItem("Test", "Desc", 100m, DateTime.UtcNow.AddDays(1), Guid.NewGuid(), 1);
        var winnerId = Guid.NewGuid();

        auction.UpdatePriceAndWinner(150m, winnerId);

        auction.CurrentPrice.Should().Be(150m);
        auction.WinnerId.Should().Be(winnerId);
    }

    [Fact]
    public void UpdatePriceAndWinner_WithLowerBid_ShouldThrowException()
    {
        var auction = new AuctionItem("Test", "Desc", 100m, DateTime.UtcNow.AddDays(1), Guid.NewGuid(), 1);
        
        Action action = () => auction.UpdatePriceAndWinner(50m, Guid.NewGuid());

        action.Should().Throw<BusinessRuleException>();
    }
}