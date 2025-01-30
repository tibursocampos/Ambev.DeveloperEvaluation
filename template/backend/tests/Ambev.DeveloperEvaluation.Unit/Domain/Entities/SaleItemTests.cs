using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;

using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities;

/// <summary>
/// Contains unit tests for the SaleItem entity class.
/// Tests cover validation scenarios and business logic.
/// </summary>
public class SaleItemTests
{
    /// <summary>
    /// Tests that no discount is applied when the quantity is less than the minimum for discount.
    /// </summary>
    [Fact(DisplayName = "No discount should be applied for quantity less than minimum for discount")]
    public void Given_QuantityLessThanMinForDiscount_When_ApplyDiscount_Then_NoDiscountShouldBeApplied()
    {
        // Arrange
        var saleItem = new SaleItem
        {
            Quantity = 3,
            UnitPrice = 10m
        };

        // Act
        saleItem.ApplyDiscount();

        // Assert
        Assert.Equal(0m, saleItem.Discount);
    }

    /// <summary>
    /// Tests that a 10% discount is applied when the quantity is between the minimum and maximum for 10% discount.
    /// </summary>
    [Fact(DisplayName = "10% discount should be applied for quantity between minimum and maximum for 10% discount")]
    public void Given_QuantityBetweenMinAndMaxForTenPercentDiscount_When_ApplyDiscount_Then_TenPercentDiscountShouldBeApplied()
    {
        // Arrange
        var saleItem = new SaleItem
        {
            Quantity = 5,
            UnitPrice = 10m
        };

        // Act
        saleItem.ApplyDiscount();

        // Assert
        Assert.Equal(5m, saleItem.Discount); // 10% of 50 (5 * 10) is 5
    }

    /// <summary>
    /// Tests that a 20% discount is applied when the quantity is between the maximum for 10% discount and the maximum for 20% discount.
    /// </summary>
    [Fact(DisplayName = "20% discount should be applied for quantity between maximum for 10% discount and maximum for 20% discount")]
    public void Given_QuantityBetweenMaxForTenPercentAndMaxForTwentyPercentDiscount_When_ApplyDiscount_Then_TwentyPercentDiscountShouldBeApplied()
    {
        // Arrange
        var saleItem = new SaleItem
        {
            Quantity = 15,
            UnitPrice = 10m
        };

        // Act
        saleItem.ApplyDiscount();

        // Assert
        Assert.Equal(30m, saleItem.Discount); // 20% of 150 (15 * 10) is 30
    }

    /// <summary>
    /// Tests that an exception is thrown when the quantity exceeds the maximum allowed for discount.
    /// </summary>
    [Fact(DisplayName = "Exception should be thrown for quantity exceeding maximum allowed for discount")]
    public void Given_QuantityExceedsMaxForDiscount_When_ApplyDiscount_Then_ShouldThrowException()
    {
        // Arrange
        var saleItem = new SaleItem
        {
            Quantity = 25,
            UnitPrice = 10m
        };

        // Act & Assert
        var exception = Assert.Throws<InvalidOperationException>(() => saleItem.ApplyDiscount());
        Assert.Equal("Cannot sell more than 20 identical items.", exception.Message);
    }

    /// <summary>
    /// Tests that validation passes when all sale item properties are valid.
    /// </summary>
    [Fact(DisplayName = "Validation should pass for valid sale item data")]
    public void Given_ValidSaleItemData_When_Validated_Then_ShouldReturnValid()
    {
        // Arrange
        var saleItem = SaleItemTestData.GenerateValidSaleItem();

        // Act
        var result = saleItem[0].Validate();

        // Assert
        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }

    /// <summary>
    /// Tests that validation fails when sale item properties are invalid.
    /// </summary>
    [Fact(DisplayName = "Validation should fail for invalid sale item data")]
    public void Given_InvalidSaleItemData_When_Validated_Then_ShouldReturnInvalid()
    {
        // Arrange
        var saleItem = new SaleItem
        {
            ProductId = 0, // Invalid: default ID
            ProductName = "", // Invalid: empty
            Quantity = 0, // Invalid: zero
            UnitPrice = 0m // Invalid: zero
        };

        // Act
        var result = saleItem.Validate();

        // Assert
        Assert.False(result.IsValid);
        Assert.NotEmpty(result.Errors);
    }
}
