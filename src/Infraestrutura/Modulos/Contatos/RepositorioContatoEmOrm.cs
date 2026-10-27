using eAgenda.Dominio.Modulos.Contatos;
using eAgenda.Infraestrutura.Compartilhado.Orm;

namespace eAgenda.Infraestrutura.Modulos.Contatos;

public sealed class RepositorioContatoEmOrm(EAgendaDbContext dbContext) :
    RepositorioBaseEmOrm<Contato>(dbContext), IRepositorioContato
{
    public override List<Contato> SelecionarTodos()
    {
        return registros
            .OrderBy(c => c.Nome)
            .ToList();
    }

    public bool ExisteContatoPorNome(string nome, Guid? idIgnorado = null)
    {
        return registros.Any(
            c => c.Id != idIgnorado &&
            c.Nome.Trim() == nome.Trim()
        );
    }

    public bool ExisteContatoPorEmail(string email, Guid? idIgnorado = null)
    {
        return registros.Any(
            c => c.Id != idIgnorado &&
            c.Email.Trim() == email.Trim()
        );
    }
}