using Ambev.DeveloperEvaluation.Application.Notifications.Sales;
using Ambev.DeveloperEvaluation.Application.Notifications.Sales.ItemCancelled;
using Ambev.DeveloperEvaluation.Application.Notifications.Sales.SaleCancelled;
using Ambev.DeveloperEvaluation.Common.RabbitMQ;
using Ambev.DeveloperEvaluation.Domain.Enums;

using Microsoft.Extensions.Logging;

using Moq;

using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Notification.Sales;

/// <summary>
/// Contains unit tests for the ItemCancelledNotificationHandler class.
/// </summary>
public class SaleCancelledNotificationHandlerTests
{
    private readonly Mock<IRabbitMQService> _rabbitMQServiceMock = new();
    private readonly Mock<ILogger<SaleCancelledNotificationHandler>> _loggerMock = new();
    private readonly SaleCancelledNotificationHandler _handler;

    public SaleCancelledNotificationHandlerTests()
    {
        _handler = new SaleCancelledNotificationHandler(_rabbitMQServiceMock.Object, _loggerMock.Object);
    }

    [Fact(DisplayName = "Should log and send message when notification is received")]
    public async Task Given_ValidNotification_When_Handled_Then_ShouldLogAndSendMessage()
    {
        // Arrange
        var notification = new SaleCancelledNotification { SaleId = Guid.NewGuid() };

        // Act
        await _handler.Handle(notification, CancellationToken.None);

        // Assert
        _rabbitMQServiceMock.Verify(
            x => x.SendMessage(It.Is<EventSalesMessageModel>(msg =>
                msg.Event == EventSale.SaleCancelled &&
                msg.ItemId == notification.SaleId)),
            Times.Once);
    }

    [Fact(DisplayName = "Should handle RabbitMQ failure without throwing exception")]
    public async Task Given_RabbitMQFailure_When_Handled_Then_ShouldThrow()
    {
        // Arrange
        var notification = new SaleCancelledNotification { SaleId = Guid.NewGuid() };

        _rabbitMQServiceMock
            .Setup(x => x.SendMessage(It.IsAny<EventSalesMessageModel>()))
            .Throws(new Exception("RabbitMQ error"));

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => _handler.Handle(notification, CancellationToken.None));
    }

    [Fact(DisplayName = "Should throw exception when dependencies are null")]
    public void Given_NullDependencies_When_Constructed_Then_ShouldThrowException()
    {
        // Assert
        Assert.Throws<ArgumentNullException>(() => new SaleCancelledNotificationHandler(null, _loggerMock.Object));
        Assert.Throws<ArgumentNullException>(() => new SaleCancelledNotificationHandler(_rabbitMQServiceMock.Object, null));
    }
}
