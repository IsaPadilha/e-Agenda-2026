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

    public bool ExisteContatoPorEmailOuTelefone(string email, string telefone, Guid? idIgnorado = null)
    {
        return registros.Any(c =>
            c.Id != idIgnorado &&
            (c.Email.ToLower() == email.ToLower() || c.Telefone == telefone)
        );
    }
}