using Ambev.DeveloperEvaluation.Common.RabbitMQ;
using Ambev.DeveloperEvaluation.Domain.Enums;

using MediatR;

using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Notifications.Sales.SaleCreated;

/// <summary>
/// Handler for processing SaleCreatedNotification
/// </summary>
public class SaleCreatedNotificationHandler : INotificationHandler<SaleCreatedNotification>
{
    private readonly IRabbitMQService _rabbitMQService;
    private readonly ILogger<SaleCreatedNotificationHandler> _logger;

    /// <summary>
    /// Initializes a new instance of SaleCreatedNotificationHandler
    /// </summary>
    /// <param name="logger">The logger instance</param>
    public SaleCreatedNotificationHandler(IRabbitMQService rabbitMQService, ILogger<SaleCreatedNotificationHandler> logger)
    {
        _rabbitMQService = rabbitMQService ?? throw new ArgumentNullException(nameof(rabbitMQService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Handles the SaleCreatedNotification
    /// </summary>
    /// <param name="notification">The SaleCreated notification</param>
    /// <param name="cancellationToken">Cancellation token</param>
    public Task Handle(SaleCreatedNotification notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Sale created with ID: {SaleId}", notification.SaleId);

        var message = new EventSalesMessageModel(EventSale.SaleCreated, notification.SaleId, DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc));
        _rabbitMQService.SendMessage(message);

        return Task.CompletedTask;
    }
}