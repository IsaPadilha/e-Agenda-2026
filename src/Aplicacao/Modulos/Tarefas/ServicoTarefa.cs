using eAgenda.Dominio.Compartilhado;
using eAgenda.Dominio.Modulos.Tarefas;
using FluentResults;

namespace eAgenda.Aplicacao.Modulos.Tarefas;

public sealed class ServicoTarefa(IRepositorioTarefa repositorioTarefa)
{
    public Result<Guid> Cadastrar(CadastrarTarefaDto dto)
    {
        Tarefa tarefa = new(dto.Titulo, dto.Prioridade);

        List<ErroValidacao> erros = tarefa.Validar();

        if (erros.Count > 0)
        {
            List<Error> errosResultado = erros
                .Select(e => new Error(e.Mensagem).WithMetadata("Campo", e.Campo)).ToList();

            return Result.Fail(errosResultado);
        }

        repositorioTarefa.Cadastrar(tarefa);

        return Result.Ok(tarefa.Id);
    }

    public Result Editar(EditarTarefaDto dto)
    {
        Tarefa tarefaAtualizada = new(dto.Titulo, dto.Prioridade);

        List<ErroValidacao> erros = tarefaAtualizada.Validar();

        if (erros.Count > 0)
        {
            List<Error> errosResultado = erros
                .Select(e => new Error(e.Mensagem).WithMetadata("Campo", e.Campo)).ToList();

            return Result.Fail(errosResultado);
        }

        bool conseguiuEditar = repositorioTarefa.Editar(dto.Id, tarefaAtualizada);

        if (!conseguiuEditar)
            return Result.Fail("Não foi possível editar a tarefa.");

        return Result.Ok();
    }

    public Result Excluir(Guid id)
    {
        bool conseguiuExcluir = repositorioTarefa.Excluir(id);

        if (!conseguiuExcluir)
            return Result.Fail("Não foi possível excluir a tarefa.");

        return Result.Ok();
    }

    public Result<TarefaDto> SelecionarPorId(Guid id)
    {
        Tarefa? tarefa = repositorioTarefa.SelecionarPorId(id);

        if (tarefa == null)
            return Result.Fail("Tarefa não encontrada.");

        TarefaDto dto = new(
            tarefa.Id,
            tarefa.Titulo,
            tarefa.Prioridade,
            tarefa.DataCriacao
        );

        return Result.Ok(dto);
    }

    public List<TarefaDto> SelecionarTodas()
    {
        return repositorioTarefa
            .SelecionarTodos()
            .Select(t => new TarefaDto(t.Id, t.Titulo, t.Prioridade, t.DataCriacao))
            .ToList();
    }
}