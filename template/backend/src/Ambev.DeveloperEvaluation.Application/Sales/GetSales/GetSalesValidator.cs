using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetSales;

/// <summary>
/// Validator for GetSalesQuery
/// </summary>
public class GetSalesValidator : AbstractValidator<GetSalesQuery>
{
    /// <summary>
    /// Initializes validation rules for GetSalesQuery
    /// </summary>
    public GetSalesValidator()
    {
        RuleFor(x => x.Page)
            .GreaterThan(0)
            .WithMessage("Page number must be greater than 0.");

        RuleFor(x => x.Size)
            .GreaterThan(0)
            .WithMessage("Page size must be greater than 0.");

        RuleFor(x => x.Order)
            .Must(order => order == "asc" || order == "desc")
            .WithMessage("Order must be 'asc' or 'desc'.");
    }
}