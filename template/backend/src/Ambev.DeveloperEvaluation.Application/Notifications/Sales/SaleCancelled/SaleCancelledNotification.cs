using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Notifications.Sales.SaleCancelled;

/// <summary>
/// Notification for when a sale is cancelled
/// </summary>
public class SaleCancelledNotification : INotification
{
    /// <summary>
    /// The unique identifier of the cancelled sale
    /// </summary>
    public Guid SaleId { get; set; }
}