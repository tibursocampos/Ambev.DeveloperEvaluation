using Ambev.DeveloperEvaluation.Domain.Entities;

using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;

public static class SaleTestData
{
    public static Sale GenerateValidSale()
    {
        return new Faker<Sale>()
            .RuleFor(s => s.SaleNumber, f => f.Random.AlphaNumeric(6))
            .RuleFor(s => s.SaleDate, f => f.Date.Past())
            .RuleFor(s => s.CustomerId, f => f.Random.Number(1, 100))
            .RuleFor(s => s.CustomerName, f => f.Person.FullName)
            .RuleFor(s => s.BranchId, f => f.Random.Number(1, 100))
            .RuleFor(s => s.BranchName, f => f.Company.CompanyName())
            .RuleFor(s => s.Items, f => SaleItemTestData.GenerateValidSaleItem(5))
            .RuleFor(s => s.IsCancelled, f => f.Random.Bool())
            .Generate();
    }
}
