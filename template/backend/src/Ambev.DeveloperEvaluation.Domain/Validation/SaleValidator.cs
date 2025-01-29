using Ambev.DeveloperEvaluation.Domain.Entities;

using FluentValidation;

namespace Ambev.DeveloperEvaluation.Domain.Validation;

public class SaleValidator : AbstractValidator<Sale>
{
    public SaleValidator()
    {
        RuleFor(s => s.SaleNumber)
            .NotEmpty().WithMessage("Sale number is required.");

        RuleFor(s => s.CustomerId)
            .GreaterThan(0).WithMessage("Customer ID must be greater than zero.");

        RuleFor(s => s.CustomerName)
            .NotEmpty().Length(3, 100).WithMessage("Customer name must be between 3 and 100 characters.");

        RuleFor(s => s.BranchId)
            .GreaterThan(0).WithMessage("Branch ID must be greater than zero.");

        RuleFor(s => s.BranchName)
            .NotEmpty().Length(3, 100).WithMessage("Branch name must be between 3 and 100 characters.");

        RuleFor(s => s.Items)
            .NotEmpty().WithMessage("Sale must have at least one item.");

        RuleForEach(s => s.Items).SetValidator(new SaleItemValidator());
    }
}
