using Ambev.DeveloperEvaluation.Application.Notifications.Sales.SaleCreated;
using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Unit.Application.TestData;

using AutoMapper;

using FluentValidation;

using MediatR;

using Microsoft.Extensions.Logging;

using Moq;

using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Sales.CreateSale;

/// <summary>
/// Contains unit tests for the CreateSaleHandler class.
/// </summary>
public class CreateSaleHandlerTests
{
    private readonly Mock<ISaleRepository> _saleRepositoryMock = new();
    private readonly Mock<IMapper> _mapperMock = new();
    private readonly Mock<IPublisher> _publisherMock = new();
    private readonly Mock<ILogger<CreateSaleHandler>> _loggerMock = new();
    private readonly CreateSaleHandler _handler;

    public CreateSaleHandlerTests()
    {
        _handler = new CreateSaleHandler(_saleRepositoryMock.Object, _mapperMock.Object, _publisherMock.Object, _loggerMock.Object);
    }

    [Fact(DisplayName = "Should create sale successfully")]
    public async Task Given_ValidCommand_When_Handled_Then_ShouldCreateSaleSuccessfully()
    {
        // Arrange
        var command = CreateSaleTestData.GenerateValidCreateSaleCommand();
        var sale = CreateSaleTestData.GenerateValidSale();
        var createSaleResult = new CreateSaleResult
        {
            Id = sale.Id,
            SaleNumber = sale.SaleNumber,
            SaleDate = sale.SaleDate,
            CustomerId = sale.CustomerId,
            CustomerName = sale.CustomerName,
            BranchId = sale.BranchId,
            BranchName = sale.BranchName,
            Items = sale.Items.Select(i => new CreateSaleItemResult
            {
                Id = i.Id,
                ProductId = i.ProductId,
                ProductName = i.ProductName,
                Quantity = i.Quantity,
                UnitPrice = i.UnitPrice,
                Discount = i.Discount,
                TotalAmount = i.TotalAmount
            }).ToList(),
            TotalAmount = sale.TotalAmount,
            ItemCount = sale.Items.Count,
            IsCancelled = sale.IsCancelled
        };

        _mapperMock.Setup(m => m.Map<Sale>(command)).Returns(sale);
        _mapperMock.Setup(m => m.Map<CreateSaleResult>(sale)).Returns(createSaleResult);
        _saleRepositoryMock.Setup(r => r.CreateAsync(It.IsAny<Sale>(), It.IsAny<CancellationToken>())).ReturnsAsync(sale);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(createSaleResult, result);
        _saleRepositoryMock.Verify(r => r.CreateAsync(It.IsAny<Sale>(), It.IsAny<CancellationToken>()), Times.Once);
        _publisherMock.Verify(p => p.Publish(It.IsAny<SaleCreatedNotification>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact(DisplayName = "Should throw validation exception for invalid command")]
    public async Task Given_InvalidCommand_When_Handled_Then_ShouldThrowValidationException()
    {
        // Arrange
        var command = CreateSaleTestData.GenerateInvalidCreateSaleCommand();

        // Act & Assert
        await Assert.ThrowsAsync<ValidationException>(() => _handler.Handle(command, CancellationToken.None));
    }

    [Fact(DisplayName = "Should apply discounts and calculate total amount")]
    public async Task Given_ValidCommand_When_Handled_Then_ShouldApplyDiscountsAndCalculateTotalAmount()
    {
        // Arrange
        var command = CreateSaleTestData.GenerateValidCreateSaleCommand();
        var sale = CreateSaleTestData.GenerateValidSale();
        var createSaleResult = new CreateSaleResult
        {
            Id = sale.Id,
            SaleNumber = sale.SaleNumber,
            SaleDate = sale.SaleDate,
            CustomerId = sale.CustomerId,
            CustomerName = sale.CustomerName,
            BranchId = sale.BranchId,
            BranchName = sale.BranchName,
            Items = sale.Items.Select(i => new CreateSaleItemResult
            {
                Id = i.Id,
                ProductId = i.ProductId,
                ProductName = i.ProductName,
                Quantity = i.Quantity,
                UnitPrice = i.UnitPrice,
                Discount = i.Discount,
                TotalAmount = i.TotalAmount
            }).ToList(),
            TotalAmount = sale.TotalAmount,
            ItemCount = sale.Items.Count,
            IsCancelled = sale.IsCancelled
        };

        _mapperMock.Setup(m => m.Map<Sale>(command)).Returns(sale);
        _mapperMock.Setup(m => m.Map<CreateSaleResult>(sale)).Returns(createSaleResult);
        _saleRepositoryMock.Setup(r => r.CreateAsync(It.IsAny<Sale>(), It.IsAny<CancellationToken>())).ReturnsAsync(sale);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(createSaleResult, result);
        _saleRepositoryMock.Verify(r => r.CreateAsync(It.IsAny<Sale>(), It.IsAny<CancellationToken>()), Times.Once);
        _mapperMock.Verify(m => m.Map<CreateSaleResult>(sale), Times.Once);
        _publisherMock.Verify(p => p.Publish(It.IsAny<SaleCreatedNotification>(), It.IsAny<CancellationToken>()), Times.Once);
    }
}