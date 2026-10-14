using eAgenda.Dominio.Modulos.Categorias;
using eAgenda.Dominio.Compartilhado;
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
        List<Guid> idsDistintos = dto.CategoriasIds
            .Where(id => id != Guid.Empty)
            .Distinct()
            .ToList();

        if (idsDistintos.Count == 0)
        {
            IError erro = new Error("Selecione ao menos uma categoria válida.")
                .WithMetadata("Campo", nameof(dto.CategoriasIds));

            return Result.Fail(erro);
        }

        List<Categoria> categoriasSelecionadas = repositorioCategoria
            .SelecionarTodos()
            .Where(c => idsDistintos.Contains(c.Id))
            .ToList();

        Despesa despesa = new Despesa(
            dto.Descricao,
            dto.DataOcorrencia,
            dto.Valor,
            dto.FormaPagamento,
            categoriasSelecionadas
        );

        List<ErroValidacao> erros = despesa.Validar();

        if (erros.Count > 0)
        {
            List<Error> errosResultado = erros
                .Select(e => new Error(e.Mensagem).WithMetadata("Campo", e.Campo))
                .ToList();

            return Result.Fail(errosResultado);
        }

        repositorioDespesa.Cadastrar(despesa);

        return Result.Ok(despesa.Id);
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
        List<DespesaDto> dtos = repositorioDespesa
            .SelecionarTodos()
            .Select(d => new DespesaDto(
                d.Id,
                d.Descricao,
                d.DataOcorrencia,
                d.Valor,
                d.FormaPagamento,
                d.Categorias.Select(c => new CategoriaDespesaDto(c.Id, c.Titulo)).ToList()
            )).ToList();

        return dtos;
    }
}