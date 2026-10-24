using eAgenda.Aplicacao.Modulos.Tarefas;
using eAgenda.Dominio.Modulos.Tarefas;
using FluentResults;
using Microsoft.AspNetCore.Mvc;

namespace eAgenda.WebApp.Modulos.Tarefas;

public class TarefaController(ServicoTarefa servicoTarefa) : Controller
{
    private const string CaminhoViews = "~/Modulos/Tarefas/Views";

    [HttpGet]
    public ActionResult Listar()
    {
        List<TarefaDto> dtos = servicoTarefa.SelecionarTodas();

        return View($"{CaminhoViews}/Listar.cshtml", dtos);
    }

    [HttpGet]
    public ActionResult Cadastrar()
    {
        var viewModel = new CadastrarTarefaViewModel
        {
            Titulo = string.Empty,
            Prioridade = PrioridadeTarefa.Normal
        };

        return View($"{CaminhoViews}/Cadastrar.cshtml", viewModel);
    }

    [HttpPost]
    public ActionResult Cadastrar(CadastrarTarefaViewModel viewModel)
    {
        CadastrarTarefaDto dto = new(
            viewModel.Titulo,
            viewModel.Prioridade
        );

        Result<Guid> resultado = servicoTarefa.Cadastrar(dto);

        if (resultado.IsFailed)
        {
            foreach (IError erro in resultado.Errors)
            {
                string campo = erro.Metadata.TryGetValue("Campo", out var valor)
                    ? valor?.ToString() ?? string.Empty
                    : string.Empty;

                ModelState.AddModelError(campo, erro.Message);
            }

            return View($"{CaminhoViews}/Cadastrar.cshtml", viewModel);
        }

        return RedirectToAction(nameof(Listar));
    }

    [HttpGet]
    public ActionResult Editar(Guid id)
    {
        var resultado = servicoTarefa.SelecionarPorId(id);

        if (resultado.IsFailed)
        {
            TempData["MensagemErro"] = resultado.Errors.First().Message;
            return RedirectToAction(nameof(Listar));
        }

        var dto = resultado.Value;

        var viewModel = new EditarTarefaViewModel
        {
            Id = dto.Id,
            Titulo = dto.Titulo,
            Prioridade = dto.Prioridade
        };

        return View($"{CaminhoViews}/Editar.cshtml", viewModel);
    }

    [HttpPost]
    public ActionResult Editar(EditarTarefaViewModel viewModel)
    {
        if (!ModelState.IsValid)
        {
            return View($"{CaminhoViews}/Editar.cshtml", viewModel);
        }

        var dto = new EditarTarefaDto(viewModel.Id, viewModel.Titulo, viewModel.Prioridade);
        var resultado = servicoTarefa.Editar(dto);

        if (resultado.IsFailed)
        {
            foreach (var erro in resultado.Errors)
            {
                ModelState.AddModelError(string.Empty, erro.Message);
            }
            return View($"{CaminhoViews}/Editar.cshtml", viewModel);
        }

        return RedirectToAction(nameof(Listar));
    }

    [HttpGet]
    public ActionResult Excluir(Guid id)
    {
        var resultado = servicoTarefa.SelecionarPorId(id);

        if (resultado.IsFailed)
        {
            TempData["MensagemErro"] = resultado.Errors.First().Message;
            return RedirectToAction(nameof(Listar));
        }

        var dto = resultado.Value;

        var viewModel = new ExcluirTarefaViewModel
        {
            Id = dto.Id,
            Titulo = dto.Titulo,
            Prioridade = dto.Prioridade,
            DataCriacao = dto.DataCriacao
        };

        return View($"{CaminhoViews}/Excluir.cshtml", viewModel);
    }

    [HttpPost]
    public ActionResult Excluir(ExcluirTarefaViewModel viewModel)
    {
        var resultado = servicoTarefa.Excluir(viewModel.Id);

        if (resultado.IsFailed)
        {
            foreach (var erro in resultado.Errors)
            {
                ModelState.AddModelError(string.Empty, erro.Message);
            }
            return View($"{CaminhoViews}/Excluir.cshtml", viewModel);
        }

        return RedirectToAction(nameof(Listar));
    }

    [HttpGet]
    public ActionResult GerenciarItens(Guid tarefaId)
    {
        Result<TarefaDto> resultado = servicoTarefa.SelecionarPorId(tarefaId);

        if (resultado.IsFailed)
        {
            TempData["MensagemErro"] = resultado.Errors.First().Message;

            return RedirectToAction(nameof(Listar));
        }

        TarefaDto dto = resultado.Value;

        GerenciarItensTarefaViewModel viewModel = new()
        {
            Id = dto.Id,
            Titulo = dto.Titulo,
            Prioridade = dto.Prioridade,
            DataCriacao = dto.DataCriacao,
            DataConclusao = dto.DataConclusao,
            PercentualConcluido = dto.PercentualConcluido,
            Concluida = dto.Concluida,
            Itens = dto.Itens.Select(i => new ItemTarefaViewModel
            {
                Id = i.Id,
                Titulo = i.Titulo,
                Concluido = i.Concluido
            }).ToList()
        };

        return View($"{CaminhoViews}/GerenciarItens.cshtml", viewModel);
    }

    [HttpPost]
    public ActionResult AdicionarItem(AdicionarItemTarefaViewModel viewModel)
    {
        AdicionarItemTarefaDto dto = new(viewModel.TarefaId, viewModel.Titulo);

        Result resultado = servicoTarefa.AdicionarItem(dto);

        if (resultado.IsFailed)
            TempData["MensagemErro"] = resultado.Errors.First().Message;

        return RedirectToAction(nameof(GerenciarItens), new { tarefaId = viewModel.TarefaId });
    }

    [HttpPost]
    public ActionResult AlterarConclusaoItem(Guid tarefaId, Guid itemId, bool concluido)
    {
        AlterarConclusaoItemTarefaDto dto = new(
            tarefaId,
            itemId,
            concluido
        );

        Result resultado = servicoTarefa.AlterarConclusaoItem(dto);

        if (resultado.IsFailed)
            TempData["MensagemErro"] = resultado.Errors.First().Message;

        return RedirectToAction(nameof(GerenciarItens), new { tarefaId });
    }

    [HttpPost]
    public ActionResult RemoverItem(Guid tarefaId, Guid itemId)
    {
        RemoverItemTarefaDto dto = new(tarefaId, itemId);

        Result resultado = servicoTarefa.RemoverItem(dto);

        if (resultado.IsFailed)
            TempData["MensagemErro"] = resultado.Errors.First().Message;

        return RedirectToAction(nameof(GerenciarItens), new { tarefaId });
    }
}