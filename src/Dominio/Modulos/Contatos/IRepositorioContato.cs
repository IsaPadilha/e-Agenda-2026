using eAgenda.Dominio.Compartilhado;

namespace eAgenda.Dominio.Modulos.Contatos;

public interface IRepositorioContato : IRepositorio<Contato>
{
    bool ExisteContatoPorNome(string nome, Guid? idIgnorado = null);
    bool ExisteContatoPorEmail(string email, Guid? idIgnorado = null);
}