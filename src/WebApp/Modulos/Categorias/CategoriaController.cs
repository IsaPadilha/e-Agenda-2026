using eAgenda.Aplicacao.Modulos.Categorias;
using eAgenda.Infraestrutura.Modulos.Categorias;
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
        Result<CategoriaDto> resultado = servicoCategoria.SelecionarPorId(id);

        if (resultado.IsFailed)
            return RedirectToAction(nameof(Listar));

        CategoriaDto dto = resultado.Value;

        EditarCategoriaViewModel viewModel = new(
            dto.Id,
            dto.Titulo
        );

        return View(viewModel);
    }

    [HttpPost]
    public ActionResult Editar(EditarCategoriaViewModel viewModel)
    {
        EditarCategoriaDto dto = new(viewModel.Id, viewModel.Titulo);

        Result resultado = servicoCategoria.Editar(dto);

        if (resultado.IsFailed)
        {
            string mensagemErro = resultado.Errors.Select(e => e.Message).First();

            ModelState.AddModelError(string.Empty, mensagemErro);

            return View(viewModel);
        }

        return RedirectToAction(nameof(Listar));
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