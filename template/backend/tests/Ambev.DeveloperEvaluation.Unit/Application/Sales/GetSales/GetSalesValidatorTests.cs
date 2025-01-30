using Ambev.DeveloperEvaluation.Application.Sales.GetSales;

using FluentValidation.TestHelper;

using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Sales.GetSales;

/// <summary>
/// Contains unit tests for the GetSalesValidator class.
/// </summary>
public class GetSalesValidatorTests
{
    private readonly GetSalesValidator _validator = new();

    [Fact(DisplayName = "Should have error when Page number is less than or equal to zero")]
    public void Given_InvalidPageNumber_When_Validated_Then_ShouldHaveError()
    {
        // Arrange
        var query = new GetSalesQuery { Page = 0, Size = 10, Order = "asc" };

        // Act & Assert
        var result = _validator.TestValidate(query);
        result.ShouldHaveValidationErrorFor(q => q.Page).WithErrorMessage("Page number must be greater than 0.");
    }

    [Fact(DisplayName = "Should have error when Page size is less than or equal to zero")]
    public void Given_InvalidPageSize_When_Validated_Then_ShouldHaveError()
    {
        // Arrange
        var query = new GetSalesQuery { Page = 1, Size = 0, Order = "asc" };

        // Act & Assert
        var result = _validator.TestValidate(query);
        result.ShouldHaveValidationErrorFor(q => q.Size).WithErrorMessage("Page size must be greater than 0.");
    }

    [Fact(DisplayName = "Should have error when Order is not 'asc' or 'desc'")]
    public void Given_InvalidOrder_When_Validated_Then_ShouldHaveError()
    {
        // Arrange
        var query = new GetSalesQuery { Page = 1, Size = 10, Order = "invalid" };

        // Act & Assert
        var result = _validator.TestValidate(query);
        result.ShouldHaveValidationErrorFor(q => q.Order).WithErrorMessage("Order must be 'asc' or 'desc'.");
    }

    [Fact(DisplayName = "Should not have error when Page number, Page size, and Order are valid")]
    public void Given_ValidQuery_When_Validated_Then_ShouldNotHaveError()
    {
        // Arrange
        var query = new GetSalesQuery { Page = 1, Size = 10, Order = "asc" };

        // Act & Assert
        var result = _validator.TestValidate(query);
        result.ShouldNotHaveValidationErrorFor(q => q.Page);
        result.ShouldNotHaveValidationErrorFor(q => q.Size);
        result.ShouldNotHaveValidationErrorFor(q => q.Order);
    }
}