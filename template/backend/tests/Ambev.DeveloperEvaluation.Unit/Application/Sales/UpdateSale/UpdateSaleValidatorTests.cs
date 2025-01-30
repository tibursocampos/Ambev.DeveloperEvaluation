using Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;

using FluentValidation.TestHelper;

using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Sales.UpdateSale;

/// <summary>
/// Contains unit tests for the UpdateSaleValidator class.
/// </summary>
public class UpdateSaleValidatorTests
{
    private readonly UpdateSaleValidator _validator = new();

    [Fact(DisplayName = "Should have error when Sale ID is empty")]
    public void Given_EmptySaleId_When_Validated_Then_ShouldHaveError()
    {
        // Arrange
        var command = new UpdateSaleCommand { Id = Guid.Empty };

        // Act & Assert
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(c => c.Id).WithErrorMessage("Sale ID is required.");
    }

    [Fact(DisplayName = "Should have error when SaleDate is empty")]
    public void Given_EmptySaleDate_When_Validated_Then_ShouldHaveError()
    {
        // Arrange
        var command = new UpdateSaleCommand { SaleDate = default };

        // Act & Assert
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(c => c.SaleDate).WithErrorMessage("Sale date is required.");
    }

    [Fact(DisplayName = "Should have error when CustomerId is less than or equal to zero")]
    public void Given_InvalidCustomerId_When_Validated_Then_ShouldHaveError()
    {
        // Arrange
        var command = new UpdateSaleCommand { CustomerId = 0 };

        // Act & Assert
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(c => c.CustomerId).WithErrorMessage("Customer ID must be greater than 0.");
    }

    [Fact(DisplayName = "Should have error when CustomerName is empty or out of length bounds")]
    public void Given_InvalidCustomerName_When_Validated_Then_ShouldHaveError()
    {
        // Arrange
        var command = new UpdateSaleCommand { CustomerName = "" };

        // Act & Assert
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(c => c.CustomerName).WithErrorMessage("Customer name must be between 3 and 100 characters.");
    }

    [Fact(DisplayName = "Should have error when BranchId is less than or equal to zero")]
    public void Given_InvalidBranchId_When_Validated_Then_ShouldHaveError()
    {
        // Arrange
        var command = new UpdateSaleCommand { BranchId = 0 };

        // Act & Assert
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(c => c.BranchId).WithErrorMessage("Branch ID must be greater than 0.");
    }

    [Fact(DisplayName = "Should have error when BranchName is empty or out of length bounds")]
    public void Given_InvalidBranchName_When_Validated_Then_ShouldHaveError()
    {
        // Arrange
        var command = new UpdateSaleCommand { BranchName = "" };

        // Act & Assert
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(c => c.BranchName).WithErrorMessage("Branch name must be between 3 and 100 characters.");
    }

    [Fact(DisplayName = "Should have error when Items list is empty")]
    public void Given_EmptyItemsList_When_Validated_Then_ShouldHaveError()
    {
        // Arrange
        var command = new UpdateSaleCommand { Items = new List<UpdateSaleItemCommand>() };

        // Act & Assert
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(c => c.Items).WithErrorMessage("Sale must contain at least one item.");
    }
}

