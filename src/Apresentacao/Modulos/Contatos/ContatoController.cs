using eAgenda.Aplicacao.Modulos.Contatos;
using eAgenda.Dominio.Modulos.Contatos;
using FluentResults;
using Microsoft.AspNetCore.Mvc;

namespace eAgenda.WebApp.Modulos.Contatos;

public sealed class ContatoController(ServicoContato servicoContato) : Controller
{
    private const string CaminhoViews = "~/Modulos/Contatos/Views";

    [HttpGet]
    public ActionResult Listar()
    {
        List<ContatoDto> contatos = servicoContato.SelecionarTodos();

        return View($"{CaminhoViews}/Listar.cshtml", contatos);
    }

    [HttpGet]
    public IActionResult Cadastrar()
    {
        CadastrarContatoViewModel viewModel = new();
        return View($"{CaminhoViews}/Cadastrar.cshtml", viewModel);
    }

    [HttpPost]
    public ActionResult Cadastrar(CadastrarContatoViewModel viewModel)
    {
        if (!ModelState.IsValid)
            return View($"{CaminhoViews}/Cadastrar.cshtml", viewModel);

        CadastrarContatoDto dto = new(
            viewModel.Nome ?? string.Empty,
            viewModel.Email ?? string.Empty,
            viewModel.Telefone ?? string.Empty,
            viewModel.Cargo ?? string.Empty,
            viewModel.Empresa ?? string.Empty
        );

        Result<Guid> resultado = servicoContato.Cadastrar(dto);

        if (resultado.IsFailed)
        {
            foreach (var erro in resultado.Errors)
            {
                // Converte o object do metadata para string de forma segura
                string campo = string.Empty;
                if (erro.Metadata.ContainsKey("Campo") && erro.Metadata["Campo"] is string valorCampo)
                {
                    campo = valorCampo;
                }

                ModelState.AddModelError(campo, erro.Message);
            }

            return View($"{CaminhoViews}/Cadastrar.cshtml", viewModel);
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


        EditarContatoViewModel viewModel = new()
        {
            Id = contato.Id,
            Nome = contato.Nome,
            Email = contato.Email,
            Telefone = contato.Telefone,
            Empresa = contato.Empresa,
            Cargo = contato.Cargo
        };

        return View($"{CaminhoViews}/Editar.cshtml", viewModel);
    }

    [HttpPost]
    public ActionResult Editar(EditarContatoViewModel viewModel)
    {

        EditarContatoDto dto = new(
            viewModel.Id,
            viewModel.Nome ?? string.Empty,
            viewModel.Email ?? string.Empty,
            viewModel.Telefone ?? string.Empty,
            viewModel.Cargo ?? string.Empty,
            viewModel.Empresa ?? string.Empty
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

            return View($"{CaminhoViews}/Editar.cshtml", viewModel);
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

        ExcluirContatoViewModel viewModel = new()
        {
            Id = dto.Id,
            Nome = dto.Nome
        };

        return View($"{CaminhoViews}/Excluir.cshtml", viewModel);
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

            return View($"{CaminhoViews}/Excluir.cshtml", viewModel);
        }

        return RedirectToAction(nameof(Listar));
    }
}