using Ambev.DeveloperEvaluation.Application.Notifications.Sales.SaleCancelled;
using Ambev.DeveloperEvaluation.Application.Sales.DeleteSale;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Unit.Application.TestData;

using FluentValidation;

using MediatR;

using Microsoft.Extensions.Logging;

using Moq;

using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Sales.DeleteSale;

/// <summary>
/// Contains unit tests for the DeleteSaleHandler class.
/// </summary>
public class DeleteSaleHandlerTests
{
    private readonly Mock<ISaleRepository> _saleRepositoryMock = new();
    private readonly Mock<IPublisher> _publisherMock = new();
    private readonly Mock<ILogger<DeleteSaleHandler>> _loggerMock = new();
    private readonly DeleteSaleHandler _handler;

    public DeleteSaleHandlerTests()
    {
        _handler = new DeleteSaleHandler(_saleRepositoryMock.Object, _publisherMock.Object, _loggerMock.Object);
    }

    [Fact(DisplayName = "Should delete sale successfully")]
    public async Task Given_ValidCommand_When_Handled_Then_ShouldDeleteSaleSuccessfully()
    {
        // Arrange
        var command = DeleteSaleTestData.GenerateValidDeleteSaleCommand();

        _saleRepositoryMock.Setup(r => r.ExistsAndNotCancelledAsync(command.Id, It.IsAny<CancellationToken>())).ReturnsAsync(true);
        _saleRepositoryMock.Setup(r => r.DeleteAsync(command.Id, It.IsAny<CancellationToken>())).ReturnsAsync(true);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result);
        _saleRepositoryMock.Verify(r => r.DeleteAsync(command.Id, It.IsAny<CancellationToken>()), Times.Once);
        _publisherMock.Verify(p => p.Publish(It.IsAny<SaleCancelledNotification>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact(DisplayName = "Should throw validation exception for invalid command")]
    public async Task Given_InvalidCommand_When_Handled_Then_ShouldThrowValidationException()
    {
        // Arrange
        var command = DeleteSaleTestData.GenerateInvalidDeleteSaleCommand();

        // Act & Assert
        await Assert.ThrowsAsync<ValidationException>(() => _handler.Handle(command, CancellationToken.None));
    }

    [Fact(DisplayName = "Should return false if sale not found or already cancelled")]
    public async Task Given_SaleNotFoundOrCancelled_When_Handled_Then_ShouldReturnFalse()
    {
        // Arrange
        var command = DeleteSaleTestData.GenerateValidDeleteSaleCommand();

        _saleRepositoryMock.Setup(r => r.ExistsAndNotCancelledAsync(command.Id, It.IsAny<CancellationToken>())).ReturnsAsync(false);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result);
        _saleRepositoryMock.Verify(r => r.DeleteAsync(command.Id, It.IsAny<CancellationToken>()), Times.Never);
        _publisherMock.Verify(p => p.Publish(It.IsAny<SaleCancelledNotification>(), It.IsAny<CancellationToken>()), Times.Never);
    }
}