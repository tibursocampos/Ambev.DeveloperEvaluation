using Ambev.DeveloperEvaluation.Application.Sales.GetSales;
using Ambev.DeveloperEvaluation.Domain.Entities;

using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Application.TestData;

public static class GetSalesTestData
{
    public static GetSalesQuery GenerateValidGetSalesQuery()
    {
        return new Faker<GetSalesQuery>()
            .RuleFor(q => q.Page, f => f.Random.Int(1, 10))
            .RuleFor(q => q.Size, f => f.Random.Int(1, 100))
            .RuleFor(q => q.Order, f => f.PickRandom(new[] { "asc", "desc" }))
            .RuleFor(q => q.Filters, f => new Dictionary<string, string> { { "CustomerName", f.Person.FullName } });
    }

    public static GetSalesQuery GenerateInvalidGetSalesQuery()
    {
        return new GetSalesQuery { Page = 0, Size = 0, Order = "invalid" };
    }

    public static List<Sale> GenerateValidSales(int count = 10)
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
            .RuleFor(s => s.IsCancelled, f => f.Random.Bool())
            .Generate(count);
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



