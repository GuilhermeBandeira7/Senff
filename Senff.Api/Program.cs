using Senff.Api;
using Senff.Api.Common.Api;
using Senff.Api.Endpoint;

var builder = WebApplication.CreateBuilder(args);
//Extension methods abaixo foram criados para resolver todas as dependências
builder.AddConfiguration();
builder.AddDataContexts();
builder.AddCrossOrigin();
builder.AddDocumentation();
builder.AddServices();

var app = builder.Build();
app.ConfigureDevEnvironment();

app.UseCors(ApiConfiguration.CorsPolicyName);
app.MapEndpoints();

app.Run();
