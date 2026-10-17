using eAgenda.Dominio.Compartilhado;

namespace eAgenda.Dominio.Modulos.Contatos;

public interface IRepositorioContato : IRepositorio<Contato>
{
    bool ExisteContatoPorEmailOuTelefone(string email, string telefone, Guid? idIgnorado = null);
}