namespace Ambev.DeveloperEvaluation.Domain.Strategies.Discount;

public class NoDiscountStrategy : IDiscountStrategy
{
    public decimal CalculateDiscount(int quantity, decimal unitPrice)
    {
        return 0;
    }
}
