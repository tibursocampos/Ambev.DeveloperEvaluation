using Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;

using FluentValidation.TestHelper;

using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Sales.UpdateSale;

/// <summary>
/// Contains unit tests for the UpdateSaleItemCommandValidator class.
/// </summary>
public class UpdateSaleItemCommandValidatorTests
{
    private readonly UpdateSaleItemCommandValidator _validator = new();

    [Fact(DisplayName = "Should have error when ProductId is less than or equal to zero")]
    public void Given_InvalidProductId_When_Validated_Then_ShouldHaveError()
    {
        // Arrange
        var item = new UpdateSaleItemCommand { ProductId = 0 };

        // Act & Assert
        var result = _validator.TestValidate(item);
        result.ShouldHaveValidationErrorFor(i => i.ProductId).WithErrorMessage("Product ID must be greater than 0.");
    }

    [Fact(DisplayName = "Should have error when ProductName is empty or out of length bounds")]
    public void Given_InvalidProductName_When_Validated_Then_ShouldHaveError()
    {
        // Arrange
        var item = new UpdateSaleItemCommand { ProductName = "" };

        // Act & Assert
        var result = _validator.TestValidate(item);
        result.ShouldHaveValidationErrorFor(i => i.ProductName).WithErrorMessage("Product name must be between 3 and 100 characters.");
    }

    [Fact(DisplayName = "Should have error when Quantity is out of bounds")]
    public void Given_InvalidQuantity_When_Validated_Then_ShouldHaveError()
    {
        // Arrange
        var item = new UpdateSaleItemCommand { Quantity = 0 };

        // Act & Assert
        var result = _validator.TestValidate(item);
        result.ShouldHaveValidationErrorFor(i => i.Quantity).WithErrorMessage("Quantity must be between 1 and 20.");
    }

    [Fact(DisplayName = "Should have error when UnitPrice is less than or equal to zero")]
    public void Given_InvalidUnitPrice_When_Validated_Then_ShouldHaveError()
    {
        // Arrange
        var item = new UpdateSaleItemCommand { UnitPrice = 0m };

        // Act & Assert
        var result = _validator.TestValidate(item);
        result.ShouldHaveValidationErrorFor(i => i.UnitPrice).WithErrorMessage("Unit price must be greater than 0.");
    }
}

