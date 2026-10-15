using eAgenda.Dominio.Modulos.Tarefas;

namespace eAgenda.Aplicacao.Modulos.Tarefas;

public record TarefaDto(
    Guid Id,
    string Titulo,
    PrioridadeTarefa Prioridade,
    DateOnly DataCriacao
);

public record CadastrarTarefaDto(
    string Titulo,
    PrioridadeTarefa Prioridade
);

public record EditarTarefaDto(
    Guid Id,
    string Titulo,
    PrioridadeTarefa Prioridade
);