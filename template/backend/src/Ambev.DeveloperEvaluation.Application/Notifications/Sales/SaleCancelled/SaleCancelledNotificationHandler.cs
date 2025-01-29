using MediatR;

using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Notifications.Sales.SaleCancelled;

/// <summary>
/// Handler for processing SaleCancelledNotification
/// </summary>
public class SaleCancelledNotificationHandler : INotificationHandler<SaleCancelledNotification>
{
    private readonly ILogger<SaleCancelledNotificationHandler> _logger;

    /// <summary>
    /// Initializes a new instance of SaleCancelledNotificationHandler
    /// </summary>
    /// <param name="logger">The logger instance</param>
    public SaleCancelledNotificationHandler(ILogger<SaleCancelledNotificationHandler> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Handles the SaleCancelledNotification
    /// </summary>
    /// <param name="notification">The SaleCancelled notification</param>
    /// <param name="cancellationToken">Cancellation token</param>
    public Task Handle(SaleCancelledNotification notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Sale cancelled with ID: {notification.SaleId}");
        return Task.CompletedTask;
    }
}