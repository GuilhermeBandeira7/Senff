using Senff.Core.Models;
using Senff.Core.Requests.FornecedorRequest;
using Senff.Core.Requests.ProdutoRequest;
using Senff.Core.Responses;

namespace Senff.Core.Handlers;

//Este handler define a implementação dos padrões de requisião e resposta
public interface IFornecedorHandler
{
    Task<Response<Fornecedor?>> CreateAsync(CreateFornecedorRequest request);
    Task<Response<Fornecedor?>> DeleteAsync(DeleteFornecedorRequest request);
    Task<Response<Fornecedor?>> EditAsync(EditFornecedorRequest request);
    Task<PagedResponse<List<Fornecedor>?>> ReadAsync(ReadFornecedorRequest request);
}