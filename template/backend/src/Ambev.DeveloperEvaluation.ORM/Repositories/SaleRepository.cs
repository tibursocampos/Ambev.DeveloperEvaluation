using Ambev.DeveloperEvaluation.Common.Utils;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;

using Microsoft.EntityFrameworkCore;

namespace Ambev.DeveloperEvaluation.ORM.Repositories;

/// <summary>
/// Implementation of ISaleRepository using Entity Framework Core
/// </summary>
public class SaleRepository : ISaleRepository
{
    private readonly DefaultContext _context;

    /// <summary>
    /// Initializes a new instance of SaleRepository
    /// </summary>
    /// <param name="context">The database context</param>
    public SaleRepository(DefaultContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Creates a new sale in the database
    /// </summary>
    /// <param name="sale">The sale to create</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The created sale</returns>
    public async Task<Sale> CreateAsync(Sale sale, CancellationToken cancellationToken = default)
    {
        sale.SaleDate = DateTime.SpecifyKind(sale.SaleDate, DateTimeKind.Utc);
        sale.CreatedAt = DateTime.SpecifyKind(sale.CreatedAt, DateTimeKind.Utc);

        await _context.Sales.AddAsync(sale, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return sale;
    }

    /// <summary>
    /// Retrieves a sale by its unique identifier
    /// </summary>
    /// <param name="id">The unique identifier of the sale</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The sale if found, null otherwise</returns>
    public async Task<Sale?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Sales
            .Include(s => s.Items)
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);
    }

    /// <summary>
    /// Retrieves all sales with pagination, ordering, and filtering
    /// </summary>
    /// <param name="page">Page number (default: 1)</param>
    /// <param name="size">Number of items per page (default: 10)</param>
    /// <param name="order">Order of the elements in the collection (asc or desc, default: asc)</param>
    /// <param name="filters">Dictionary of filters to apply</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A paginated list of sales</returns>
    public async Task<IEnumerable<Sale>> GetAllAsync(int page = 1,
                                                     int size = 10,
                                                     string order = Constants.AscendingOrder,
                                                     Dictionary<string, string>? filters = null,
                                                     CancellationToken cancellationToken = default)
    {
        IQueryable<Sale> query = _context.Sales.Include(s => s.Items);

        if (filters != null)
        {
            query = ApplyFilters(query, filters);
        }

        query = order.Equals(Constants.DescendingOrder, 
                             StringComparison.CurrentCultureIgnoreCase) ? 
                             query.OrderByDescending(s => s.SaleDate) :
                             query.OrderBy(s => s.SaleDate);

        query = query.Skip((page - 1) * size).Take(size);

        return await query.ToListAsync(cancellationToken);
    }

    /// <summary>
    /// Updates an existing sale in the database
    /// </summary>
    /// <param name="sale">The sale to update</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>True if the sale was updated, false if not found</returns>
    public async Task<bool> UpdateAsync(Sale sale, CancellationToken cancellationToken = default)
    {
        sale.SaleDate = DateTime.SpecifyKind(sale.SaleDate, DateTimeKind.Utc);
        sale.CreatedAt = DateTime.SpecifyKind(sale.CreatedAt, DateTimeKind.Utc);
        sale.UpdatedAt = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc);

        _context.Sales.Update(sale);

        return await _context.SaveChangesAsync(cancellationToken) > 0;
    }

    /// <summary>
    /// Deletes a sale from the database
    /// </summary>
    /// <param name="id">The unique identifier of the sale to delete</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>True if the sale was deleted, false if not found</returns>
    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var sale = await _context.Sales
            .Include(s => s.Items)
            .FirstOrDefaultAsync(s => s.Id == id, cancellationToken) ?? throw new InvalidOperationException($"Sale with ID {id} not found");

        sale.Cancel();
        _context.Sales.Update(sale);

        return await _context.SaveChangesAsync(cancellationToken) > 0;
    }

    public async Task<bool> ExistsAndNotCancelledAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Sales.AnyAsync(s => s.Id == id && !s.IsCancelled, cancellationToken);
    }

    /// <summary>
    /// Applies filters to the sales query
    /// </summary>
    /// <param name="query">The sales query</param>
    /// <param name="filters">Dictionary of filters to apply</param>
    /// <returns>The filtered sales query</returns>
    private static IQueryable<Sale> ApplyFilters(IQueryable<Sale> query, Dictionary<string, string> filters)
    {
        foreach (var filter in filters)
        {
            switch (filter.Key)
            {
                case Constants.CustomerNameFilter:
                if (!string.IsNullOrEmpty(filter.Value))
                {
                    query = query.Where(s => s.CustomerName.Contains(filter.Value.Replace("*", "")));
                }
                break;
                case Constants.SaleDateFilter:
                if (DateTime.TryParse(filter.Value, out var saleDate))
                {
                    var convertedDate = DateTime.SpecifyKind(saleDate, DateTimeKind.Utc);
                    query = query.Where(s => s.SaleDate.Date == convertedDate.Date);
                }
                break;
                case Constants.SaleDateStartFilter:
                if (DateTime.TryParse(filter.Value, out var saleDateStart))
                {
                    var convertedDateStart = DateTime.SpecifyKind(saleDateStart, DateTimeKind.Utc);
                    query = query.Where(s => s.SaleDate.Date >= convertedDateStart.Date);
                }
                break;
                case Constants.SaleDateEndFilter:
                if (DateTime.TryParse(filter.Value, out var saleDateEnd))
                {
                    var convertedDateEnd = DateTime.SpecifyKind(saleDateEnd, DateTimeKind.Utc);
                    query = query.Where(s => s.SaleDate.Date <= convertedDateEnd.Date);
                }
                break;
                case Constants.BranchNameFilter:
                if (!string.IsNullOrEmpty(filter.Value))
                {
                    query = query.Where(s => s.BranchName.Contains(filter.Value.Replace("*", "")));
                }
                break;
                case Constants.IsCancelledFilter:
                if (bool.TryParse(filter.Value, out var isCancelled))
                {
                    query = query.Where(s => s.IsCancelled == isCancelled);
                }
                break;  
            }
        }

        return query;
    }
}