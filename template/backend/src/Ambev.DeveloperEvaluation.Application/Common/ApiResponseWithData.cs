namespace Ambev.DeveloperEvaluation.Application.Common;

public class ApiResponseWithData<T> : ApiResponse
{
    public T? Data { get; set; }
}
