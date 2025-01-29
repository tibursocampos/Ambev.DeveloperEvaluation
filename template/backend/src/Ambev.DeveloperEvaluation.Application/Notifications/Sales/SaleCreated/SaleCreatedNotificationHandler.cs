using MediatR;

using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Notifications.Sales.SaleCreated;

/// <summary>
/// Handler for processing SaleCreatedNotification
/// </summary>
public class SaleCreatedNotificationHandler : INotificationHandler<SaleCreatedNotification>
{
    private readonly ILogger<SaleCreatedNotificationHandler> _logger;

    /// <summary>
    /// Initializes a new instance of SaleCreatedNotificationHandler
    /// </summary>
    /// <param name="logger">The logger instance</param>
    public SaleCreatedNotificationHandler(ILogger<SaleCreatedNotificationHandler> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Handles the SaleCreatedNotification
    /// </summary>
    /// <param name="notification">The SaleCreated notification</param>
    /// <param name="cancellationToken">Cancellation token</param>
    public Task Handle(SaleCreatedNotification notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Sale created with ID: {notification.SaleId}");
        return Task.CompletedTask;
    }
}