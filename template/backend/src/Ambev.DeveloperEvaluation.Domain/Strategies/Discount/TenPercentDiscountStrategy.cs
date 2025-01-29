﻿namespace Ambev.DeveloperEvaluation.Domain.Strategies.Discount;

public class TenPercentDiscountStrategy : IDiscountStrategy
{
    private const decimal DiscountRate = 0.10m;

    public decimal CalculateDiscount(int quantity, decimal unitPrice)
    {
        return quantity * unitPrice * DiscountRate;
    }
}
