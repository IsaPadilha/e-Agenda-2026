using eAgenda.Aplicacao.Modulos.Categorias;
using eAgenda.Aplicacao.Modulos.Despesas;
using eAgenda.Dominio.Modulos.Despesas;
using FluentResults;
using Microsoft.AspNetCore.Mvc;

namespace eAgenda.WebApp.Modulos.Despesas;

public sealed class DespesaController(
    ServicoDespesa servicoDespesa,
    ServicoCategoria servicoCategoria
) : Controller
{
    private const string CaminhoViews = "~/Modulos/Despesas/Views";

    [HttpGet]
    public ActionResult Listar()
    {
        List<DespesaDto> dtos = servicoDespesa.SelecionarTodos();

        return View($"{CaminhoViews}/Listar.cshtml", dtos);
    }

    [HttpGet]
    public ActionResult Cadastrar()
    {
        List<CategoriaDespesaDto> categoriasDtos = servicoCategoria
            .SelecionarTodos()
            .Select(c => new CategoriaDespesaDto(c.Id, c.Titulo))
            .ToList();

        CadastrarDespesaViewModel viewModel = new()
        {
            Descricao = string.Empty,
            DataOcorrencia = DateTime.Today.ToString("yyyy-MM-dd"),
            Valor = 0,
            FormaPagamento = FormaPagamento.AVista,
            CategoriasDisponiveis = categoriasDtos
        };

        return View($"{CaminhoViews}/Cadastrar.cshtml", viewModel);
    }

    [HttpPost]
    public ActionResult Cadastrar(CadastrarDespesaViewModel viewModel)
    {
        if (viewModel == null)
        {
            TempData["MensagemErro"] = "Os dados enviados são inválidos.";
            return RedirectToAction(nameof(Listar));
        }

        string formaPagamentoStr = Request.Form["FormaPagamento"];

        int formaPagamentoInt = formaPagamentoStr switch
        {
            "AVista" => (int)FormaPagamento.AVista,
            "Credito" => (int)FormaPagamento.Credito,
            "Debito" => (int)FormaPagamento.Debito,
            _ => (int)FormaPagamento.AVista
        };

        var categoriaIds = Request.Form["CategoriasIds"]
            .Where(x => !string.IsNullOrEmpty(x) && Guid.TryParse(x, out _))
            .Select(Guid.Parse)
            .ToList();

        if (!DateOnly.TryParse(viewModel.DataOcorrencia, out DateOnly dataConvertida))
        {
            ModelState.AddModelError("DataOcorrencia", "O campo Data de Ocorrência deve conter uma data válida.");
        }

        if (!ModelState.IsValid)
        {
            viewModel.CategoriasDisponiveis = servicoCategoria
                .SelecionarTodos()
                .Select(c => new CategoriaDespesaDto(c.Id, c.Titulo))
                .ToList();

            return View($"{CaminhoViews}/Cadastrar.cshtml", viewModel);
        }

        CadastrarDespesaDto dto = new(
            viewModel.Descricao,
            dataConvertida,
            viewModel.Valor,
            (FormaPagamento)formaPagamentoInt,
            categoriaIds
        );

        Result<Guid> resultado = servicoDespesa.Cadastrar(dto);

        if (resultado.IsFailed)
        {
            List<CategoriaDespesaDto> categoriasDtos = servicoCategoria
                .SelecionarTodos()
                .Select(c => new CategoriaDespesaDto(c.Id, c.Titulo))
                .ToList();

            viewModel.CategoriasDisponiveis = categoriasDtos;

            if (!resultado.Errors.Any(e => e.HasMetadataKey("Campo")))
            {
                TempData["MensagemErro"] = resultado.Errors.First().Message;
                return View($"{CaminhoViews}/Cadastrar.cshtml", viewModel);
            }

            foreach (IError erro in resultado.Errors)
            {
                string campo = erro.Metadata.TryGetValue("Campo", out var valor) ? valor?.ToString() ?? string.Empty : string.Empty;

                ModelState.AddModelError(campo, erro.Message);
            }

            return View($"{CaminhoViews}/Cadastrar.cshtml", viewModel);
        }

        return RedirectToAction(nameof(Listar));
    }

    [HttpGet]
    public ActionResult Editar(Guid id)
    {
        Result<DespesaDto> resultado = servicoDespesa.SelecionarPorId(id);

        if (resultado.IsFailed)
        {
            TempData["MensagemErro"] = resultado.Errors.First().Message;

            return RedirectToAction(nameof(Listar));
        }

        DespesaDto dto = resultado.Value;

        List<CategoriaDespesaDto> categoriasDtos = servicoCategoria
            .SelecionarTodos()
            .Select(c => new CategoriaDespesaDto(c.Id, c.Titulo))
            .ToList();

        EditarDespesaViewModel viewModel = new()
        {
            Id = id,
            Descricao = dto.Descricao,
            DataOcorrencia = dto.Data.ToString("yyyy-MM-dd"),
            Valor = dto.Valor,
            FormaPagamento = dto.FormaPagamento,
            CategoriasIds = dto.Categorias.Select(c => c.Id).ToList(),
            CategoriasDisponiveis = categoriasDtos
        };
        return View($"{CaminhoViews}/Editar.cshtml", viewModel);
    }

    [HttpPost]
    public ActionResult Editar(EditarDespesaViewModel viewModel)
    {
        if (viewModel == null)
        {
            TempData["MensagemErro"] = "Os dados enviados são inválidos.";
            return RedirectToAction(nameof(Listar));
        }

        if (!DateOnly.TryParse(viewModel.DataOcorrencia, out DateOnly dataConvertida))
        {
            ModelState.AddModelError("DataOcorrencia", "O campo Data de Ocorrência deve conter uma data válida.");
        }

        if (!ModelState.IsValid)
        {
            viewModel.CategoriasDisponiveis = servicoCategoria
                .SelecionarTodos()
                .Select(c => new CategoriaDespesaDto(c.Id, c.Titulo))
                .ToList();

            return View($"{CaminhoViews}/Editar.cshtml", viewModel);
        }

        EditarDespesaDto dto = new(
            viewModel.Id,
            viewModel.Descricao,
            dataConvertida,
            viewModel.Valor,
            viewModel.FormaPagamento,
            viewModel.CategoriasIds
        );

        Result resultado = servicoDespesa.Editar(dto);

        if (resultado.IsFailed)
        {
            List<CategoriaDespesaDto> categoriasDtos = servicoCategoria
                .SelecionarTodos()
                .Select(c => new CategoriaDespesaDto(c.Id, c.Titulo))
                .ToList();

            viewModel.CategoriasDisponiveis = categoriasDtos;

            foreach (var erro in resultado.Errors)
            {
                string campo = erro.Metadata.TryGetValue("Campo", out var valor)
                    ? valor?.ToString() ?? string.Empty
                    : string.Empty;

                ModelState.AddModelError(campo, erro.Message);
            }

            return View($"{CaminhoViews}/Editar.cshtml", viewModel);
        }

        return RedirectToAction(nameof(Listar));
    }

    [HttpGet]
    public ActionResult Excluir(Guid id)
    {
        Result<DespesaDto> resultado = servicoDespesa.SelecionarPorId(id);

        if (resultado.IsFailed)
        {
            TempData["MensagemErro"] = resultado.Errors.First().Message;

            return RedirectToAction(nameof(Listar));
        }

        DespesaDto dto = resultado.Value;

        ExcluirDespesaViewModel viewModel = new()
        {
            Id = dto.Id,
            Descricao = dto.Descricao,
            DataOcorrencia = dto.Data.ToString("yyyy-MM-dd"),
            Valor = dto.Valor,
            FormaPagamento = dto.FormaPagamento
        };

        return View($"{CaminhoViews}/Excluir.cshtml", viewModel);
    }

    [HttpPost]
    public ActionResult Excluir(ExcluirDespesaViewModel excluirVm)
    {
        Result resultado = servicoDespesa.Excluir(excluirVm.Id);

        if (resultado.IsFailed)
            TempData["MensagemErro"] = resultado.Errors.First().Message;

        return RedirectToAction(nameof(Listar));
    }
}