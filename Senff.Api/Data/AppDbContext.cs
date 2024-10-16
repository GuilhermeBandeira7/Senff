using System.Reflection;
using Senff.Api.Data.Mappings;
using Senff.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Senff.Api.Data;

//Classe que criar a conexão com o banco de dados
//Criamos uma dependência de um DbContextOptions que é resolvida no Program.cs
public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Produto> Produtos { get; set; } = null!;
    public DbSet<Fornecedor> Fornecedors { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //Adicionamos aqui as configurações do mapeamento
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}