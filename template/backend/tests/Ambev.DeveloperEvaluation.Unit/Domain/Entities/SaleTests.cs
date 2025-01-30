using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;

using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities;

/// <summary>
/// Contains unit tests for the Sale entity class.
/// Tests cover status changes, validation scenarios, and business logic.
/// </summary>
public class SaleTests
{
    /// <summary>
    /// Tests that when a sale is cancelled, its status changes to Cancelled.
    /// </summary>
    [Fact(DisplayName = "Sale status should change to Cancelled when cancelled")]
    public void Given_ActiveSale_When_Cancelled_Then_StatusShouldBeCancelled()
    {
        // Arrange
        var sale = SaleTestData.GenerateValidSale();
        sale.IsCancelled = false;

        // Act
        sale.Cancel();

        // Assert
        Assert.True(sale.IsCancelled);
    }

    /// <summary>
    /// Tests that the total amount is calculated correctly.
    /// </summary>
    [Fact(DisplayName = "Total amount should be calculated correctly")]
    public void Given_SaleWithItems_When_CalculateTotalAmount_Then_TotalAmountShouldBeCorrect()
    {
        // Arrange
        var sale = SaleTestData.GenerateValidSale();

        // Act
        sale.CalculateTotalAmount();

        // Assert
        var expectedTotalAmount = sale.Items.Sum(item => item.UnitPrice * item.Quantity);
        Assert.Equal(expectedTotalAmount, sale.TotalAmount);
    }

    /// <summary>
    /// Tests that validation passes when all sale properties are valid.
    /// </summary>
    [Fact(DisplayName = "Validation should pass for valid sale data")]
    public void Given_ValidSaleData_When_Validated_Then_ShouldReturnValid()
    {
        // Arrange
        var sale = SaleTestData.GenerateValidSale();

        // Act
        var result = sale.Validate();

        // Assert
        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }

    /// <summary>
    /// Tests that validation fails when sale properties are invalid.
    /// </summary>
    [Fact(DisplayName = "Validation should fail for invalid sale data")]
    public void Given_InvalidSaleData_When_Validated_Then_ShouldReturnInvalid()
    {
        // Arrange
        var sale = new Sale
        {
            SaleNumber = "", // Invalid: empty
            SaleDate = DateTime.MinValue, // Invalid: default date
            CustomerId = 0, // Invalid: default ID
            CustomerName = "", // Invalid: empty
            BranchId = 0, // Invalid: default ID
            BranchName = "", // Invalid: empty
            Items = new List<SaleItem>(), // Invalid: empty list
            IsCancelled = false
        };

        // Act
        var result = sale.Validate();

        // Assert
        Assert.False(result.IsValid);
        Assert.NotEmpty(result.Errors);
    }
}