﻿using Ambev.DeveloperEvaluation.Domain.Entities;
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
    public async Task<IEnumerable<Sale>> GetAllAsync(int page = 1, int size = 10, string order = "asc", Dictionary<string, string>? filters = null, CancellationToken cancellationToken = default)
    {
        IQueryable<Sale> query = _context.Sales.Include(s => s.Items);

        // Apply filters
        if (filters != null)
        {
            foreach (var filter in filters)
            {
                if (filter.Key == "CustomerName" && !string.IsNullOrEmpty(filter.Value))
                {
                    query = query.Where(s => s.CustomerName.Contains(filter.Value.Replace("*", "")));
                }
            }
        }

        query = order.Equals("desc", StringComparison.CurrentCultureIgnoreCase) ? query.OrderByDescending(s => s.SaleDate) : query.OrderBy(s => s.SaleDate);
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
        var sale = await GetByIdAsync(id, cancellationToken);
        if (sale == null)
            return false;

        _context.Sales.Remove(sale);
        return await _context.SaveChangesAsync(cancellationToken) > 0;
    }
}
