using Ambev.DeveloperEvaluation.Application.Sales.GetSale;

using FluentValidation.TestHelper;

using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Sales.GetSale;

/// <summary>
/// Contains unit tests for the GetSaleValidator class.
/// </summary>
public class GetSaleValidatorTests
{
    private readonly GetSaleValidator _validator = new();

    [Fact(DisplayName = "Should have error when Sale ID is empty")]
    public void Given_EmptySaleId_When_Validated_Then_ShouldHaveError()
    {
        // Arrange
        var query = new GetSaleQuery(Guid.Empty);

        // Act & Assert
        var result = _validator.TestValidate(query);
        result.ShouldHaveValidationErrorFor(q => q.Id).WithErrorMessage("Sale ID is required");
    }

    [Fact(DisplayName = "Should not have error when Sale ID is valid")]
    public void Given_ValidSaleId_When_Validated_Then_ShouldNotHaveError()
    {
        // Arrange
        var query = new GetSaleQuery(Guid.NewGuid());

        // Act & Assert
        var result = _validator.TestValidate(query);
        result.ShouldNotHaveValidationErrorFor(q => q.Id);
    }
}