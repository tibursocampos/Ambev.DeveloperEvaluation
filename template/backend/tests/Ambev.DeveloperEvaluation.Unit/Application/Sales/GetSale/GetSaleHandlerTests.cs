using Ambev.DeveloperEvaluation.Application.Sales.GetSale;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Unit.Application.TestData;

using AutoMapper;

using FluentValidation;

using Microsoft.Extensions.Logging;

using Moq;

using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Sales.GetSale;

/// <summary>
/// Contains unit tests for the GetSaleHandler class.
/// </summary>
public class GetSaleHandlerTests
{
    private readonly Mock<ISaleRepository> _saleRepositoryMock = new();
    private readonly Mock<IMapper> _mapperMock = new();
    private readonly Mock<ILogger<GetSaleHandler>> _loggerMock = new();
    private readonly GetSaleHandler _handler;

    public GetSaleHandlerTests()
    {
        _handler = new GetSaleHandler(_saleRepositoryMock.Object, _mapperMock.Object, _loggerMock.Object);
    }

    [Fact(DisplayName = "Should return sale details successfully")]
    public async Task Given_ValidQuery_When_Handled_Then_ShouldReturnSaleDetailsSuccessfully()
    {
        // Arrange
        var query = GetSaleTestData.GenerateValidGetSaleQuery();
        var sale = GetSaleTestData.GenerateValidSale();
        sale.Id = query.Id;

        var getSaleResult = new GetSaleResult
        {
            Id = sale.Id,
            SaleNumber = sale.SaleNumber,
            SaleDate = sale.SaleDate,
            CustomerId = sale.CustomerId,
            CustomerName = sale.CustomerName,
            BranchId = sale.BranchId,
            BranchName = sale.BranchName,
            Items = sale.Items.ConvertAll(i => new GetSaleItemResult
            {
                Id = i.Id,
                ProductId = i.ProductId,
                ProductName = i.ProductName,
                Quantity = i.Quantity,
                UnitPrice = i.UnitPrice,
                Discount = i.Discount,
                TotalAmount = i.TotalAmount
            }),
            IsCancelled = sale.IsCancelled
        };

        _mapperMock.Setup(m => m.Map<GetSaleResult>(sale)).Returns(getSaleResult);
        _saleRepositoryMock.Setup(r => r.GetByIdAsync(query.Id, It.IsAny<CancellationToken>())).ReturnsAsync(sale);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(getSaleResult, result);
        _saleRepositoryMock.Verify(r => r.GetByIdAsync(query.Id, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact(DisplayName = "Should throw validation exception for invalid query")]
    public async Task Given_InvalidQuery_When_Handled_Then_ShouldThrowValidationException()
    {
        // Arrange
        var query = GetSaleTestData.GenerateInvalidGetSaleQuery();

        // Act & Assert
        await Assert.ThrowsAsync<ValidationException>(() => _handler.Handle(query, CancellationToken.None));
    }

    [Fact(DisplayName = "Should return null if sale not found")]
    public async Task Given_SaleNotFound_When_Handled_Then_ShouldReturnNull()
    {
        // Arrange
        var query = GetSaleTestData.GenerateValidGetSaleQuery();

        _saleRepositoryMock.Setup(r => r.GetByIdAsync(query.Id, It.IsAny<CancellationToken>())).ReturnsAsync((Sale?)null);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Null(result);
        _saleRepositoryMock.Verify(r => r.GetByIdAsync(query.Id, It.IsAny<CancellationToken>()), Times.Once);
    }
}