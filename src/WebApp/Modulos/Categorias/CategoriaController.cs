using eAgenda.Aplicacao.Modulos.Categorias;
using FluentResults;
using Microsoft.AspNetCore.Mvc;

namespace eAgenda.WebApp.Modulos.Categorias;

public sealed class CategoriaController(ServicoCategoria servicoCategoria) : Controller
{
    [HttpGet]
    public ActionResult Listar()
    {
        List<CategoriaDto> categorias = servicoCategoria.SelecionarTodos();

        return View(categorias);
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
        Result<Guid> resultado = servicoCategoria
            .Cadastrar(new CadastrarCategoriaDto(viewModel.Titulo));

        if (resultado.IsFailed)
        {
            string mensagemErro = resultado.Errors.Select(e => e.Message).First();

            ModelState.AddModelError(string.Empty, mensagemErro);

            return View(viewModel);
        }

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