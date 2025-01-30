using Ambev.DeveloperEvaluation.Domain.Entities;

using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;

public static class SaleItemTestData
{
    public static List<SaleItem> GenerateValidSaleItem(int qtde = 1) => new Faker<SaleItem>()
            .RuleFor(s => s.ProductId, f => f.Random.Number(1, 100))
            .RuleFor(s => s.ProductName, f => f.Commerce.ProductName())
            .RuleFor(s => s.Quantity, f => f.Random.Number(1, 20))
            .RuleFor(s => s.UnitPrice, f => f.Random.Decimal(10, 100))
            .Generate(qtde);

    public static List<SaleItem> GenerateInvalidSaleItem(int qtde = 1) => new Faker<SaleItem>()
        .RuleFor(s => s.ProductId, f => f.Random.Number(1, 100))
        .RuleFor(s => s.ProductName, f => f.Commerce.ProductName())
        .RuleFor(s => s.Quantity, f => f.Random.Number(21, 30))
        .RuleFor(s => s.UnitPrice, f => f.Random.Decimal(10, 100))
        .Generate(qtde);
}
