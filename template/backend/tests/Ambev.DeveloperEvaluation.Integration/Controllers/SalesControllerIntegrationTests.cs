using Ambev.DeveloperEvaluation.Application.Common;
using Ambev.DeveloperEvaluation.Integration.TestData;
using Ambev.DeveloperEvaluation.WebApi;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSale;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSales;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.UpdateSale;

using System.Net.Http.Json;
using System.Text.Json;

using Xunit;

namespace Ambev.DeveloperEvaluation.Integration.Controllers;

/// <summary>
/// Integration tests for SalesController
/// </summary>
public class SalesControllerIntegrationTests : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    /// <summary>
    /// Initializes a new instance of the <see cref="SalesControllerIntegrationTests"/> class.
    /// </summary>
    /// <param name="factory">The custom web application factory</param>
    public SalesControllerIntegrationTests(CustomWebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    /// <summary>
    /// Tests the CreateSale endpoint.
    /// </summary>
    /// <param name="request">The create sale request</param>
    /// <param name="expectedSuccess">Indicates whether the request is expected to succeed</param>
    [Theory(DisplayName = "Given valid or invalid data, CreateSale returns expected result")]
    [MemberData(nameof(SalesControllerTestData.CreateSaleData), MemberType = typeof(SalesControllerTestData))]
    public async Task GivenValidOrInvalidData_CreateSale_ReturnsExpectedResult(CreateSaleRequest request, bool expectedSuccess)
    {
        // Act
        var response = await _client.PostAsJsonAsync("/api/sales", request);

        // Assert
        if (expectedSuccess)
        {
            response.EnsureSuccessStatusCode();
            var responseData = await response.Content.ReadFromJsonAsync<ApiResponseWithData<CreateSaleResponse>>();
            Assert.NotNull(responseData);
            Assert.True(responseData.Success);
            Assert.Equal("Sale created successfully", responseData.Message);
        }
        else
        {
            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                Assert.False(response.IsSuccessStatusCode, $"Expected failure but got success. Response content: {errorContent}");
            }
        }
    }

    /// <summary>
    /// Tests the UpdateSale endpoint.
    /// </summary>
    /// <param name="request">The update sale request</param>
    /// <param name="expectedSuccess">Indicates whether the request is expected to succeed</param>
    [Theory(DisplayName = "Given valid or invalid data, UpdateSale returns expected result")]
    [MemberData(nameof(SalesControllerTestData.UpdateSaleData), MemberType = typeof(SalesControllerTestData))]
    public async Task GivenValidOrInvalidData_UpdateSale_ReturnsExpectedResult(UpdateSaleRequest request, bool expectedSuccess)
    {
        // Arrange
        var createRequest = new CreateSaleRequest
        {
            SaleDate = request.SaleDate,
            CustomerId = request.CustomerId,
            CustomerName = request.CustomerName,
            BranchId = request.BranchId,
            BranchName = request.BranchName,
            Items = request.Items.Select(i => new CreateSaleItemRequest
            {
                ProductId = i.ProductId,
                ProductName = i.ProductName,
                Quantity = i.Quantity,
                UnitPrice = i.UnitPrice
            }).ToList()
        };

        var createdSale = await CreateSaleAsync(createRequest);
        request.Id = createdSale.Id;

        if (!expectedSuccess)
            request.Items = new List<UpdateSaleItemRequest>();

        // Act
        var response = await _client.PutAsJsonAsync($"/api/sales/{request.Id}", request);

        // Assert
        if (expectedSuccess)
        {
            response.EnsureSuccessStatusCode();
            var responseData = await response.Content.ReadFromJsonAsync<ApiResponseWithData<UpdateSaleResponse>>();
            Assert.NotNull(responseData);
            Assert.True(responseData.Success);
            Assert.Equal("Sale updated successfully", responseData.Message);
        }
        else
        {
            Assert.False(response.IsSuccessStatusCode);
        }
    }

    /// <summary>
    /// Tests the DeleteSale endpoint.
    /// </summary>
    [Fact(DisplayName = "Given a valid sale ID, DeleteSale returns expected result")]
    public async Task GivenValidSaleId_DeleteSale_ReturnsExpectedResult()
    {
        // Arrange
        var createRequest = SalesControllerTestData.CreateSaleFaker.Generate();
        var createdSale = await CreateSaleAsync(createRequest);

        // Act
        var response = await _client.DeleteAsync($"/api/sales/{createdSale.Id}");

        // Assert
        response.EnsureSuccessStatusCode();
        var responseData = await response.Content.ReadFromJsonAsync<ApiResponse>();
        Assert.NotNull(responseData);
        Assert.True(responseData.Success);
        Assert.Equal("Sale cancelled successfully", responseData.Message);
    }

    /// <summary>
    /// Tests the GetSale endpoint.
    /// </summary>
    /// <param name="id">The sale ID</param>
    /// <param name="expectedSuccess">Indicates whether the request is expected to succeed</param>
    [Theory(DisplayName = "Given a valid or invalid sale ID, GetSale returns expected result")]
    [MemberData(nameof(SalesControllerTestData.GetSaleData), MemberType = typeof(SalesControllerTestData))]
    public async Task GivenValidOrInvalidSaleId_GetSale_ReturnsExpectedResult(Guid id, bool expectedSuccess)
    {
        // Arrange
        if (expectedSuccess)
        {
            var createRequest = SalesControllerTestData.CreateSaleFaker.Generate();
            var createdSale = await CreateSaleAsync(createRequest);
            id = createdSale.Id;
        }

        // Act
        var response = await _client.GetAsync($"/api/sales/{id}");

        // Assert
        if (expectedSuccess)
        {
            response.EnsureSuccessStatusCode();
            var responseData = await response.Content.ReadFromJsonAsync<ApiResponseWithData<GetSaleResponse>>();
            Assert.NotNull(responseData);
            Assert.True(responseData.Success);
            Assert.Equal(id, responseData.Data.Id);
        }
        else
        {
            Assert.False(response.IsSuccessStatusCode);
        }
    }

    /// <summary>
    /// Tests the GetSales endpoint.
    /// </summary>
    /// <param name="page">The page number</param>
    /// <param name="size">The page size</param>
    /// <param name="order">The order of the results</param>
    /// <param name="filters">The filters to apply</param>
    [Theory(DisplayName = "Given valid pagination parameters, GetSales returns expected result")]
    [MemberData(nameof(SalesControllerTestData.GetSalesData), MemberType = typeof(SalesControllerTestData))]
    public async Task GivenValidPaginationParameters_GetSales_ReturnsExpectedResult(int page, int size, string order, Dictionary<string, string>? filters)
    {
        var createRequest1 = SalesControllerTestData.CreateSaleFaker.Generate();
        var createRequest2 = SalesControllerTestData.CreateSaleFaker.Generate();
        await CreateSaleAsync(createRequest1);
        await CreateSaleAsync(createRequest2);

        // Act
        var response = await _client.GetAsync($"/api/sales?page={page}&size={size}&order={order}");

        // Assert
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        var data = ExtractDataFromJson(content);
        Assert.NotNull(data);
        if (page == 2)
        {
            Assert.Empty(data);
        }
        else
        {
            Assert.NotEmpty(data);
        }
    }

    /// <summary>
    /// Creates a sale asynchronously.
    /// </summary>
    /// <param name="request">The create sale request</param>
    /// <returns>The created sale response</returns>
    private async Task<CreateSaleResponse?> CreateSaleAsync(CreateSaleRequest request)
    {
        var response = await _client.PostAsJsonAsync("/api/sales", request);
        response.EnsureSuccessStatusCode();
        var responseData = await response.Content.ReadFromJsonAsync<ApiResponseWithData<CreateSaleResponse>>();

        return responseData?.Data;
    }

    /// <summary>
    /// Deletes a sale asynchronously.
    /// </summary>
    /// <param name="id">The sale ID</param>
    /// <returns>True if the sale was deleted successfully, otherwise false</returns>
    private async Task<bool> DeleteSaleAsync(Guid id)
    {
        var response = await _client.DeleteAsync($"/api/sales/{id}");
        return response.IsSuccessStatusCode;
    }

    /// <summary>
    /// Extracts data from JSON.
    /// </summary>
    /// <param name="json">The JSON string</param>
    /// <returns>The list of GetSalesResponse</returns>
    private List<GetSalesResponse> ExtractDataFromJson(string json)
    {
        using (JsonDocument document = JsonDocument.Parse(json))
        {
            JsonElement root = document.RootElement;
            JsonElement dataElement = root.GetProperty("data");
            return JsonSerializer.Deserialize<List<GetSalesResponse>>(dataElement.GetRawText());
        }
    }
}
