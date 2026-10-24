using eAgenda.Dominio.Compartilhado;
using eAgenda.Dominio.Modulos.Contatos;
using FluentResults;

namespace eAgenda.Aplicacao.Modulos.Contatos;

public sealed class ServicoContato(IRepositorioContato repositorioContato)
{
    public Result<Guid> Cadastrar(CadastrarContatoDto dto)
    {
        Contato contato = new Contato(
            dto.Nome,
            dto.Email,
            dto.Telefone,
            dto.Cargo,
            dto.Empresa
        );

        List<ErroValidacao> erros = contato.Validar();

        if (repositorioContato.ExisteContatoPorNome(contato.Nome))
        {
            erros.Add(new ErroValidacao
            {
                Campo = nameof(dto.Nome),
                Mensagem = "Já existe um contato cadastrado com o nome informado"
            });
        }

        if (repositorioContato.ExisteContatoPorEmail(contato.Email))
        {
            erros.Add(new ErroValidacao
            {
                Campo = nameof(dto.Email),
                Mensagem = "Já existe um contato cadastrado com o email informado"
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
        Contato contatoAtualizado = new Contato(
            dto.Nome,
            dto.Email,
            dto.Telefone,
            dto.Cargo,
            dto.Empresa
        );

        List<ErroValidacao> erros = contatoAtualizado.Validar();

        if (repositorioContato.ExisteContatoPorNome(contatoAtualizado.Nome, dto.Id))
        {
            erros.Add(new ErroValidacao
            {
                Campo = nameof(dto.Nome),
                Mensagem = "Já existe um contato cadastrado com o nome informado"
            });
        }

        if (repositorioContato.ExisteContatoPorEmail(contatoAtualizado.Email, dto.Id))
        {
            erros.Add(new ErroValidacao
            {
                Campo = nameof(dto.Email),
                Mensagem = "Já existe um contato cadastrado com o email informado"
            });
        }

        if (erros.Count > 0)
        {
            List<Error> errosResultado = erros
                .Select(e => new Error(e.Mensagem).WithMetadata("Campo", e.Campo)).ToList();

            return Result.Fail(errosResultado);
        }

        bool conseguiuEditar = repositorioContato.Editar(dto.Id, contatoAtualizado);

        if (!conseguiuEditar)
            return Result.Fail("Não foi possível editar o contato.");

        return Result.Ok();
    }

    public Result Excluir(Guid id)
    {
        bool conseguiuExcluir = repositorioContato.Excluir(id);

        if (!conseguiuExcluir)
            return Result.Fail("Não foi possível excluir o contato.");

        return Result.Ok();
    }

    public Result<ContatoDto> SelecionarPorId(Guid id)
    {
        Contato? contato = repositorioContato.SelecionarPorId(id);

        if (contato == null)
            return Result.Fail("Contato não encontrada.");

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
        return repositorioContato
            .SelecionarTodos()
            .Select(c => new ContatoDto(c.Id, c.Nome, c.Email, c.Telefone, c.Cargo, c.Empresa))
            .ToList();
    }
}