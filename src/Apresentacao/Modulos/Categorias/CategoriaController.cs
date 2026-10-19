using eAgenda.Aplicacao.Modulos.Categorias;
using eAgenda.Aplicacao.Modulos.Despesas;
using eAgenda.Dominio.Modulos.Despesas;
using FluentResults;
using Microsoft.AspNetCore.Mvc;

namespace eAgenda.WebApp.Modulos.Categorias;

public sealed class CategoriaController(ServicoCategoria servicoCategoria) : Controller
{
    [HttpGet]
    public ActionResult Listar()
    {
        var categorias = servicoCategoria.SelecionarTodos();

        return View("~/Modulos/Categorias/Views/Listar.cshtml", categorias);
    }

    [HttpGet]
    public ActionResult Cadastrar()
    {
        return View("~/Modulos/Categorias/Views/Cadastrar.cshtml");
    }

    [HttpPost]
    public ActionResult Cadastrar(CadastrarCategoriaViewModel viewModel)
    {
        if (string.IsNullOrWhiteSpace(viewModel.Titulo))
        {
            ModelState.AddModelError("Titulo", "O título é obrigatório.");
            return View("~/Modulos/Categorias/Views/Cadastrar.cshtml", viewModel);
        }

        var dto = new CadastrarCategoriaDto(viewModel.Titulo);
        var resultado = servicoCategoria.Cadastrar(dto);

        if (resultado.IsFailed)
        {
            foreach (var erro in resultado.Errors)
            {
                ModelState.AddModelError(string.Empty, erro.Message);
            }
            return View("~/Modulos/Categorias/Views/Cadastrar.cshtml", viewModel);
        }

        return RedirectToAction(nameof(Listar));
    }

    [HttpGet]
    public ActionResult Editar(Guid id)
    {
        var resultado = servicoCategoria.SelecionarPorId(id);

        if (resultado.IsFailed)
        {
            return RedirectToAction(nameof(Listar));
        }

        var categoriaDto = resultado.Value;

        var viewModel = new EditarCategoriaViewModel
        {
            Id = categoriaDto.Id,
            Titulo = categoriaDto.Titulo
        };

        return View("~/Modulos/Categorias/Views/Editar.cshtml", viewModel);
    }

    [HttpPost]
    public ActionResult Editar(EditarCategoriaViewModel viewModel)
    {

        if (!ModelState.IsValid)
        {
            return View("~/Modulos/Categorias/Views/Editar.cshtml", viewModel);
        }

        var dto = new EditarCategoriaDto(viewModel.Id, viewModel.Titulo);

        var resultado = servicoCategoria.Editar(dto);

        if (resultado.IsFailed)
        {
            foreach (var erro in resultado.Errors)
            {
                ModelState.AddModelError(string.Empty, erro.Message);
            }

            return View("~/Modulos/Categorias/Views/Editar.cshtml", viewModel);
        }

        return RedirectToAction(nameof(Listar));
    }

    [HttpGet]
    public ActionResult Excluir(Guid id)
    {
        var resultado = servicoCategoria.SelecionarPorId(id);

        if (resultado.IsFailed)
        {
            return RedirectToAction(nameof(Listar));
        }

        var categoriaDto = resultado.Value;

        var viewModel = new ExcluirCategoriaViewModel
        {
            Id = categoriaDto.Id,
            Titulo = categoriaDto.Titulo
        };

        return View("~/Modulos/Categorias/Views/Excluir.cshtml", viewModel);
    }

    [HttpPost]
    public ActionResult Excluir(ExcluirCategoriaViewModel viewModel)
    {
        var resultado = servicoCategoria.Excluir(viewModel.Id);

        if (resultado.IsFailed)
        {
            foreach (var erro in resultado.Errors)
            {
                ModelState.AddModelError(string.Empty, erro.Message);
            }

            return View("~/Modulos/Categorias/Views/Excluir.cshtml", viewModel);
        }

        return RedirectToAction(nameof(Listar));
    }
}