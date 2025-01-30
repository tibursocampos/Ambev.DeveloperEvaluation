using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Validation;

using FluentValidation.TestHelper;

using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Validation;

/// <summary>
/// Contains unit tests for the SaleValidator class.
/// </summary>
public class SaleValidatorTests
{
    private readonly SaleValidator _validator = new();

    [Fact(DisplayName = "Should have error when SaleNumber is empty")]
    public void Given_EmptySaleNumber_When_Validated_Then_ShouldHaveError()
    {
        // Arrange
        var sale = new Sale { SaleNumber = "" };

        // Act & Assert
        var result = _validator.TestValidate(sale);
        result.ShouldHaveValidationErrorFor(s => s.SaleNumber).WithErrorMessage("Sale number is required.");
    }

    [Fact(DisplayName = "Should have error when CustomerId is less than or equal to zero")]
    public void Given_InvalidCustomerId_When_Validated_Then_ShouldHaveError()
    {
        // Arrange
        var sale = new Sale { CustomerId = 0 };

        // Act & Assert
        var result = _validator.TestValidate(sale);
        result.ShouldHaveValidationErrorFor(s => s.CustomerId).WithErrorMessage("Customer ID must be greater than zero.");
    }

    [Fact(DisplayName = "Should have error when CustomerName is empty or out of length bounds")]
    public void Given_InvalidCustomerName_When_Validated_Then_ShouldHaveError()
    {
        // Arrange
        var sale = new Sale { CustomerName = "" };

        // Act & Assert
        var result = _validator.TestValidate(sale);
        result.ShouldHaveValidationErrorFor(s => s.CustomerName).WithErrorMessage("Customer name must be between 3 and 100 characters.");
    }

    [Fact(DisplayName = "Should have error when BranchId is less than or equal to zero")]
    public void Given_InvalidBranchId_When_Validated_Then_ShouldHaveError()
    {
        // Arrange
        var sale = new Sale { BranchId = 0 };

        // Act & Assert
        var result = _validator.TestValidate(sale);
        result.ShouldHaveValidationErrorFor(s => s.BranchId).WithErrorMessage("Branch ID must be greater than zero.");
    }

    [Fact(DisplayName = "Should have error when BranchName is empty or out of length bounds")]
    public void Given_InvalidBranchName_When_Validated_Then_ShouldHaveError()
    {
        // Arrange
        var sale = new Sale { BranchName = "" };

        // Act & Assert
        var result = _validator.TestValidate(sale);
        result.ShouldHaveValidationErrorFor(s => s.BranchName).WithErrorMessage("Branch name must be between 3 and 100 characters.");
    }

    [Fact(DisplayName = "Should have error when Items list is empty")]
    public void Given_EmptyItemsList_When_Validated_Then_ShouldHaveError()
    {
        // Arrange
        var sale = new Sale { Items = new List<SaleItem>() };

        // Act & Assert
        var result = _validator.TestValidate(sale);
        result.ShouldHaveValidationErrorFor(s => s.Items).WithErrorMessage("Sale must have at least one item.");
    }
}