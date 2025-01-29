using MediatR;

using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Notifications.Sales.SaleModified;

/// <summary>
/// Handler for processing SaleModifiedNotification
/// </summary>
public class SaleModifiedNotificationHandler : INotificationHandler<SaleModifiedNotification>
{
    private readonly ILogger<SaleModifiedNotificationHandler> _logger;

    /// <summary>
    /// Initializes a new instance of SaleModifiedNotificationHandler
    /// </summary>
    /// <param name="logger">The logger instance</param>
    public SaleModifiedNotificationHandler(ILogger<SaleModifiedNotificationHandler> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Handles the SaleModifiedNotification
    /// </summary>
    /// <param name="notification">The SaleModified notification</param>
    /// <param name="cancellationToken">Cancellation token</param>
    public Task Handle(SaleModifiedNotification notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Sale modified with ID: {notification.SaleId}");
        return Task.CompletedTask;
    }
}