using Senff.Api.Common.Api;
using Senff.Core.Handlers;
using Senff.Core.Models;
using Senff.Core.Requests.FornecedorRequest;
using Senff.Core.Requests.ProdutoRequest;
using Senff.Core.Responses;

namespace Senff.Api.Endpoint.Fornecedores;

public class EditFornecedorEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
        => app.MapPut("/{id}", HandleAsync)
            .WithName("Fornecedores: Update")
            .WithSummary("Atualiza um fornecedor")
            .WithDescription("Atualiza um fornecedor")
            .WithOrder(3)
            .Produces<Response<Fornecedor?>>();

    private static async Task<IResult> HandleAsync(
        IFornecedorHandler handler,
        EditFornecedorRequest request,
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