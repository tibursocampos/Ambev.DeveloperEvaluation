using Ambev.DeveloperEvaluation.Domain.Strategies.Discount;

using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Strategies.Discount;

/// <summary>
/// Contains unit tests for the discount strategies.
/// </summary>
public class DiscountStrategyTests
{
    /// <summary>
    /// Tests that no discount is applied by the NoDiscountStrategy.
    /// </summary>
    [Fact(DisplayName = "No discount should be applied by NoDiscountStrategy")]
    public void Given_NoDiscountStrategy_When_CalculateDiscount_Then_NoDiscountShouldBeApplied()
    {
        // Arrange
        var strategy = new NoDiscountStrategy();
        int quantity = 5;
        decimal unitPrice = 10m;

        // Act
        var discount = strategy.CalculateDiscount(quantity, unitPrice);

        // Assert
        Assert.Equal(0m, discount);
    }

    /// <summary>
    /// Tests that a 10% discount is applied by the TenPercentDiscountStrategy.
    /// </summary>
    [Fact(DisplayName = "10% discount should be applied by TenPercentDiscountStrategy")]
    public void Given_TenPercentDiscountStrategy_When_CalculateDiscount_Then_TenPercentDiscountShouldBeApplied()
    {
        // Arrange
        var strategy = new TenPercentDiscountStrategy();
        int quantity = 5;
        decimal unitPrice = 10m;

        // Act
        var discount = strategy.CalculateDiscount(quantity, unitPrice);

        // Assert
        Assert.Equal(5m, discount); // 10% of 50 (5 * 10) is 5
    }

    /// <summary>
    /// Tests that a 20% discount is applied by the TwentyPercentDiscountStrategy.
    /// </summary>
    [Fact(DisplayName = "20% discount should be applied by TwentyPercentDiscountStrategy")]
    public void Given_TwentyPercentDiscountStrategy_When_CalculateDiscount_Then_TwentyPercentDiscountShouldBeApplied()
    {
        // Arrange
        var strategy = new TwentyPercentDiscountStrategy();
        int quantity = 5;
        decimal unitPrice = 10m;

        // Act
        var discount = strategy.CalculateDiscount(quantity, unitPrice);

        // Assert
        Assert.Equal(10m, discount); // 20% of 50 (5 * 10) is 10
    }
}