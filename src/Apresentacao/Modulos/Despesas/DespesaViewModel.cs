using eAgenda.Aplicacao.Modulos.Despesas;
using eAgenda.Dominio.Modulos.Despesas;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

public class CadastrarDespesaViewModel
{
    public string Descricao { get; set; } = string.Empty;
    public string DataOcorrencia { get; set; } = DateTime.Today.ToString("yyyy-MM-dd");
    public decimal Valor { get; set; }
    public FormaPagamento FormaPagamento { get; set; }
    public List<Guid> CategoriasIds { get; set; } = [];

    [ValidateNever]
    public List<CategoriaDespesaDto> CategoriasDisponiveis { get; set; } = [];
}

public class EditarDespesaViewModel
{
    public Guid Id { get; set; }
    public string Descricao { get; set; } = string.Empty;
    public string DataOcorrencia { get; set; } = string.Empty;
    public decimal Valor { get; set; }
    public FormaPagamento FormaPagamento { get; set; }
    public List<Guid> CategoriasIds { get; set; } = [];

    [ValidateNever]
    public List<CategoriaDespesaDto> CategoriasDisponiveis { get; set; } = [];
}

public class ExcluirDespesaViewModel
{
    public Guid Id { get; set; }
    public string Descricao { get; set; } = string.Empty;
    public string DataOcorrencia { get; set; } = string.Empty;
    public decimal Valor { get; set; }
    public FormaPagamento FormaPagamento { get; set; }
}