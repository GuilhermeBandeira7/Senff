using Microsoft.AspNetCore.Mvc;
using Senff.Api.Data;
using Senff.Core.Handlers;
using Senff.Core.Models;
using Senff.Core.Requests.ProdutoRequest;
using Senff.Core.Responses;
using Microsoft.EntityFrameworkCore;
using RabbitMqLib.Services;

namespace Senff.Api.Handlers;

//Este handler é criado para implementar os padrões criados no core.
//Temos uma dependência do AppDbContext que foi resolvida no Program.cs
public class ProdutoHandler(AppDbContext context) : IProdutoHandler
{
    private readonly IRabbitPublisher _publisher
        = new RabbitPublisher("products sender", "guest", "guest");

    public async Task<Response<Produto?>> AddSuppierAsync(AddSupplierRequest request)
    {
        try
        {
            var produto = await context
                .Produtos
                .FirstOrDefaultAsync(x => x.Id == request.Id && x.UserId == request.UserId);

            var fornecedor = await context
                .Fornecedors
                .FirstOrDefaultAsync(x => x.Id == request.FornecedorId && x.UserId == request.UserId);

            if (produto is null || fornecedor is null)
                return new Response<Produto?>(null, 404, "Produto ou fornecedor não encontrado.");

            produto.Fornecedor = fornecedor;
            produto.FornecedorId = fornecedor.Id;

            context.Produtos.Update(produto);
            await context.SaveChangesAsync();

            return new Response<Produto?>(produto, 201, "Fornecedor adicionado com sucesso!");
        }
        catch
        {
            return new Response<Produto?>(null, 500, "Não foi adicionar fornecedor ao produto.");
        }
    }

    public async Task<Response<Produto?>> CreateAsync(
        CreateProdutoRequest request)
    {
        var produto = new Produto()
        {
            UserId = request.UserId,
            Nome = request.Nome,
            Descricao = request.Descricao,
            Preco = request.Preco
        };

        try
        {
            await context.Produtos.AddAsync(produto);
            await context.SaveChangesAsync();

            //rabbitMq
            await _publisher.SendingMessage<Produto>(produto, "senffapi", "prod");
            
            return new Response<Produto?>(produto, 201, "Produto criado com sucesso!");
        }
        catch(Exception ex)
        {
            return new Response<Produto?>(null, 500, "Não foi possível criar o produto.");
        }
    }

    public async Task<Response<Produto?>> DeleteAsync(DeleteProdutoRequest request)
    {
        try
        {
            var produto = await context
                .Produtos
                .FirstOrDefaultAsync(x => x.Id == request.Id && x.UserId == request.UserId);

            if (produto is null)
                return new Response<Produto?>(null, 404, "Produto não encontrado.");

            context.Produtos.Remove(produto);
            await context.SaveChangesAsync();

            return new Response<Produto?>(produto, message: "Produto excluído com sucesso!");
        }
        catch
        {
            return new Response<Produto?>(null, 500, "Não foi possível excluir o produto");
        }
    }

    public async Task<Response<Produto?>> DetailsAsync(DetailsProdutoRequest request)
    {
        try
        {
            var produto = await context
                .Produtos
                .Where(x => x.Id == request.Id && x.UserId == request.UserId)
                .Include(x => x.Fornecedor)
                .FirstOrDefaultAsync();

            if (produto is null)
                return new Response<Produto?>(null, 404, "Produto não encontrado.");

            return new Response<Produto?>(produto);
        }
        catch
        {
            return new Response<Produto?>(null, 500, "Não foi possível trazer os detalhes do produto");
        }
    }

    public async Task<Response<Produto?>> EditAsync(EditProdutoRequest request)
    {
        try
        {
            var produto = await context
                .Produtos
                .FirstOrDefaultAsync(x => x.Id == request.Id && x.UserId == request.UserId);


            if (produto is null)
                return new Response<Produto?>(null, 404, "Produto não encontrado.");

            produto.Nome = request.Nome;
            produto.Descricao = request.Descricao;
            produto.Preco = request.Preco;

            context.Produtos.Update(produto);
            await context.SaveChangesAsync();

            return new Response<Produto?>(produto, message: "Produto editado com sucesso!");
        }
        catch
        {
            return new Response<Produto?>(null, 500, "Não foi possível alterar o produto");
        }
    }

    public async Task<PagedResponse<List<Produto>?>> IndexAsync(IndexProdutoRequest request)
    {
        try
        {
            var query = context
                .Produtos
                .AsNoTracking()
                .Where(x => x.UserId == request.UserId)
                .Include(x => x.Fornecedor)
                .OrderBy(x => x.Nome);

            var produtos = await query
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync();

            var count = await query.CountAsync();

            return new PagedResponse<List<Produto>?>(
                produtos,
                count,
                request.PageNumber,
                request.PageSize);
        }
        catch
        {
            return new PagedResponse<List<Produto>?>(null, 500, "Não foi possível consultar os produtos");
        }
    }
}