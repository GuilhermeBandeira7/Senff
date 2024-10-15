using Senff.Api.Common.Api;
using Senff.Core.Handlers;
using Senff.Core.Models;
using Senff.Core.Requests.FornecedorRequest;
using Senff.Core.Responses;

namespace Senff.Api.Endpoint.Fornecedores;

public class DeleteFornecedorEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
        => app.MapDelete("/{id}", HandleAsync)
            .WithName("Fornecedor: Delete")
            .WithSummary("Exclui um fornecedor")
            .WithDescription("Exclui um fornecedor")
            .WithOrder(2)
            .Produces<Response<Fornecedor?>>();
    
    private static async Task<IResult> HandleAsync(
        IFornecedorHandler handler,
        int id)
    {
        var request = new DeleteFornecedorRequest()
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