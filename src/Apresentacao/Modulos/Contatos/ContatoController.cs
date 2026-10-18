using eAgenda.Aplicacao.Modulos.Contatos;
using FluentResults;
using Microsoft.AspNetCore.Mvc;

namespace eAgenda.WebApp.Modulos.Contatos;

public sealed class ContatoController(ServicoContato servicoContato) : Controller
{
    [HttpGet]
    public ActionResult Listar()
    {
        List<ContatoDto> contatos = servicoContato.SelecionarTodos();

        return View(contatos);
    }

    [HttpGet]
    public IActionResult Cadastrar()
    {
        CadastrarContatoViewModel viewModel = new();
        return View(viewModel);
    }

    [HttpPost]
    public ActionResult Cadastrar(CadastrarContatoViewModel viewModel)
    {
        CadastrarContatoDto dto = new(
            viewModel.Nome ?? string.Empty,
            viewModel.Email ?? string.Empty,
            viewModel.Telefone ?? string.Empty,
            viewModel.Cargo,
            viewModel.Empresa
        );

        Result<Guid> resultado = servicoContato.Cadastrar(dto);

        if (resultado.IsFailed)
        {
            foreach (IError erro in resultado.Errors)
            {
                if (erro.Metadata.TryGetValue("Campo", out object? campoObj) && campoObj is string campo)
                {
                    ModelState.AddModelError(campo, erro.Message);
                }
                else
                {
                    ModelState.AddModelError(string.Empty, erro.Message);
                }
            }

            return View(viewModel);
        }

        return RedirectToAction(nameof(Listar));
    }

    [HttpGet]
    public ActionResult Editar(Guid id)
    {
        Result<ContatoDto> resultado = servicoContato.SelecionarPorId(id);

        if (resultado.IsFailed)
            return RedirectToAction(nameof(Listar));

        ContatoDto contato = resultado.Value;

        EditarContatoViewModel viewModel = new(
            contato.Id,
            contato.Nome,
            contato.Email,
            contato.Telefone,
            contato.Cargo,
            contato.Empresa
        );

        return View(viewModel);
    }

    [HttpPost]
    public ActionResult Editar(EditarContatoViewModel viewModel)
    {
        EditarContatoDto dto = new(
            viewModel.Id,
            viewModel.Nome ?? string.Empty,
            viewModel.Email ?? string.Empty,
            viewModel.Telefone ?? string.Empty,
            viewModel.Cargo,
            viewModel.Empresa
        );

        Result resultado = servicoContato.Editar(dto);

        if (resultado.IsFailed)
        {
            foreach (IError erro in resultado.Errors)
            {
                if (erro.Metadata.TryGetValue("Campo", out object? campoObj) && campoObj is string campo)
                {
                    ModelState.AddModelError(campo, erro.Message);
                }
                else
                {
                    ModelState.AddModelError(string.Empty, erro.Message);
                }
            }

            return View(viewModel);
        }

        return RedirectToAction(nameof(Listar));
    }

    [HttpGet]
    public ActionResult Excluir(Guid id)
    {
        Result<ContatoDto> resultado = servicoContato.SelecionarPorId(id);

        if (resultado.IsFailed)
            return RedirectToAction(nameof(Listar));

        ContatoDto dto = resultado.Value;

        ExcluirContatoViewModel viewModel = new(dto.Id, dto.Nome);

        return View(viewModel);
    }

    [HttpPost]
    public ActionResult Excluir(ExcluirContatoViewModel viewModel)
    {
        Result resultado = servicoContato.Excluir(viewModel.Id);

        if (resultado.IsFailed)
        {
            foreach (IError erro in resultado.Errors)
            {
                ModelState.AddModelError(string.Empty, erro.Message);
            }

            return View(viewModel);
        }

        return RedirectToAction(nameof(Listar));
    }
}