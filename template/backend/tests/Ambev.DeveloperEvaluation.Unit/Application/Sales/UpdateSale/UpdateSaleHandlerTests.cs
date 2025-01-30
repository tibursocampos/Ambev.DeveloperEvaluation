using Ambev.DeveloperEvaluation.Application.Notifications.Sales.ItemCancelled;
using Ambev.DeveloperEvaluation.Application.Notifications.Sales.SaleModified;
using Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Unit.Application.TestData;

using AutoMapper;

using FluentValidation;

using MediatR;

using Microsoft.Extensions.Logging;

using Moq;

using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Sales.UpdateSale;

/// <summary>
/// Contains unit tests for the UpdateSaleHandler class.
/// </summary>
public class UpdateSaleHandlerTests
{
    private readonly Mock<ISaleRepository> _saleRepositoryMock = new();
    private readonly Mock<IMapper> _mapperMock = new();
    private readonly Mock<IPublisher> _publisherMock = new();
    private readonly Mock<ILogger<UpdateSaleHandler>> _loggerMock = new();
    private readonly UpdateSaleHandler _handler;

    public UpdateSaleHandlerTests()
    {
        _handler = new UpdateSaleHandler(_saleRepositoryMock.Object, _mapperMock.Object, _publisherMock.Object, _loggerMock.Object);
    }

    [Fact(DisplayName = "Should update sale successfully")]
    public async Task Given_ValidCommand_When_Handled_Then_ShouldUpdateSaleSuccessfully()
    {
        // Arrange
        var command = UpdateSaleTestData.GenerateValidUpdateSaleCommand();
        var sale = UpdateSaleTestData.GenerateValidSale();
        sale.Id = command.Id;

        var updateSaleResult = new UpdateSaleResult
        {
            Id = sale.Id,
            SaleNumber = sale.SaleNumber,
            SaleDate = sale.SaleDate,
            CustomerId = sale.CustomerId,
            CustomerName = sale.CustomerName,
            BranchId = sale.BranchId,
            BranchName = sale.BranchName,
            Items = sale.Items.Select(i => new UpdateSaleItemResult
            {
                Id = i.Id,
                ProductId = i.ProductId,
                ProductName = i.ProductName,
                Quantity = i.Quantity,
                UnitPrice = i.UnitPrice,
                Discount = i.Discount,
                TotalAmount = i.TotalAmount
            }).ToList(),
            IsCancelled = sale.IsCancelled
        };

        _mapperMock.Setup(m => m.Map<Sale>(command)).Returns(sale);
        _mapperMock.Setup(m => m.Map<UpdateSaleResult>(sale)).Returns(updateSaleResult);
        _saleRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(sale);
        _saleRepositoryMock.Setup(r => r.UpdateAsync(It.IsAny<Sale>(), It.IsAny<CancellationToken>())).ReturnsAsync(true);
        _saleRepositoryMock.Setup(r => r.ExistsAndNotCancelledAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(true);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(updateSaleResult, result);
        _saleRepositoryMock.Verify(r => r.UpdateAsync(sale, It.IsAny<CancellationToken>()), Times.Once);
        _publisherMock.Verify(p => p.Publish(It.IsAny<SaleModifiedNotification>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact(DisplayName = "Should throw validation exception for invalid command")]
    public async Task Given_InvalidCommand_When_Handled_Then_ShouldThrowValidationException()
    {
        // Arrange
        var command = UpdateSaleTestData.GenerateInvalidUpdateSaleCommand();

        // Act & Assert
        await Assert.ThrowsAsync<ValidationException>(() => _handler.Handle(command, CancellationToken.None));
    }

    [Fact(DisplayName = "Should return null if sale not found during update")]
    public async Task Given_SaleNotFound_When_Handled_Then_ShouldReturnNull()
    {
        // Arrange
        var command = UpdateSaleTestData.GenerateValidUpdateSaleCommand();
        var sale = UpdateSaleTestData.GenerateValidSale();
        sale.Id = command.Id;

        _mapperMock.Setup(m => m.Map<Sale>(command)).Returns(sale);
        _saleRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(sale);
        _saleRepositoryMock.Setup(r => r.UpdateAsync(sale, It.IsAny<CancellationToken>())).ReturnsAsync(false);
        _saleRepositoryMock.Setup(r => r.ExistsAndNotCancelledAsync(command.Id, It.IsAny<CancellationToken>())).ReturnsAsync(true);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Null(result);
        _saleRepositoryMock.Verify(r => r.UpdateAsync(sale, It.IsAny<CancellationToken>()), Times.Once);
        _publisherMock.Verify(p => p.Publish(It.IsAny<SaleModifiedNotification>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact(DisplayName = "Should publish ItemCancelledNotification if items changed")]
    public async Task Given_ItemsChanged_When_Handled_Then_ShouldPublishItemCancelledNotification()
    {
        // Arrange
        var command = UpdateSaleTestData.GenerateValidUpdateSaleCommand();
        var existingSale = UpdateSaleTestData.GenerateValidSale();
        existingSale.Id = command.Id;
        existingSale.Items = [new SaleItem { Id = Guid.NewGuid(), ProductId = 2, Quantity = 1, UnitPrice = 20m }];
        var updatedSale = UpdateSaleTestData.GenerateValidSale();
        updatedSale.Id = command.Id;
        updatedSale.Items = [new SaleItem { Id = Guid.NewGuid(), ProductId = 1, Quantity = 2, UnitPrice = 10m }];

        var updateSaleResult = new UpdateSaleResult
        {
            Id = updatedSale.Id,
            SaleNumber = updatedSale.SaleNumber,
            SaleDate = updatedSale.SaleDate,
            CustomerId = updatedSale.CustomerId,
            CustomerName = updatedSale.CustomerName,
            BranchId = updatedSale.BranchId,
            BranchName = updatedSale.BranchName,
            Items = updatedSale.Items.ConvertAll(i => new UpdateSaleItemResult
            {
                Id = i.Id,
                ProductId = i.ProductId,
                ProductName = i.ProductName,
                Quantity = i.Quantity,
                UnitPrice = i.UnitPrice,
                Discount = i.Discount,
                TotalAmount = i.TotalAmount
            }),
            IsCancelled = updatedSale.IsCancelled
        };

        _mapperMock.Setup(m => m.Map<Sale>(command)).Returns(updatedSale);
        _mapperMock.Setup(m => m.Map<UpdateSaleResult>(updatedSale)).Returns(updateSaleResult);
        _saleRepositoryMock.Setup(r => r.GetByIdAsync(command.Id, It.IsAny<CancellationToken>())).ReturnsAsync(existingSale);
        _saleRepositoryMock.Setup(r => r.UpdateAsync(updatedSale, It.IsAny<CancellationToken>())).ReturnsAsync(true);
        _saleRepositoryMock.Setup(r => r.ExistsAndNotCancelledAsync(command.Id, It.IsAny<CancellationToken>())).ReturnsAsync(true);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(updateSaleResult, result);
        _saleRepositoryMock.Verify(r => r.UpdateAsync(updatedSale, It.IsAny<CancellationToken>()), Times.Once);
        _publisherMock.Verify(p => p.Publish(It.IsAny<ItemCancelledNotification>(), It.IsAny<CancellationToken>()), Times.Once);
        _publisherMock.Verify(p => p.Publish(It.IsAny<SaleModifiedNotification>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact(DisplayName = "Should publish SaleModifiedNotification if items changed")]
    public async Task Given_ItemsChanged_When_Handled_Then_ShouldPublishSaleModifiedNotification()
    {
        // Arrange
        var command = UpdateSaleTestData.GenerateValidUpdateSaleCommand();
        var existingSale = UpdateSaleTestData.GenerateValidSale();
        existingSale.Id = command.Id;
        existingSale.Items = [new SaleItem { Id = Guid.NewGuid(), ProductId = 2, Quantity = 1, UnitPrice = 20m }];
        var updatedSale = UpdateSaleTestData.GenerateValidSale();
        updatedSale.Id = command.Id;
        updatedSale.Items = existingSale.Items;
        updatedSale.Items[0].Quantity = 10;

        var updateSaleResult = new UpdateSaleResult
        {
            Id = updatedSale.Id,
            SaleNumber = updatedSale.SaleNumber,
            SaleDate = updatedSale.SaleDate,
            CustomerId = updatedSale.CustomerId,
            CustomerName = updatedSale.CustomerName,
            BranchId = updatedSale.BranchId,
            BranchName = updatedSale.BranchName,
            Items = updatedSale.Items.ConvertAll(i => new UpdateSaleItemResult
            {
                Id = i.Id,
                ProductId = i.ProductId,
                ProductName = i.ProductName,
                Quantity = i.Quantity,
                UnitPrice = i.UnitPrice,
                Discount = i.Discount,
                TotalAmount = i.TotalAmount
            }),
            IsCancelled = updatedSale.IsCancelled
        };

        _mapperMock.Setup(m => m.Map<Sale>(command)).Returns(updatedSale);
        _mapperMock.Setup(m => m.Map<UpdateSaleResult>(updatedSale)).Returns(updateSaleResult);
        _saleRepositoryMock.Setup(r => r.GetByIdAsync(command.Id, It.IsAny<CancellationToken>())).ReturnsAsync(existingSale);
        _saleRepositoryMock.Setup(r => r.UpdateAsync(updatedSale, It.IsAny<CancellationToken>())).ReturnsAsync(true);
        _saleRepositoryMock.Setup(r => r.ExistsAndNotCancelledAsync(command.Id, It.IsAny<CancellationToken>())).ReturnsAsync(true);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(updateSaleResult, result);
        _saleRepositoryMock.Verify(r => r.UpdateAsync(updatedSale, It.IsAny<CancellationToken>()), Times.Once);
        _publisherMock.Verify(p => p.Publish(It.IsAny<ItemCancelledNotification>(), It.IsAny<CancellationToken>()), Times.Never);
        _publisherMock.Verify(p => p.Publish(It.IsAny<SaleModifiedNotification>(), It.IsAny<CancellationToken>()), Times.Once);
    }
}
