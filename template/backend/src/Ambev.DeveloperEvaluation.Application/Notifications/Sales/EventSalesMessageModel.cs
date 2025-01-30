using Ambev.DeveloperEvaluation.Domain.Enums;

namespace Ambev.DeveloperEvaluation.Application.Notifications.Sales;

public record EventSalesMessageModel(EventSale Event, Guid ItemId, DateTime Timestamp);
