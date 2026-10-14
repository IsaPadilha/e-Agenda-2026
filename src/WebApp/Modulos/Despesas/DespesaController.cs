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
    [HttpGet]
    public ActionResult Listar()
    {
        List<DespesaDto> dtos = servicoDespesa.SelecionarTodos();

        return View(dtos);
    }

    [HttpGet]
    public ActionResult Cadastrar()
    {
        List<CategoriaDespesaDto> categoriasDtos = servicoCategoria
            .SelecionarTodos()
            .Select(c => new CategoriaDespesaDto(c.Id, c.Titulo))
            .ToList();

        CadastrarDespesaViewModel viewModel = new(
            string.Empty,
            DateOnly.FromDateTime(DateTime.UtcNow),
            0,
            FormaPagamento.AVista,
            []
        );

        return View(viewModel with { CategoriasDisponiveis = categoriasDtos });
    }

    [HttpPost]
    public ActionResult Cadastrar(CadastrarDespesaViewModel viewModel)
    {
        CadastrarDespesaDto dto = new(
            viewModel.Descricao,
            viewModel.DataOcorrencia,
            viewModel.Valor,
            viewModel.FormaPagamento,
            viewModel.CategoriasIds
        );

        Result<Guid> resultado = servicoDespesa.Cadastrar(dto);

        if (resultado.IsFailed)
        {
            foreach (IError erro in resultado.Errors)
            {
                string campo = erro.Metadata["Campo"].ToString() ?? string.Empty;

                ModelState.AddModelError(campo, erro.Message);
            }

            List<CategoriaDespesaDto> categoriasDtos = servicoCategoria
                .SelecionarTodos()
                .Select(c => new CategoriaDespesaDto(c.Id, c.Titulo))
                .ToList();

            return View(viewModel with { CategoriasDisponiveis = categoriasDtos });
        }

        return RedirectToAction(nameof(Listar));
    }
}