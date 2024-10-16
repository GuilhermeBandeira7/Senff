using System.ComponentModel.DataAnnotations;

namespace Senff.Core.Requests.ProdutoRequest;

public class DeleteProdutoRequest : Request
{
    [Required(ErrorMessage = "É necessário informar o Id do Produto")]
    public int Id { get; set; }
}