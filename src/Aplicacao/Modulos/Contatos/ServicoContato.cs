using eAgenda.Dominio.Compartilhado;
using eAgenda.Dominio.Modulos.Contatos;
using FluentResults;

namespace eAgenda.Aplicacao.Modulos.Contatos;

public sealed class ServicoContato(IRepositorioContato repositorioContato)
{
    public Result<Guid> Cadastrar(CadastrarContatoDto dto)
    {
        Contato contato = new(dto.Nome, dto.Email, dto.Telefone, dto.Cargo, dto.Empresa);

        List<ErroValidacao> erros = contato.Validar();

        if (repositorioContato.ExisteContatoPorEmailOuTelefone(contato.Email, contato.Telefone))
        {
            erros.Add(new ErroValidacao
            {
                Campo = nameof(dto.Email),
                Mensagem = "Já existe um contato cadastrado com o e-mail ou telefone informado"
            });
        }

        if (erros.Count > 0)
        {
            List<Error> errosResultado = erros
                .Select(e => new Error(e.Mensagem).WithMetadata("Campo", e.Campo)).ToList();

            return Result.Fail(errosResultado);
        }

        repositorioContato.Cadastrar(contato);

        return Result.Ok(contato.Id);
    }

    public Result Editar(EditarContatoDto dto)
    {
        Contato contatoAtualizado = new(dto.Nome, dto.Email, dto.Telefone, dto.Cargo, dto.Empresa);
        contatoAtualizado.Id = dto.Id;

        List<ErroValidacao> erros = contatoAtualizado.Validar();

        if (repositorioContato.ExisteContatoPorEmailOuTelefone(contatoAtualizado.Email, contatoAtualizado.Telefone, dto.Id))
        {
            erros.Add(new ErroValidacao
            {
                Campo = nameof(contatoAtualizado.Email),
                Mensagem = "Já existe um contato cadastrado com o e-mail ou telefone informado"
            });
        }

        if (erros.Count > 0)
        {
            List<Error> errosResultado = erros
                .Select(e => new Error(e.Mensagem).WithMetadata("Campo", e.Campo)).ToList();

            return Result.Fail(errosResultado);
        }

        repositorioContato.Editar(dto.Id, contatoAtualizado);

        return Result.Ok();
    }

    public Result Excluir(Guid id)
    {
        bool conseguiuExcluir = repositorioContato.Excluir(id);

        if (!conseguiuExcluir)
            return Result.Fail("Não foi possível encontrar o contato informado.");

        return Result.Ok();
    }

    public Result<ContatoDto> SelecionarPorId(Guid id)
    {
        Contato? contato = repositorioContato.SelecionarPorId(id);

        if (contato == null)
            return Result.Fail("Contato não encontrado.");

        return Result.Ok(new ContatoDto(
            contato.Id,
            contato.Nome,
            contato.Email,
            contato.Telefone,
            contato.Cargo,
            contato.Empresa
        ));
    }

    public List<ContatoDto> SelecionarTodos()
    {
        List<Contato> contatos = repositorioContato.SelecionarTodos();

        return contatos
            .Select(c => new ContatoDto(
                c.Id,
                c.Nome,
                c.Email,
                c.Telefone,
                c.Cargo,
                c.Empresa
            ))
            .ToList();
    }
}