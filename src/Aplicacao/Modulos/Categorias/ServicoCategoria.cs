using eAgenda.Dominio.Modulos.Categorias;
using FluentResults;

namespace eAgenda.Aplicacao.Modulos.Categorias;

public sealed class ServicoCategoria(IRepositorioCategoria repositorioCategoria)
{
    public Result<Guid> Cadastrar(CadastrarCategoriaDto dto)
    {
        Categoria categoria = new Categoria(dto.Titulo);

        List<string> erros = categoria.Validar();

        if (repositorioCategoria.ExisteCategoriaPorTitulo(categoria.Titulo))
            erros.Add("Já existe uma categoria cadastrada com o título informado");

        if (erros.Count > 0)
            return Result.Fail(erros[0]);

        repositorioCategoria.Cadastrar(categoria);

        return Result.Ok(categoria.Id);
    }

    public Result Editar(EditarCategoriaDto dto)
    {
        Categoria categoriaAtualizada = new Categoria(dto.Titulo);

        List<string> erros = categoriaAtualizada.Validar();

        if (repositorioCategoria.ExisteCategoriaPorTitulo(categoriaAtualizada.Titulo, dto.Id))
            erros.Add("Já existe uma categoria cadastrada com o título informado");

        if (erros.Count > 0)
            return Result.Fail(erros[0]);

        repositorioCategoria.Editar(dto.Id, categoriaAtualizada);

        return Result.Ok();
    }

    public Result<CategoriaDto> SelecionarPorId(Guid id)
    {
        Categoria? categoria = repositorioCategoria.SelecionarPorId(id);

        if (categoria == null)
            return Result.Fail("Categoria não encontrada.");

        return Result.Ok(new CategoriaDto(categoria.Id, categoria.Titulo));
    }

    public List<CategoriaDto> SelecionarTodos()
    {
        List<Categoria> categorias = repositorioCategoria.SelecionarTodos();

        return categorias
            .Select(c => new CategoriaDto(c.Id, c.Titulo))
            .ToList();
    }
}