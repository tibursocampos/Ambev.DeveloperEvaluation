﻿using Ambev.DeveloperEvaluation.Application.Notifications.Sales;
using Ambev.DeveloperEvaluation.Application.Notifications.Sales.SaleCreated;
using Ambev.DeveloperEvaluation.Common.RabbitMQ;
using Ambev.DeveloperEvaluation.Domain.Enums;

using Microsoft.Extensions.Logging;

using Moq;

using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Notification.Sales;

/// <summary>
/// Contains unit tests for the ItemCancelledNotificationHandler class.
/// </summary>
public class SaleCreatedNotificationHandlerTests
{
    private readonly Mock<IRabbitMQService> _rabbitMQServiceMock = new();
    private readonly Mock<ILogger<SaleCreatedNotificationHandler>> _loggerMock = new();
    private readonly SaleCreatedNotificationHandler _handler;

    public SaleCreatedNotificationHandlerTests()
    {
        _handler = new SaleCreatedNotificationHandler(_rabbitMQServiceMock.Object, _loggerMock.Object);
    }

    [Fact(DisplayName = "Should log and send message when notification is received")]
    public async Task Given_ValidNotification_When_Handled_Then_ShouldLogAndSendMessage()
    {
        // Arrange
        var notification = new SaleCreatedNotification { SaleId = Guid.NewGuid() };

        // Act
        await _handler.Handle(notification, CancellationToken.None);

        // Assert
        _rabbitMQServiceMock.Verify(
            x => x.SendMessage(It.Is<EventSalesMessageModel>(msg =>
                msg.Event == EventSale.SaleCreated &&
                msg.ItemId == notification.SaleId)),
            Times.Once);
    }

    [Fact(DisplayName = "Should handle RabbitMQ failure without throwing exception")]
    public async Task Given_RabbitMQFailure_When_Handled_Then_ShouldThrow()
    {
        // Arrange
        var notification = new SaleCreatedNotification { SaleId = Guid.NewGuid() };

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
        Assert.Throws<ArgumentNullException>(() => new SaleCreatedNotificationHandler(null, _loggerMock.Object));
        Assert.Throws<ArgumentNullException>(() => new SaleCreatedNotificationHandler(_rabbitMQServiceMock.Object, null));
    }
}
