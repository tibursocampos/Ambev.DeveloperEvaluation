using Ambev.DeveloperEvaluation.Common.RabbitMQ;
using Ambev.DeveloperEvaluation.Domain.Enums;

using MediatR;

using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Notifications.Sales.ItemCancelled;

/// <summary>
/// Handler for processing ItemCancelledNotification
/// </summary>
public class ItemCancelledNotificationHandler : INotificationHandler<ItemCancelledNotification>
{
    private readonly IRabbitMQService _rabbitMQService;
    private readonly ILogger<ItemCancelledNotificationHandler> _logger;

    /// <summary>
    /// Initializes a new instance of ItemCancelledNotificationHandler
    /// </summary>
    /// <param name="logger">The logger instance</param>
    public ItemCancelledNotificationHandler(IRabbitMQService rabbitMQService, ILogger<ItemCancelledNotificationHandler> logger)
    {
        _rabbitMQService = rabbitMQService ?? throw new ArgumentNullException(nameof(rabbitMQService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Handles the ItemCancelledNotification
    /// </summary>
    /// <param name="notification">The ItemCancelled notification</param>
    /// <param name="cancellationToken">Cancellation token</param>
    public Task Handle(ItemCancelledNotification notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Item with ID cancelled: {ItemId}", notification.SaleId);

        var message = new EventSalesMessageModel(EventSale.ItemCancelled, notification.SaleId, DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc));
        _rabbitMQService.SendMessage(message);

        return Task.CompletedTask;
    }
}