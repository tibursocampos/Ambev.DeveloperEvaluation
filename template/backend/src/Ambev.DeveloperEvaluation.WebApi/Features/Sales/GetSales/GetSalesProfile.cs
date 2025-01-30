using Ambev.DeveloperEvaluation.Application.Common;
using Ambev.DeveloperEvaluation.Application.Sales.GetSales;

using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSales;

/// <summary>
/// Profile for mapping GetSales feature requests to queries and results to responses
/// </summary>
public class GetSalesProfile : Profile
{
    /// <summary>
    /// Initializes the mappings for GetSales feature
    /// </summary>
    public GetSalesProfile()
    {
        CreateMap<GetSalesRequest, GetSalesQuery>();
        CreateMap<GetSalesResult, GetSalesResponse>();
        CreateMap<PaginatedList<GetSalesResult>, PaginatedList<GetSalesResponse>>()
            .ConvertUsing((src, dest, context) =>
            {
                var mappedItems = context.Mapper.Map<List<GetSalesResponse>>(src);
                return new PaginatedList<GetSalesResponse>(mappedItems, src.TotalCount, src.CurrentPage, src.PageSize);
            });
    }
}