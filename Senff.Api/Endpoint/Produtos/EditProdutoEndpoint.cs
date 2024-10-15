using Senff.Api.Common.Api;
using Senff.Core.Handlers;
using Senff.Core.Models;
using Senff.Core.Requests.ProdutoRequest;
using Senff.Core.Responses;

namespace Senff.Api.Endpoint.Produtos;

public class EditProdutoEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
        => app.MapPut("/{id}", HandleAsync)
            .WithName("Produtos: Update")
            .WithSummary("Atualiza um produto")
            .WithDescription("Atualiza um produto")
            .WithOrder(4)
            .Produces<Response<Produto?>>();

    private static async Task<IResult> HandleAsync(
        IProdutoHandler handler,
        EditProdutoRequest request,
        int id)
    {
        request.UserId = ApiConfiguration.UserId;
        request.Id = id;

        var result = await handler.EditAsync(request);
        return result.IsSuccess
            ? TypedResults.Ok(result)
            : TypedResults.BadRequest(result);
    }
}