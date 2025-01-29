using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale;

/// <summary>
/// Validator for CreateSaleCommand
/// </summary>
public class CreateSaleValidator : AbstractValidator<CreateSaleCommand>
{
    /// <summary>
    /// Initializes validation rules for CreateSaleCommand
    /// </summary>
    public CreateSaleValidator()
    {
        RuleFor(command => command.SaleDate)
            .NotEmpty().WithMessage("Sale date is required.");

        RuleFor(command => command.CustomerId)
            .GreaterThan(0).WithMessage("Customer ID must be greater than 0.");

        RuleFor(command => command.CustomerName)
            .NotEmpty().Length(3, 100).WithMessage("Customer name must be between 3 and 100 characters.");

        RuleFor(command => command.BranchId)
            .GreaterThan(0).WithMessage("Branch ID must be greater than 0.");

        RuleFor(command => command.BranchName)
            .NotEmpty().Length(3, 100).WithMessage("Branch name must be between 3 and 100 characters.");

        RuleFor(command => command.Items)
            .NotEmpty().WithMessage("Sale must contain at least one item.");

        RuleForEach(command => command.Items).SetValidator(new CreateSaleItemCommandValidator());
    }
}

/// <summary>
/// Validator for CreateSaleItemCommand that defines validation rules for creating a sale item
/// </summary>
public class CreateSaleItemCommandValidator : AbstractValidator<CreateSaleItemCommand>
{
    /// <summary>
    /// Initializes a new instance of the CreateSaleItemCommandValidator with defined validation rules
    /// </summary>
    public CreateSaleItemCommandValidator()
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