using Ambev.DeveloperEvaluation.Common.Validation;
using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Validation;

using System.ComponentModel.DataAnnotations.Schema;

namespace Ambev.DeveloperEvaluation.Domain.Entities;

/// <summary>
/// Represents a sale in the system with its details and items.
/// This entity follows domain-driven design principles and includes business rules validation.
/// </summary>
public class Sale : BaseEntity
{
    /// <summary>
    /// Gets or sets the sale number.
    /// </summary>
    public string SaleNumber { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the date when the sale was made.
    /// </summary>
    [Column(TypeName = "timestamp with time zone")]
    public DateTime SaleDate { get; set; }

    /// <summary>
    /// Gets or sets the customer ID (external identity).
    /// </summary>
    public int CustomerId { get; set; }

    /// <summary>
    /// Gets or sets the customer name (denormalized).
    /// </summary>
    public string CustomerName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the total sale amount.
    /// </summary>
    public decimal TotalAmount { get; set; }

    /// <summary>
    /// Gets or sets the total sale amount before discounts.
    /// </summary>
    public decimal TotalAmountBeforeDiscount { get; set; }

    /// <summary>
    /// Gets or sets the branch ID (external identity).
    /// </summary>
    public int BranchId { get; set; }

    /// <summary>
    /// Gets or sets the branch name (denormalized).
    /// </summary>
    public string BranchName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the list of items in the sale.
    /// </summary>
    public List<SaleItem> Items { get; set; } = new();

    /// <summary>
    /// Gets or sets whether the sale is cancelled.
    /// </summary>
    public bool IsCancelled { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the sale was created.
    /// </summary>
    [Column(TypeName = "timestamp with time zone")]
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Gets or sets the date and time of the last update to the sale's information.
    /// </summary>
    [Column(TypeName = "timestamp with time zone")]
    public DateTime? UpdatedAt { get; set; }

    /// <summary>
    /// Gets the total number of items in the sale.
    /// </summary>
    public int ItemCount => Items.Count;

    /// <summary>
    /// Initializes a new instance of the Sale class.
    /// </summary>
    public Sale()
    {
        CreatedAt = DateTime.UtcNow;
        SaleNumber = GenerateSaleNumber();
    }

    /// <summary>
    /// Generates a unique sale number.
    /// </summary>
    /// <returns>A unique sale number</returns>
    private static string GenerateSaleNumber()
    {
        return $"SALE-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString()[..8].ToUpper()}";
    }

    /// <summary>
    /// Performs validation of the sale entity using the SaleValidator rules.
    /// </summary>
    /// <returns>
    /// A <see cref="ValidationResultDetail"/> containing:
    /// - IsValid: Indicates whether all validation rules passed
    /// - Errors: Collection of validation errors if any rules failed
    /// </returns>
    public ValidationResultDetail Validate()
    {
        var validator = new SaleValidator();
        var result = validator.Validate(this);
        return new ValidationResultDetail
        {
            IsValid = result.IsValid,
            Errors = result.Errors.Select(o => (ValidationErrorDetail)o)
        };
    }

    /// <summary>
    /// Calculates the total amount of the sale based on the items and discounts.
    /// </summary>
    public void CalculateTotalAmount()
    {
        TotalAmountBeforeDiscount = Items.Sum(item => item.Quantity * item.UnitPrice);
        TotalAmount = Items.Sum(item => item.CalculateTotal());
    }

    /// <summary>
    /// Cancels the sale.
    /// </summary>
    public void Cancel()
    {
        IsCancelled = true;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Applies discounts to the items in the sale.
    /// </summary>
    public void ApplyDiscounts()
    {
        foreach (var item in Items)
        {
            item.ApplyDiscount();
        }
    }
}