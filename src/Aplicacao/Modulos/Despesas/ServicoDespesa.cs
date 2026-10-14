using eAgenda.Dominio.Modulos.Categorias;
using eAgenda.Dominio.Modulos.Despesas;
using FluentResults;

namespace eAgenda.Aplicacao.Modulos.Despesas;

public sealed class ServicoDespesa(
    IRepositorioDespesa repositorioDespesa,
    IRepositorioCategoria repositorioCategoria
)
{
    public Result<Guid> Cadastrar(CadastrarDespesaDto dto)
    {
        return Result.Ok();
    }

    public Result Editar(EditarDespesaDto dto)
    {
        return Result.Ok();
    }

    public Result Excluir(Guid id)
    {
        return Result.Ok();
    }

    public Result<DespesaDto> SelecionarPorId(Guid id)
    {
        return Result.Ok();
    }

    public List<DespesaDto> SelecionarTodos()
    {
        return new List<DespesaDto>();
    }
}