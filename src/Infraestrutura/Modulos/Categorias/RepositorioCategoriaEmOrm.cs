using eAgenda.Dominio.Modulos.Categorias;
using eAgenda.Infraestrutura.Compartilhado.Orm;

namespace eAgenda.Infraestrutura.Modulos.Categorias;

public sealed class RepositorioCategoriaEmOrm(EAgendaDbContext dbContext) : IRepositorioCategoria
{
    public void Cadastrar(Categoria entidade)
    {
        dbContext.Categorias.Add(entidade);

        dbContext.SaveChanges();
    }

    public bool Editar(Guid idSelecionado, Categoria entidadeAtualizada)
    {
        throw new NotImplementedException();
    }

    public bool Excluir(Guid idSelecionado)
    {
        throw new NotImplementedException();
    }

    public Categoria? SelecionarPorId(Guid idSelecionado)
    {
        return dbContext.Categorias.SingleOrDefault(c => c.Id == idSelecionado);
    }

    public List<Categoria> SelecionarTodos()
    {
        return dbContext.Categorias
            .OrderBy(c => c.Titulo)
            .ToList();
    }

    public bool ExisteCategoriaPorTitulo(string tituloCategoria, Guid? idIgnorado = null)
    {
        return dbContext.Categorias.Any(c =>
            c.Id != idIgnorado &&
            c.Titulo.ToLower() == tituloCategoria.ToLower()
        );
    }
}