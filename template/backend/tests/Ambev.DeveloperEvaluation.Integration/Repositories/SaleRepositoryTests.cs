using Ambev.DeveloperEvaluation.Integration.TestData;
using Ambev.DeveloperEvaluation.ORM;
using Ambev.DeveloperEvaluation.ORM.Repositories;

using Microsoft.EntityFrameworkCore;

using Xunit;

namespace Ambev.DeveloperEvaluation.Integration.Repositories;

/// <summary>
/// Tests for SaleRepository
/// </summary>
public class SaleRepositoryTests
{
    /// <summary>
    /// The options for the in-memory database context.
    /// </summary>
    private readonly DbContextOptions<DefaultContext> _dbContextOptions;

    /// <summary>
    /// Initializes a new instance of the <see cref="SaleRepositoryTests"/> class.
    /// </summary>
    public SaleRepositoryTests()
    {
        _dbContextOptions = new DbContextOptionsBuilder<DefaultContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;
    }

    [Fact(DisplayName = "Should add sale to database when valid sale is provided")]
    public async Task GivenValidSale_WhenCreateAsyncCalled_ThenAddsSaleToDatabase()
    {
        // Arrange
        using var context = new DefaultContext(_dbContextOptions);
        var _sut = new SaleRepository(context);
        var newSale = SaleRepositoryTestData.GenerateSale();

        // Act
        var createdSale = await _sut.CreateAsync(newSale);

        // Assert
        Assert.NotNull(createdSale);
        Assert.Equal(newSale.Id, createdSale.Id);
    }

    [Fact(DisplayName = "Should return sale when existing ID is provided")]
    public async Task GivenExistingSaleId_WhenGetByIdAsyncCalled_ThenReturnsSale()
    {
        // Arrange
        using var context = new DefaultContext(_dbContextOptions);
        var _sut = new SaleRepository(context);
        var existingSale = SaleRepositoryTestData.GenerateSale();
        await context.Sales.AddAsync(existingSale);
        await context.SaveChangesAsync();

        // Act
        var retrievedSale = await _sut.GetByIdAsync(existingSale.Id);

        // Assert
        Assert.NotNull(retrievedSale);
        Assert.Equal(existingSale.Id, retrievedSale.Id);
    }

    [Fact(DisplayName = "Should update sale details when sale exists")]
    public async Task GivenExistingSale_WhenUpdateAsyncCalled_ThenUpdatesSaleDetails()
    {
        // Arrange
        using var context = new DefaultContext(_dbContextOptions);
        var _sut = new SaleRepository(context);
        var originalSale = SaleRepositoryTestData.GenerateSale();
        await context.Sales.AddAsync(originalSale);
        await context.SaveChangesAsync();

        // Act
        originalSale.CustomerName = "Updated Customer";
        var updateResult = await _sut.UpdateAsync(originalSale);

        // Assert
        Assert.True(updateResult);
        var updatedSale = await _sut.GetByIdAsync(originalSale.Id);
        Assert.Equal("Updated Customer", updatedSale?.CustomerName);
    }

    [Fact(DisplayName = "Should cancel sale when existing ID is provided")]
    public async Task GivenExistingSaleId_WhenDeleteAsyncCalled_ThenCancelsSale()
    {
        // Arrange
        using var context = new DefaultContext(_dbContextOptions);
        var _sut = new SaleRepository(context);
        var saleToCancel = SaleRepositoryTestData.GenerateSale();
        await context.Sales.AddAsync(saleToCancel);
        await context.SaveChangesAsync();

        // Act
        var deleteResult = await _sut.DeleteAsync(saleToCancel.Id);

        // Assert
        Assert.True(deleteResult);
        var cancelledSale = await _sut.GetByIdAsync(saleToCancel.Id);
        Assert.NotNull(cancelledSale);
        Assert.True(cancelledSale?.IsCancelled);
    }

    [Fact(DisplayName = "Should return paginated sales list with specified size")]
    public async Task GivenSalesExist_WhenGetAllAsyncCalled_ThenReturnsPaginatedSales()
    {
        // Arrange
        using var context = new DefaultContext(_dbContextOptions);
        var _sut = new SaleRepository(context);
        for (var i = 0; i < 20; i++)
        {
            var sale = SaleRepositoryTestData.GenerateSale();
            await context.Sales.AddAsync(sale);
        }
        await context.SaveChangesAsync();

        // Act
        var paginatedSales = await _sut.GetAllAsync(page: 2, size: 5);

        // Assert
        Assert.Equal(5, paginatedSales.Count());
    }
}