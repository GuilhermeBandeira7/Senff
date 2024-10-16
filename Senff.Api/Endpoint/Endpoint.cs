using RabbitMqLib.Services;
using Senff.Api.Common.Api;
using Senff.Api.Endpoint.Fornecedores;
using Senff.Api.Endpoint.Produtos;

namespace Senff.Api.Endpoint;

public static class Endpoint 
{
    public static void MapEndpoints(this WebApplication app)
    {
        var endpoints = app.MapGroup("");
        
        endpoints.MapGroup("/")
            .WithTags("Health Check")
            .MapGet("/", () =>
            {
                return new { message = "OK" };
            });
        
        endpoints.MapGroup("/")
            .WithTags("RabbitMq initializer")
            .MapGet("/rabbit", () =>
            {
                InitializeRabbitMq();
                return new { message = "RabbitMq inicializado com Queues e Exchanges para produtos e fornecedores" };
            });
        
        endpoints.MapGroup("v1/produtos")
            .WithTags("Produtos")
            .MapEndpoint<CreateProdutoEndpoint>()
            .MapEndpoint<EditProdutoEndpoint>()
            .MapEndpoint<DeleteProdutoEndpoint>()
            .MapEndpoint<IndexProdutoEndpoint>()
            .MapEndpoint<DetailProdutoEndpoint>()
            .MapEndpoint<AddSupplierEndpoint>();

        endpoints.MapGroup("v1/fornecedores")
            .WithTags("Fornecedores")
            //.RequireAuthorization()
            .MapEndpoint<CreateFornecedorEndpoint>()
            .MapEndpoint<DeleteFornecedorEndpoint>()
            .MapEndpoint<EditFornecedorEndpoint>()
            .MapEndpoint<ReadFornecedorEndpoint>();
    }
    
    private static IEndpointRouteBuilder MapEndpoint<TEndpoint>(this IEndpointRouteBuilder app)
        where TEndpoint : IEndpoint
    {
        TEndpoint.Map(app);
        return app;
    }

    private static void InitializeRabbitMq()
    {
        var publisher = new RabbitPublisher("Products Publisher", "guest", "guest");

        publisher.CreateExchange("senffapi", "direct");
        publisher.CreateQueue("providers", dlx: true);
        publisher.CreateQueue("products", dlx: true);
        publisher.BindQueueToExchange("senffapi", "providers", "prov");
        publisher.BindQueueToExchange("senffapi", "products", "prod");
    }
}