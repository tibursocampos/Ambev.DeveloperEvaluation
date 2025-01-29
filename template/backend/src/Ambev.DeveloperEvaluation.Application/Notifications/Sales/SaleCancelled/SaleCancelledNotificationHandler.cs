using Ambev.DeveloperEvaluation.Common.RabbitMQ;
using Ambev.DeveloperEvaluation.Domain.Enums;

using MediatR;

using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Notifications.Sales.SaleCancelled;

/// <summary>
/// Handler for processing SaleCancelledNotification
/// </summary>
public class SaleCancelledNotificationHandler : INotificationHandler<SaleCancelledNotification>
{
    private readonly IRabbitMQService _rabbitMQService;
    private readonly ILogger<SaleCancelledNotificationHandler> _logger;

    /// <summary>
    /// Initializes a new instance of SaleCancelledNotificationHandler
    /// </summary>
    /// <param name="logger">The logger instance</param>
    public SaleCancelledNotificationHandler(IRabbitMQService rabbitMQService, ILogger<SaleCancelledNotificationHandler> logger)
    {
        _rabbitMQService = rabbitMQService ?? throw new ArgumentNullException(nameof(rabbitMQService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Handles the SaleCancelledNotification
    /// </summary>
    /// <param name="notification">The SaleCancelled notification</param>
    /// <param name="cancellationToken">Cancellation token</param>
    public Task Handle(SaleCancelledNotification notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Sale with ID cancelled: {SaleId}", notification.SaleId);

        var message = new EventSalesMessageModel(EventSale.SaleCancelled, notification.SaleId, DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc));
        _rabbitMQService.SendMessage(message);

        return Task.CompletedTask;
    }
}