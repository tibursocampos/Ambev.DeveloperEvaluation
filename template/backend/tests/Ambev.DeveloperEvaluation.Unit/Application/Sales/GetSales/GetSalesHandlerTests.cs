using Ambev.DeveloperEvaluation.Application.Common;
using Ambev.DeveloperEvaluation.Application.Sales.GetSales;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Unit.Application.TestData;

using AutoMapper;

using FluentValidation;

using Microsoft.Extensions.Logging;

using Moq;

using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Sales.GetSales;

/// <summary>
/// Contains unit tests for the GetSalesHandler class.
/// </summary>
public class GetSalesHandlerTests
{
    private readonly Mock<ISaleRepository> _saleRepositoryMock = new();
    private readonly Mock<IMapper> _mapperMock = new();
    private readonly Mock<ILogger<GetSalesHandler>> _loggerMock = new();
    private readonly GetSalesHandler _handler;

    public GetSalesHandlerTests()
    {
        _handler = new GetSalesHandler(_saleRepositoryMock.Object, _mapperMock.Object, _loggerMock.Object);
    }

    [Fact(DisplayName = "Should return paginated list of sales successfully")]
    public async Task Given_ValidQuery_When_Handled_Then_ShouldReturnPaginatedListOfSalesSuccessfully()
    {
        // Arrange
        var query = GetSalesTestData.GenerateValidGetSalesQuery();
        var sales = GetSalesTestData.GenerateValidSales(query.Size);

        var salesResult = sales.Select(s => new GetSalesResult
        {
            Id = s.Id,
            SaleNumber = s.SaleNumber,
            SaleDate = s.SaleDate,
            CustomerId = s.CustomerId,
            CustomerName = s.CustomerName,
            BranchId = s.BranchId,
            BranchName = s.BranchName,
            TotalAmount = s.TotalAmount,
            IsCancelled = s.IsCancelled
        }).ToList();

        _mapperMock.Setup(m => m.Map<List<GetSalesResult>>(sales)).Returns(salesResult);
        _saleRepositoryMock.Setup(r => r.GetAllAsync(query.Page, query.Size, query.Order, query.Filters, It.IsAny<CancellationToken>())).ReturnsAsync(sales);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(query.Page, result.CurrentPage);
        Assert.Equal(query.Size, result.PageSize);
        Assert.Equal(salesResult.Count, result.TotalCount);
        _saleRepositoryMock.Verify(r => r.GetAllAsync(query.Page, query.Size, query.Order, query.Filters, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact(DisplayName = "Should throw validation exception for invalid query")]
    public async Task Given_InvalidQuery_When_Handled_Then_ShouldThrowValidationException()
    {
        // Arrange
        var query = GetSalesTestData.GenerateInvalidGetSalesQuery();

        // Act & Assert
        await Assert.ThrowsAsync<ValidationException>(() => _handler.Handle(query, CancellationToken.None));
    }

    [Fact(DisplayName = "Should return empty paginated list if no sales found")]
    public async Task Given_NoSalesFound_When_Handled_Then_ShouldReturnEmptyPaginatedList()
    {
        // Arrange
        var query = GetSalesTestData.GenerateValidGetSalesQuery();
        var sales = new List<Sale>();

        _mapperMock.Setup(m => m.Map<List<GetSalesResult>>(sales)).Returns(new List<GetSalesResult>());
        _saleRepositoryMock.Setup(r => r.GetAllAsync(query.Page, query.Size, query.Order, query.Filters, It.IsAny<CancellationToken>())).ReturnsAsync(sales);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
        Assert.Equal(query.Page, result.CurrentPage);
        Assert.Equal(query.Size, result.PageSize);
        Assert.Equal(0, result.TotalCount);
        _saleRepositoryMock.Verify(r => r.GetAllAsync(query.Page, query.Size, query.Order, query.Filters, It.IsAny<CancellationToken>()), Times.Once);
    }
}