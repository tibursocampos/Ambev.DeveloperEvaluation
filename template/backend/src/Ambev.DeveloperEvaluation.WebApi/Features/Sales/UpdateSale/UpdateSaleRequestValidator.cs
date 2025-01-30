using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.UpdateSale;

/// <summary>
/// Validator for UpdateSaleRequest that defines validation rules for updating a sale
/// </summary>
public class UpdateSaleRequestValidator : AbstractValidator<UpdateSaleRequest>
{
    /// <summary>
    /// Initializes a new instance of the UpdateSaleRequestValidator with defined validation rules
    /// </summary>
    public UpdateSaleRequestValidator()
    {
        RuleFor(request => request.Id)
            .NotEmpty().WithMessage("Sale ID is required.");

        RuleFor(request => request.SaleDate)
            .NotEmpty().WithMessage("Sale date is required.");

        RuleFor(request => request.CustomerId)
            .GreaterThan(0).WithMessage("Customer ID must be greater than 0.");

        RuleFor(request => request.CustomerName)
            .NotEmpty().Length(3, 100).WithMessage("Customer name must be between 3 and 100 characters.");

        RuleFor(request => request.BranchId)
            .GreaterThan(0).WithMessage("Branch ID must be greater than 0.");

        RuleFor(request => request.BranchName)
            .NotEmpty().Length(3, 100).WithMessage("Branch name must be between 3 and 100 characters.");

        RuleFor(request => request.Items)
            .NotEmpty().WithMessage("Sale must contain at least one item.");

        RuleForEach(request => request.Items).SetValidator(new UpdateSaleItemRequestValidator());
    }
}

/// <summary>
/// Validator for UpdateSaleItemRequest that defines validation rules for updating a sale item
/// </summary>
public class UpdateSaleItemRequestValidator : AbstractValidator<UpdateSaleItemRequest>
{
    /// <summary>
    /// Initializes a new instance of the UpdateSaleItemRequestValidator with defined validation rules
    /// </summary>
    public UpdateSaleItemRequestValidator()
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