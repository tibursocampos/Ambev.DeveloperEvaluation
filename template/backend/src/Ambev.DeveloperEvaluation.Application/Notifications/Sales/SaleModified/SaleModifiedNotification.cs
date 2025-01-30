﻿using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Notifications.Sales.SaleModified;

/// <summary>
/// Notification for when a sale is modified
/// </summary>
public class SaleModifiedNotification : INotification
{
    /// <summary>
    /// The unique identifier of the modified sale
    /// </summary>
    public Guid SaleId { get; set; }
}