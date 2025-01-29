using MediatR;
using FluentValidation;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Application.Notifications.Sales.SaleCancelled;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Sales.DeleteSale;

/// <summary>
/// Handler for processing DeleteSaleCommand requests
/// </summary>
public class DeleteSaleHandler : IRequestHandler<DeleteSaleCommand, bool>
{
    private readonly ISaleRepository _saleRepository;
    private readonly IPublisher _publisher;
    private readonly ILogger<DeleteSaleHandler> _logger;

    /// <summary>
    /// Initializes a new instance of DeleteSaleHandler
    /// </summary>
    /// <param name="saleRepository">The sale repository</param>
    /// <param name="publisher">The MediatR publisher instance</param>
    /// <param name="logger">The logger instance</param>
    public DeleteSaleHandler(ISaleRepository saleRepository, IPublisher publisher, ILogger<DeleteSaleHandler> logger)
    {
        _saleRepository = saleRepository ?? throw new ArgumentNullException(nameof(saleRepository));
        _publisher = publisher ?? throw new ArgumentNullException(nameof(publisher));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Handles the DeleteSaleCommand request
    /// </summary>
    /// <param name="command">The DeleteSale command</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>True if the sale was deleted, false if not found</returns>
    public async Task<bool> Handle(DeleteSaleCommand command, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling DeleteSaleCommand for Sale ID: {SaleId}", command.Id);

        var validator = new DeleteSaleValidator();
        var validationResult = await validator.ValidateAsync(command, cancellationToken);
        if (!validationResult.IsValid)
        {
            _logger.LogWarning("Validation failed for DeleteSaleCommand. Errors: {Errors}", validationResult.Errors);
            throw new ValidationException(validationResult.Errors);
        }

        if (!await _saleRepository.ExistsAndNotCancelledAsync(command.Id, cancellationToken))
        {
            _logger.LogWarning("Sale with ID {SaleId} not found or already cancelled", command.Id);
            return false;
        }

        var deleted = await _saleRepository.DeleteAsync(command.Id, cancellationToken);
        if (deleted)
        {
            _logger.LogInformation("Sale with ID {SaleId} deleted successfully", command.Id);

            await _publisher.Publish(new SaleCancelledNotification { SaleId = command.Id }, cancellationToken);

            _logger.LogInformation("Published SaleCancelledNotification for Sale ID: {SaleId}", command.Id);
        }

        return deleted;
    }
}

