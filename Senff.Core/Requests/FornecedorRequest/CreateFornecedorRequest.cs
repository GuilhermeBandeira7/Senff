using System.ComponentModel.DataAnnotations;

namespace Senff.Core.Requests.FornecedorRequest;

public class CreateFornecedorRequest : Request
{
    [Required(ErrorMessage = "Nome inválido")]
    [MaxLength(80, ErrorMessage = "O Nome deve conter até 80 caracteres")]
    [MinLength(5, ErrorMessage = "O Nome deve conter no mínimo 5 caracteres")]
    public string Nome { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Telefone inválido")]
    public int Telefone { get; set; }
}