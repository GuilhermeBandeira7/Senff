using Senff.Api.Common.Api;
using Senff.Core.Handlers;
using Senff.Core.Models;
using Senff.Core.Requests.ProdutoRequest;
using Senff.Core.Responses;

namespace Senff.Api.Endpoint.Produtos;

public class DeleteProdutoEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
        => app.MapDelete("/{id}", HandleAsync)
            .WithName("Produtos: Delete")
            .WithSummary("Exclui um produto")
            .WithDescription("Exclui um produto")
            .WithOrder(3)
            .Produces<Response<Produto?>>();

    private static async Task<IResult> HandleAsync(
        IProdutoHandler handler,
        int id)
    {
        var request = new DeleteProdutoRequest()
        {
            UserId = ApiConfiguration.UserId,
            Id = id
        };

        var result = await handler.DeleteAsync(request);
        return result.IsSuccess
            ? TypedResults.Ok(result)
            : TypedResults.BadRequest(result);
    }
}