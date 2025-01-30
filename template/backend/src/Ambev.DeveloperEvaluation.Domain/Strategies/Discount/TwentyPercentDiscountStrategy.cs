namespace Ambev.DeveloperEvaluation.Domain.Strategies.Discount;

public class TwentyPercentDiscountStrategy : IDiscountStrategy
{
    private const decimal DiscountRate = 0.20m;

    public decimal CalculateDiscount(int quantity, decimal unitPrice)
    {
        return quantity * unitPrice * DiscountRate;
    }
}
