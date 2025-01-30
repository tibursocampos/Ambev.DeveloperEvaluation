using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Unit.Application.TestData;

using FluentValidation.TestHelper;

using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Sales.CreateSale;

/// <summary>
/// Contains unit tests for the CreateSaleValidator class.
/// </summary>
public class CreateSaleValidatorTests
{
    private readonly CreateSaleValidator _validator = new();

    [Fact(DisplayName = "Should have error when SaleDate is empty")]
    public void Given_EmptySaleDate_When_Validated_Then_ShouldHaveError()
    {
        // Arrange
        var command = new CreateSaleCommand { SaleDate = default };

        // Act & Assert
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(c => c.SaleDate).WithErrorMessage("Sale date is required.");
    }

    [Fact(DisplayName = "Should have error when CustomerId is less than or equal to zero")]
    public void Given_InvalidCustomerId_When_Validated_Then_ShouldHaveError()
    {
        // Arrange
        var command = new CreateSaleCommand { CustomerId = 0 };

        // Act & Assert
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(c => c.CustomerId).WithErrorMessage("Customer ID must be greater than 0.");
    }

    [Fact(DisplayName = "Should have error when CustomerName is empty or out of length bounds")]
    public void Given_InvalidCustomerName_When_Validated_Then_ShouldHaveError()
    {
        // Arrange
        var command = new CreateSaleCommand { CustomerName = "" };

        // Act & Assert
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(c => c.CustomerName).WithErrorMessage("Customer name must be between 3 and 100 characters.");
    }

    [Fact(DisplayName = "Should have error when BranchId is less than or equal to zero")]
    public void Given_InvalidBranchId_When_Validated_Then_ShouldHaveError()
    {
        // Arrange
        var command = new CreateSaleCommand { BranchId = 0 };

        // Act & Assert
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(c => c.BranchId).WithErrorMessage("Branch ID must be greater than 0.");
    }

    [Fact(DisplayName = "Should have error when BranchName is empty or out of length bounds")]
    public void Given_InvalidBranchName_When_Validated_Then_ShouldHaveError()
    {
        // Arrange
        var command = new CreateSaleCommand { BranchName = "" };

        // Act & Assert
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(c => c.BranchName).WithErrorMessage("Branch name must be between 3 and 100 characters.");
    }

    [Fact(DisplayName = "Should have error when Items list is empty")]
    public void Given_EmptyItemsList_When_Validated_Then_ShouldHaveError()
    {
        // Arrange
        var command = new CreateSaleCommand { Items = [] };

        // Act & Assert
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(c => c.Items).WithErrorMessage("Sale must contain at least one item.");
    }

    [Fact(DisplayName = "Should not have error when all fields are valid")]
    public void Given_ValidCommand_When_Validated_Then_ShouldNotHaveError()
    {
        // Arrange
        var command = CreateSaleTestData.GenerateValidCreateSaleCommand();

        // Act & Assert
        var result = _validator.TestValidate(command);
        result.ShouldNotHaveValidationErrorFor(c => c.SaleDate);
        result.ShouldNotHaveValidationErrorFor(c => c.CustomerId);
        result.ShouldNotHaveValidationErrorFor(c => c.CustomerName);
        result.ShouldNotHaveValidationErrorFor(c => c.BranchId);
        result.ShouldNotHaveValidationErrorFor(c => c.BranchName);
        result.ShouldNotHaveValidationErrorFor(c => c.Items);
    }
}