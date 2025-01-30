using Ambev.DeveloperEvaluation.Application.Sales.DeleteSale;

using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Application.TestData;

public static class DeleteSaleTestData
{
    public static DeleteSaleCommand GenerateValidDeleteSaleCommand()
    {
        return new Faker<DeleteSaleCommand>()
            .CustomInstantiator(f => new DeleteSaleCommand(Guid.NewGuid()));
    }

    public static DeleteSaleCommand GenerateInvalidDeleteSaleCommand()
    {
        return new DeleteSaleCommand(Guid.Empty);
    }
}