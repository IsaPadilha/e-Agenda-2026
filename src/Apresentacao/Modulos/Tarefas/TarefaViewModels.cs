using eAgenda.Dominio.Modulos.Tarefas;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace eAgenda.WebApp.Modulos.Tarefas;

public class CadastrarTarefaViewModel
{
    public string Titulo { get; set; } = string.Empty;
    public PrioridadeTarefa Prioridade { get; set; }
}

public class EditarTarefaViewModel
{
    public Guid Id { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public PrioridadeTarefa Prioridade { get; set; }
}

public class ExcluirTarefaViewModel
{
    public Guid Id { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public PrioridadeTarefa Prioridade { get; set; }
    public DateOnly DataCriacao { get; set; }
}

public class GerenciarItensTarefaViewModel
{
    public Guid Id { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public PrioridadeTarefa Prioridade { get; set; }
    public DateOnly DataCriacao { get; set; }
    public DateOnly? DataConclusao { get; set; }
    public int PercentualConcluido { get; set; }
    public bool Concluida { get; set; }
    public List<ItemTarefaViewModel> Itens { get; set; } = [];
}

public class AdicionarItemTarefaViewModel
{
    public Guid TarefaId { get; set; }
    public string Titulo { get; set; } = string.Empty;
}

public class ItemTarefaViewModel
{
    public Guid Id { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public bool Concluido { get; set; }
}