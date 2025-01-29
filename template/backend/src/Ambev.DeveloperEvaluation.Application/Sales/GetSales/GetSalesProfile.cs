using AutoMapper;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Application.Common;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetSales;

/// <summary>
/// Profile for mapping between Sale entity and GetSalesResult
/// </summary>
public class GetSalesProfile : Profile
{
    /// <summary>
    /// Initializes the mappings for GetSales operation
    /// </summary>
    public GetSalesProfile()
    {
        CreateMap<Sale, GetSalesResult>();
    }
}