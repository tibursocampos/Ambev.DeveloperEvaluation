using Ambev.DeveloperEvaluation.Application.Sales.GetSale;
using Ambev.DeveloperEvaluation.Domain.Entities;

using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Application.TestData;

public static class GetSaleTestData
{
    public static GetSaleQuery GenerateValidGetSaleQuery()
    {
        return new Faker<GetSaleQuery>()
            .CustomInstantiator(f => new GetSaleQuery(Guid.NewGuid()));
    }

    public static GetSaleQuery GenerateInvalidGetSaleQuery()
    {
        return new GetSaleQuery(Guid.Empty);
    }

    public static Sale GenerateValidSale()
    {
        return new Faker<Sale>()
            .RuleFor(s => s.Id, f => Guid.NewGuid())
            .RuleFor(s => s.SaleNumber, f => f.Commerce.Ean8())
            .RuleFor(s => s.SaleDate, f => f.Date.Past())
            .RuleFor(s => s.CustomerId, f => f.Random.Int(1, 1000))
            .RuleFor(s => s.CustomerName, f => f.Person.FullName)
            .RuleFor(s => s.BranchId, f => f.Random.Int(1, 100))
            .RuleFor(s => s.BranchName, f => f.Company.CompanyName())
            .RuleFor(s => s.Items, f => GenerateValidSaleItems())
            .RuleFor(s => s.IsCancelled, f => f.Random.Bool());
    }

    public static List<SaleItem> GenerateValidSaleItems()
    {
        return new Faker<SaleItem>()
            .RuleFor(i => i.Id, f => Guid.NewGuid())
            .RuleFor(i => i.ProductId, f => f.Random.Int(1, 1000))
            .RuleFor(i => i.ProductName, f => f.Commerce.ProductName())
            .RuleFor(i => i.Quantity, f => f.Random.Int(1, 20))
            .RuleFor(i => i.UnitPrice, f => f.Finance.Amount(1, 100))
            .Generate(3);
    }
}