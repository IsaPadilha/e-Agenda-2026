using eAgenda.Dominio.Compartilhado;
using eAgenda.Dominio.Modulos.Compromissos;
using FluentResults;

namespace eAgenda.Aplicacao.Modulos.Compromissos;

public sealed class ServicoCompromisso(IRepositorioCompromisso repositorioCompromisso)
{
    public Result<Guid> Cadastrar(CadastrarCompromissoDto dto)
    {
        Compromisso compromisso = new(
            dto.Assunto,
            dto.DataOcorrencia,
            dto.HoraInicio,
            dto.HoraTermino,
            dto.TipoCompromisso,
            dto.LocalOuLink,
            dto.ContatoId,
            null
        );

        List<ErroValidacao> erros = compromisso.Validar();

        if (repositorioCompromisso.ExisteConflitoDeHorario(dto.DataOcorrencia, dto.HoraInicio, dto.HoraTermino))
        {
            erros.Add(new ErroValidacao
            {
                Campo = nameof(dto.HoraInicio),
                Mensagem = "Já existe um compromisso cadastrado com sobreposição de horários nesta data."
            });
        }

        if (erros.Count > 0)
        {
            List<Error> errosResultado = erros
                .Select(e => new Error(e.Mensagem).WithMetadata("Campo", e.Campo))
                .ToList();

            return Result.Fail(errosResultado);
        }

        repositorioCompromisso.Cadastrar(compromisso);

        return Result.Ok(compromisso.Id);
    }

    public Result Editar(EditarCompromissoDto dto)
    {
        Compromisso compromissoAtualizado = new(
            dto.Assunto,
            dto.DataOcorrencia,
            dto.HoraInicio,
            dto.HoraTermino,
            dto.TipoCompromisso,
            dto.LocalOuLink,
            dto.ContatoId,
            null
        );
        compromissoAtualizado.Id = dto.Id;

        List<ErroValidacao> erros = compromissoAtualizado.Validar();

        if (repositorioCompromisso.ExisteConflitoDeHorario(dto.DataOcorrencia, dto.HoraInicio, dto.HoraTermino, dto.Id))
        {
            erros.Add(new ErroValidacao
            {
                Campo = nameof(dto.HoraInicio),
                Mensagem = "Já existe outro compromisso cadastrado com sobreposição de horários nesta data."
            });
        }

        if (erros.Count > 0)
        {
            List<Error> errosResultado = erros
                .Select(e => new Error(e.Mensagem).WithMetadata("Campo", e.Campo))
                .ToList();

            return Result.Fail(errosResultado);
        }

        repositorioCompromisso.Editar(dto.Id, compromissoAtualizado);

        return Result.Ok();
    }

    public Result Excluir(Guid id)
    {
        bool conseguiuExcluir = repositorioCompromisso.Excluir(id);

        if (!conseguiuExcluir)
            return Result.Fail("Não foi possível encontrar o compromisso informado.");

        return Result.Ok();
    }

    public Result<CompromissoDto> SelecionarPorId(Guid id)
    {
        Compromisso? compromisso = repositorioCompromisso.SelecionarPorId(id);

        if (compromisso == null)
            return Result.Fail("Compromisso não encontrado.");

        return Result.Ok(new CompromissoDto(
            compromisso.Id,
            compromisso.Assunto,
            compromisso.DataOcorrencia,
            compromisso.HoraInicio,
            compromisso.HoraTermino,
            compromisso.TipoCompromisso,
            compromisso.LocalOuLink,
            compromisso.ContatoId,
            compromisso.Contato?.Nome
        ));
    }

    public List<CompromissoDto> SelecionarTodos()
    {
        List<Compromisso> compromissos = repositorioCompromisso.SelecionarTodos();

        return compromissos
            .Select(c => new CompromissoDto(
                c.Id,
                c.Assunto,
                c.DataOcorrencia,
                c.HoraInicio,
                c.HoraTermino,
                c.TipoCompromisso,
                c.LocalOuLink,
                c.ContatoId,
                c.Contato?.Nome
            ))
            .ToList();
    }
}