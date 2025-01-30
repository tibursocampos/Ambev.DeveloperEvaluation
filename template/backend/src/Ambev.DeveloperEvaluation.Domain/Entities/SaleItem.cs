using Ambev.DeveloperEvaluation.Common.Validation;
using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Strategies.Discount;
using Ambev.DeveloperEvaluation.Domain.Validation;

namespace Ambev.DeveloperEvaluation.Domain.Entities;

/// <summary>
/// Represents a shopping sale items that a customer purchase.
/// This entity follows domain-driven design principles and includes business rules validation.
/// </summary>
public class SaleItem : BaseEntity
{
    private const int MinQuantityForDiscount = 4;
    private const int MaxQuantityForTenPercentDiscount = 9;
    private const int MaxQuantityForTwentyPercentDiscount = 20;

    private static readonly Dictionary<Func<int, bool>, IDiscountStrategy> DiscountStrategies = new()
    {
        { quantity => quantity < MinQuantityForDiscount, new NoDiscountStrategy() },
        { quantity => quantity >= MinQuantityForDiscount && quantity <= MaxQuantityForTenPercentDiscount, new TenPercentDiscountStrategy() },
        { quantity => quantity > MaxQuantityForTenPercentDiscount && quantity <= MaxQuantityForTwentyPercentDiscount, new TwentyPercentDiscountStrategy() }
    };

    /// <summary>
    /// Gets or sets the product ID.
    /// </summary>
    public int ProductId { get; set; }

    public string ProductName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the quantity of the product.
    /// </summary>
    public int Quantity { get; set; }

    /// <summary>
    /// Gets or sets the unit price of the product.
    /// </summary>
    public decimal UnitPrice { get; set; }

    /// <summary>
    /// Gets or sets the discount amount.
    /// </summary>
    public decimal Discount { get; private set; }

    /// <summary>
    /// Gets the total amount of the sale item.
    /// </summary>
    public decimal TotalAmount { get; private set; }

    /// <summary>
    /// Gets or sets the sale ID.
    /// </summary>
    public Guid SaleId { get; set; }

    /// <summary>
    /// Performs validation of the sale item entity using the SaleItemValidator rules.
    /// </summary>
    /// <returns>
    /// A <see cref="ValidationResultDetail"/> containing:
    /// - IsValid: Indicates whether all validation rules passed
    /// - Errors: Collection of validation errors if any rules failed
    /// </returns>
    public ValidationResultDetail Validate()
    {
        var validator = new SaleItemValidator();
        var result = validator.Validate(this);
        return new ValidationResultDetail
        {
            IsValid = result.IsValid,
            Errors = result.Errors.Select(o => (ValidationErrorDetail)o)
        };
    }

    /// <summary>
    /// Calculates the total amount of the sale item.
    /// </summary>
    /// <returns></returns>
    public decimal CalculateTotal()
    {
        TotalAmount = Quantity * UnitPrice - Discount;
        return TotalAmount;
    }

    /// <summary>
    /// Applies a discount to the sale item based on the quantity purchased.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown when the quantity exceeds the maximum allowed for discount.</exception>
    public void ApplyDiscount()
    {
        var strategy = DiscountStrategies.FirstOrDefault(pair => pair.Key(Quantity)).Value ?? throw new InvalidOperationException("Cannot sell more than 20 identical items.");
        Discount = strategy.CalculateDiscount(Quantity, UnitPrice);
    }
}