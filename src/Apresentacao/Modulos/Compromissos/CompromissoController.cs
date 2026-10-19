using eAgenda.Aplicacao.Modulos.Compromissos;
using eAgenda.Dominio.Modulos.Compromissos;
using FluentResults;
using Microsoft.AspNetCore.Mvc;

namespace eAgenda.WebApp.Modulos.Compromissos;

public sealed class CompromissoController(ServicoCompromisso servicoCompromisso) : Controller
{
    private const string CaminhoViews = "~/Modulos/Compromissos/Views";

    [HttpGet]
    public ActionResult Listar()
    {
        List<CompromissoDto> compromissos = servicoCompromisso.SelecionarTodos();

        return View($"{CaminhoViews}/Listar.cshtml", compromissos);
    }

    [HttpGet]
    public IActionResult Cadastrar()
    {
        CadastrarCompromissoViewModel viewModel = new();
        return View($"{CaminhoViews}/Cadastrar.cshtml", viewModel);
    }

    [HttpPost]
    public ActionResult Cadastrar(CadastrarCompromissoViewModel viewModel)
    {
        TipoCompromisso tipoEnum = Enum.Parse<TipoCompromisso>(viewModel.TipoLocal);

        string localOuLink = tipoEnum == TipoCompromisso.Presencial
            ? (viewModel.Local ?? string.Empty)
            : (viewModel.Link ?? string.Empty);

        CadastrarCompromissoDto dto = new(
            viewModel.Assunto ?? string.Empty,
            DateOnly.FromDateTime(viewModel.Data),
            TimeOnly.FromTimeSpan(viewModel.HoraInicio),
            TimeOnly.FromTimeSpan(viewModel.HoraTermino),
            tipoEnum,
            localOuLink,
            viewModel.ContatoId
        );

        Result<Guid> resultado = servicoCompromisso.Cadastrar(dto);

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

            return View($"{CaminhoViews}/Cadastrar.cshtml", viewModel);
        }

        return RedirectToAction(nameof(Listar));
    }

    [HttpGet]
    public ActionResult Editar(Guid id)
    {
        Result<CompromissoDto> resultado = servicoCompromisso.SelecionarPorId(id);

        if (resultado.IsFailed)
            return RedirectToAction(nameof(Listar));

        CompromissoDto compromisso = resultado.Value;

        bool ehPresencial = compromisso.TipoCompromisso == TipoCompromisso.Presencial;

        EditarCompromissoViewModel viewModel = new(
            compromisso.Id,
            compromisso.Assunto,
            ehPresencial ? compromisso.LocalOuLink : null,
            !ehPresencial ? compromisso.LocalOuLink : null,
            compromisso.DataOcorrencia.ToDateTime(TimeOnly.MinValue),
            compromisso.HoraInicio.ToTimeSpan(),
            compromisso.HoraTermino.ToTimeSpan(),
            compromisso.TipoCompromisso.ToString(),
            compromisso.ContatoId,
            null
        );

        return View($"{CaminhoViews}/Editar.cshtml", viewModel);
    }

    [HttpPost]
    public ActionResult Editar(EditarCompromissoViewModel viewModel)
    {
        TipoCompromisso tipoEnum = Enum.Parse<TipoCompromisso>(viewModel.TipoLocal);

        string localOuLink = tipoEnum == TipoCompromisso.Presencial
            ? (viewModel.Local ?? string.Empty)
            : (viewModel.Link ?? string.Empty);

        EditarCompromissoDto dto = new(
            viewModel.Id,
            viewModel.Assunto ?? string.Empty,
            DateOnly.FromDateTime(viewModel.Data),
            TimeOnly.FromTimeSpan(viewModel.HoraInicio),
            TimeOnly.FromTimeSpan(viewModel.HoraTermino),
            tipoEnum,
            localOuLink,
            viewModel.ContatoId
        );

        Result resultado = servicoCompromisso.Editar(dto);

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
        Result<CompromissoDto> resultado = servicoCompromisso.SelecionarPorId(id);

        if (resultado.IsFailed)
            return RedirectToAction(nameof(Listar));

        CompromissoDto dto = resultado.Value;

        ExcluirCompromissoViewModel viewModel = new(dto.Id, dto.Assunto);

        return View($"{CaminhoViews}/Excluir.cshtml", viewModel);
    }

    [HttpPost]
    public ActionResult Excluir(ExcluirCompromissoViewModel viewModel)
    {
        Result resultado = servicoCompromisso.Excluir(viewModel.Id);

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