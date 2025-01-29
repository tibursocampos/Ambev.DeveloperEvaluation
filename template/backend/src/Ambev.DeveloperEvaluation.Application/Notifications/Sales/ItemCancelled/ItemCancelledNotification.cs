using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Notifications.Sales.ItemCancelled;

/// <summary>
/// Notification for when an item in a sale is cancelled
/// </summary>
public class ItemCancelledNotification : INotification
{
    /// <summary>
    /// The unique identifier of the cancelled item
    /// </summary>
    public Guid ItemId { get; set; }
}