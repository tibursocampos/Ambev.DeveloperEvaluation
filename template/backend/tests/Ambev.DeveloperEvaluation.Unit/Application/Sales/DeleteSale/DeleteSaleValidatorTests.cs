using Ambev.DeveloperEvaluation.Application.Sales.DeleteSale;

using FluentValidation.TestHelper;

using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Sales.DeleteSale;

/// <summary>
/// Contains unit tests for the DeleteSaleValidator class.
/// </summary>
public class DeleteSaleValidatorTests
{
    private readonly DeleteSaleValidator _validator = new();

    [Fact(DisplayName = "Should have error when Sale ID is empty")]
    public void Given_EmptySaleId_When_Validated_Then_ShouldHaveError()
    {
        // Arrange
        var command = new DeleteSaleCommand(Guid.Empty);

        // Act & Assert
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(c => c.Id).WithErrorMessage("Sale ID is required.");
    }

    [Fact(DisplayName = "Should not have error when Sale ID is valid")]
    public void Given_ValidSaleId_When_Validated_Then_ShouldNotHaveError()
    {
        // Arrange
        var command = new DeleteSaleCommand(Guid.NewGuid());

        // Act & Assert
        var result = _validator.TestValidate(command);
        result.ShouldNotHaveValidationErrorFor(c => c.Id);
    }
}