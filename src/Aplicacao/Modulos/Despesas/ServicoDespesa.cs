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
        List<Guid> idsDistintos = (dto.CategoriasIds ?? new List<Guid>())
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
            dto.Data,
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
        Console.WriteLine($"ID: {dto.Id}");
        Console.WriteLine($"Categorias: {dto.CategoriasIds.Count}");

        List<Guid> idsDistintos = dto.CategoriasIds
            .Where(id => id != Guid.Empty)
            .Distinct()
            .ToList();

        if (idsDistintos.Count == 0)
        {
            Console.WriteLine("Nenhuma categoria válida.");

            IError erro = new Error("Selecione ao menos uma categoria válida.")
                .WithMetadata("Campo", nameof(dto.CategoriasIds));

            return Result.Fail(erro);
        }

        List<Categoria> categoriasSelecionadas = repositorioCategoria
            .SelecionarTodos()
            .Where(c => idsDistintos.Contains(c.Id))
            .ToList();

        Despesa despesaAtualizada = new(
            dto.Descricao,
            dto.Data,
            dto.Valor,
            dto.FormaPagamento,
            categoriasSelecionadas
        );

        List<ErroValidacao> erros = despesaAtualizada.Validar();

        if (erros.Count > 0)
        {
            foreach (var erro in erros)
                Console.WriteLine($"Erro validação: {erro.Mensagem}");

            List<Error> errosResultado = erros
                .Select(e => new Error(e.Mensagem).WithMetadata("Campo", e.Campo))
                .ToList();

            return Result.Fail(errosResultado);
        }

        bool conseguiuEditar = repositorioDespesa.Editar(dto.Id, despesaAtualizada);

        Console.WriteLine($"Conseguiu editar: {conseguiuEditar}");

        if (!conseguiuEditar)
            return Result.Fail("Não foi possível editar a despesa.");

        return Result.Ok();
    }

    public Result Excluir(Guid id)
    {
        bool conseguiuExcluir = repositorioDespesa.Excluir(id);

        if (!conseguiuExcluir)
            return Result.Fail("Não foi possível excluir a despesa.");

        return Result.Ok();
    }

    public Result<DespesaDto> SelecionarPorId(Guid id)
    {
        Despesa? despesa = repositorioDespesa.SelecionarPorId(id);

        if (despesa == null)
            return Result.Fail<DespesaDto>("Despesa não encontrada.");

        DespesaDto dto = new(
            despesa.Id,
            despesa.Descricao,
            despesa.Data,
            despesa.Valor,
            despesa.FormaPagamento,
            despesa.Categorias.Select(c => new CategoriaDespesaDto(c.Id, c.Titulo)).ToList()
        );

        return Result.Ok(dto);
    }

    public List<DespesaDto> SelecionarTodos()
    {
        List<DespesaDto> dtos = repositorioDespesa
            .SelecionarTodos()
            .Select(d => new DespesaDto(
                d.Id,
                d.Descricao,
                d.Data,
                d.Valor,
                d.FormaPagamento,
                d.Categorias.Select(c => new CategoriaDespesaDto(c.Id, c.Titulo)).ToList()
            )).ToList();

        return dtos;
    }
}