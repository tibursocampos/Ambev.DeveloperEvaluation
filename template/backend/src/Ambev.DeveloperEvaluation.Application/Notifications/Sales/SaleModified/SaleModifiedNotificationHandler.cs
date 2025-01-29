using Ambev.DeveloperEvaluation.Common.RabbitMQ;
using Ambev.DeveloperEvaluation.Domain.Enums;

using MediatR;

using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Notifications.Sales.SaleModified;

/// <summary>
/// Handler for processing SaleModifiedNotification
/// </summary>
public class SaleModifiedNotificationHandler : INotificationHandler<SaleModifiedNotification>
{
    private readonly IRabbitMQService _rabbitMQService;
    private readonly ILogger<SaleModifiedNotificationHandler> _logger;

    /// <summary>
    /// Initializes a new instance of SaleModifiedNotificationHandler
    /// </summary>
    /// <param name="logger">The logger instance</param>
    public SaleModifiedNotificationHandler(IRabbitMQService rabbitMQService, ILogger<SaleModifiedNotificationHandler> logger)
    {
        _rabbitMQService = rabbitMQService ?? throw new ArgumentNullException(nameof(rabbitMQService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Handles the SaleModifiedNotification
    /// </summary>
    /// <param name="notification">The SaleModified notification</param>
    /// <param name="cancellationToken">Cancellation token</param>
    public Task Handle(SaleModifiedNotification notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Sale with ID modified: {SaleId}", notification.SaleId);

        var message = new EventSalesMessageModel(EventSale.SaleModified, notification.SaleId, DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc));
        _rabbitMQService.SendMessage(message);

        return Task.CompletedTask;
    }
}