using MediatR;

using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Notifications.Sales.ItemCancelled;

/// <summary>
/// Handler for processing ItemCancelledNotification
/// </summary>
public class ItemCancelledNotificationHandler : INotificationHandler<ItemCancelledNotification>
{
    private readonly ILogger<ItemCancelledNotificationHandler> _logger;

    /// <summary>
    /// Initializes a new instance of ItemCancelledNotificationHandler
    /// </summary>
    /// <param name="logger">The logger instance</param>
    public ItemCancelledNotificationHandler(ILogger<ItemCancelledNotificationHandler> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Handles the ItemCancelledNotification
    /// </summary>
    /// <param name="notification">The ItemCancelled notification</param>
    /// <param name="cancellationToken">Cancellation token</param>
    public Task Handle(ItemCancelledNotification notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Item cancelled with ID: {notification.ItemId}");
        return Task.CompletedTask;
    }
}