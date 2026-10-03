using eAgenda.Dominio.Modulos.Categorias;
using Microsoft.AspNetCore.Mvc;

namespace eAgenda.WebApp.Modulos.Categorias;

public sealed class CategoriaController(IRepositorioCategoria repositorioCategoria) : Controller
{
    [HttpGet]
    public ActionResult Listar()
    {
        List<Categoria> categorias = repositorioCategoria.SelecionarTodos();

        List<ListarCategoriasViewModel> listarVms = categorias
            .Select(c => new ListarCategoriasViewModel(c.Id, c.Titulo))
            .ToList();

        return View();
    }

    [HttpGet]
    public ActionResult Cadastrar()
    {
        CadastrarCategoriaViewModel viewModel = new("");

        return View(viewModel);
    }

    [HttpPost]
    public ActionResult Cadastrar(CadastrarCategoriaViewModel viewModel)
    {
        Categoria categoria = new Categoria(viewModel.Titulo);

        List<string> erros = categoria.Validar();

        if (repositorioCategoria.ExisteCategoriaPorTitulo(categoria.Titulo))
            erros.Add("Já existe uma categoria cadastrada com o título informado");

        if (erros.Count > 0)
        {
            ModelState.AddModelError(string.Empty, erros[0]);

            return View(viewModel);
        }

        repositorioCategoria.Cadastrar(categoria);

        return RedirectToAction(nameof(Listar));
    }

    [HttpGet]
    public ActionResult Editar(Guid id)
    {
        throw new NotImplementedException();
    }

    [HttpPost]
    public ActionResult Editar(Guid id, EditarCategoriaViewModel viewModel)
    {
        throw new NotImplementedException();
    }

    [HttpGet]
    public ActionResult Excluir(Guid id)
    {
        throw new NotImplementedException();
    }

    [HttpPost]
    public ActionResult Excluir(ExcluirCategoriaViewModel viewModel)
    {
        throw new NotImplementedException();
    }
}