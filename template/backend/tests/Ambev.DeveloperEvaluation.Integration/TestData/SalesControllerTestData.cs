using Bogus;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.UpdateSale;

namespace Ambev.DeveloperEvaluation.Integration.TestData;

/// <summary>
/// Provides test data for SalesController integration tests
/// </summary>
public static class SalesControllerTestData
{
    /// <summary>
    /// Faker instance for generating valid sale item requests
    /// </summary>
    public static readonly Faker<CreateSaleItemRequest> CreateSaleItemFaker = new Faker<CreateSaleItemRequest>()
        .RuleFor(i => i.ProductId, f => f.Random.Int(1, 100))
        .RuleFor(i => i.ProductName, f => f.Commerce.ProductName())
        .RuleFor(i => i.Quantity, f => f.Random.Int(1, 20))
        .RuleFor(i => i.UnitPrice, f => f.Finance.Amount(10, 100));

    /// <summary>
    /// Faker instance for generating valid sale creation requests
    /// </summary>
    public static readonly Faker<CreateSaleRequest> CreateSaleFaker = new Faker<CreateSaleRequest>()
        .RuleFor(s => s.SaleDate, f => f.Date.Recent())
        .RuleFor(s => s.CustomerId, f => f.Random.Int(10, 100))
        .RuleFor(s => s.CustomerName, f => f.Person.FullName)
        .RuleFor(s => s.BranchId, f => f.Random.Int(10, 100))
        .RuleFor(s => s.BranchName, f => f.Company.CompanyName())
        .RuleFor(s => s.Items, f => CreateSaleItemFaker.Generate(3));

    private static readonly Faker<UpdateSaleItemRequest> UpdateSaleItemFaker = new Faker<UpdateSaleItemRequest>()
        .RuleFor(i => i.ProductId, f => f.Random.Int(1, 100))
        .RuleFor(i => i.ProductName, f => f.Commerce.ProductName())
        .RuleFor(i => i.Quantity, f => f.Random.Int(1, 10))
        .RuleFor(i => i.UnitPrice, f => f.Finance.Amount(10, 100));

    private static readonly Faker<UpdateSaleRequest> UpdateSaleFaker = new Faker<UpdateSaleRequest>()
        .RuleFor(s => s.Id, f => f.Random.Guid())
        .RuleFor(s => s.SaleDate, f => f.Date.Recent())
        .RuleFor(s => s.CustomerId, f => f.Random.Int(10, 100))
        .RuleFor(s => s.CustomerName, f => f.Person.FullName)
        .RuleFor(s => s.BranchId, f => f.Random.Int(10, 100))
        .RuleFor(s => s.BranchName, f => f.Company.CompanyName())
        .RuleFor(s => s.Items, f => UpdateSaleItemFaker.Generate(3))
        .RuleFor(s => s.IsCancelled, f => f.Random.Bool());

    /// <summary>
    /// Generates test cases for sale creation endpoint
    /// </summary>
    /// <returns>
    /// IEnumerable of test cases containing:
    /// - CreateSaleRequest: Request payload
    /// - bool: Expected success status
    /// </returns>
    public static IEnumerable<object[]> CreateSaleData()
    {
        yield return new object[] { CreateSaleFaker.Generate(), true };
        yield return new object[] { CreateSaleFaker.Clone().RuleFor(s => s.CustomerId, 0).Generate(), false };
    }

    /// <summary>
    /// Generates test cases for sale update endpoint
    /// </summary>
    /// <returns>
    /// IEnumerable of test cases containing:
    /// - UpdateSaleRequest: Request payload
    /// - bool: Expected success status
    /// </returns>
    public static IEnumerable<object[]> UpdateSaleData()
    {
        yield return new object[] { UpdateSaleFaker.Generate(), true };
        yield return new object[] { UpdateSaleFaker.Generate(), false };
    }

    /// <summary>
    /// Generates test cases for sale retrieval endpoint
    /// </summary>
    /// <returns>
    /// IEnumerable of test cases containing:
    /// - Guid: Sale ID to retrieve
    /// - bool: Expected success status
    /// </returns>
    public static IEnumerable<object[]> GetSaleData()
    {
        yield return new object[] { Guid.NewGuid(), false };
        yield return new object[] { Guid.Empty, false };
    }

    /// <summary>
    /// Generates test cases for sales list endpoint
    /// </summary>
    /// <returns>
    /// IEnumerable of test cases containing pagination parameters:
    /// - int: Page number
    /// - int: Page size
    /// - string: Sort order
    /// - string: Filter criteria
    /// </returns>
    public static IEnumerable<object[]> GetSalesData()
    {
        yield return new object[] { 1, 10, "asc", null };
        yield return new object[] { 2, 10, "asc", null };
    }
}