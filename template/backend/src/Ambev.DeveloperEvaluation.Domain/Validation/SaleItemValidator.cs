using Ambev.DeveloperEvaluation.Domain.Entities;

using FluentValidation;

namespace Ambev.DeveloperEvaluation.Domain.Validation;

public class SaleItemValidator : AbstractValidator<SaleItem>
{
    public SaleItemValidator()
    {
        RuleFor(item => item.ProductId)
            .GreaterThan(0).WithMessage("Product ID must be greater than 0.");

        RuleFor(item => item.ProductName)
            .NotEmpty().Length(3, 100).WithMessage("Product name must be between 3 and 100 characters.");

        RuleFor(item => item.Quantity)
            .InclusiveBetween(1, 20).WithMessage("Quantity must be between 1 and 20.");

        RuleFor(item => item.UnitPrice)
            .GreaterThan(0).WithMessage("Unit price must be greater than 0.");
    }
}
