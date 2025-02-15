﻿using AutoMapper;
using MediatR;
using FluentValidation;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Application.Notifications.Sales.SaleModified;
using Ambev.DeveloperEvaluation.Application.Notifications.Sales.ItemCancelled;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;

/// <summary>
/// Handler for processing UpdateSaleCommand requests
/// </summary>
public class UpdateSaleHandler : IRequestHandler<UpdateSaleCommand, UpdateSaleResult?>
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;
    private readonly IPublisher _publisher;
    private readonly ILogger<UpdateSaleHandler> _logger;

    /// <summary>
    /// Initializes a new instance of UpdateSaleHandler
    /// </summary>
    /// <param name="saleRepository">The sale repository</param>
    /// <param name="mapper">The AutoMapper instance</param>
    /// <param name="publisher">The MediatR publisher instance</param>
    /// <param name="logger">The logger instance</param>
    public UpdateSaleHandler(ISaleRepository saleRepository, IMapper mapper, IPublisher publisher, ILogger<UpdateSaleHandler> logger)
    {
        _saleRepository = saleRepository ?? throw new ArgumentNullException(nameof(saleRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _publisher = publisher ?? throw new ArgumentNullException(nameof(publisher));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Handles the UpdateSaleCommand request
    /// </summary>
    /// <param name="command">The UpdateSale command</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The updated sale details</returns>
    public async Task<UpdateSaleResult?> Handle(UpdateSaleCommand command, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling UpdateSaleCommand for Sale ID: {SaleId}", command.Id);

        await ValidateCommandAsync(command, cancellationToken);

        var existingSale = await GetExistingSaleAsync(command.Id, cancellationToken);
        var updatedSale = UpdateSaleDetails(existingSale, command);

        var result = await UpdateSaleAsync(updatedSale, cancellationToken);
        if(result is null)
        {
            return default;
        }

        await PublishNotificationAsync(existingSale, updatedSale, cancellationToken);

        return result;
    }

    private async Task ValidateCommandAsync(UpdateSaleCommand command, CancellationToken cancellationToken)
    {
        var validator = new UpdateSaleValidator();
        var validationResult = await validator.ValidateAsync(command, cancellationToken);
        if (!validationResult.IsValid)
        {
            _logger.LogWarning("Validation failed for UpdateSaleCommand. Errors: {Errors}", validationResult.Errors);
            throw new ValidationException(validationResult.Errors);
        }

        if (command.Items.Count == 0)
        {
            _logger.LogWarning("Sale with ID {SaleId} must contain at least one item", command.Id);
            throw new ValidationException("Sale must contain at least one item.");
        }

        if (!await _saleRepository.ExistsAndNotCancelledAsync(command.Id, cancellationToken))
        {
            _logger.LogWarning("Sale with ID {SaleId} not found or already cancelled", command.Id);
            throw new ValidationException("Sale not found or already cancelled.");
        }
    }

    private async Task<Sale> GetExistingSaleAsync(Guid saleId, CancellationToken cancellationToken)
    {
        var existingSale = await _saleRepository.GetByIdAsync(saleId, cancellationToken);
        if (existingSale is null)
        {
            _logger.LogWarning("Sale with ID {SaleId} not found", saleId);
            throw new KeyNotFoundException("Sale not found.");
        }

        return existingSale;
    }

    private Sale UpdateSaleDetails(Sale existingSale, UpdateSaleCommand command)
    {
        var sale = _mapper.Map<Sale>(command);
        sale.ApplyDiscounts();
        sale.CalculateTotalAmount();

        return sale;
    }

    private async Task<UpdateSaleResult?> UpdateSaleAsync(Sale sale, CancellationToken cancellationToken)
    {
        var updated = await _saleRepository.UpdateAsync(sale, cancellationToken);
        if (!updated)
        {
            _logger.LogWarning("Sale with ID {SaleId} not found during update", sale.Id);
            return default;
        }

        var result = _mapper.Map<UpdateSaleResult>(sale);
        _logger.LogInformation("Sale with ID {SaleId} updated successfully", sale.Id);

        return result;
    }

    private async Task PublishNotificationAsync(Sale existingSale, Sale updatedSale, CancellationToken cancellationToken)
    {
        if (ItemsChanged(existingSale.Items, updatedSale.Items))
        {
            await _publisher.Publish(new ItemCancelledNotification { SaleId = updatedSale.Id }, cancellationToken);
            _logger.LogInformation("Published ItemCancelledNotification for Sale ID: {SaleId}", updatedSale.Id);
        }
        else
        {
            await _publisher.Publish(new SaleModifiedNotification { SaleId = updatedSale.Id }, cancellationToken);
            _logger.LogInformation("Published SaleModifiedNotification for Sale ID: {SaleId}", updatedSale.Id);
        }
    }

    private static bool ItemsChanged(List<SaleItem> existingItems, List<SaleItem> updatedItems)
    {
        var existingItemIds = existingItems.Select(i => i.Id).ToHashSet();
        var updatedItemIds = updatedItems.Select(i => i.Id).ToHashSet();

        return !existingItemIds.SetEquals(updatedItemIds);
    }
}
