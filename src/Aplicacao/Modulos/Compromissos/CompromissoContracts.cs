using eAgenda.Dominio.Modulos.Compromissos;
namespace eAgenda.Aplicacao.Modulos.Compromissos;

public record CompromissoDto(
    Guid Id,
    string Assunto,
    DateOnly DataOcorrencia,
    TimeOnly HoraInicio,
    TimeOnly HoraTermino,
    TipoCompromisso TipoCompromisso,
    string LocalOuLink,
    Guid? ContatoId,
    string? NomeContato
);

public record CadastrarCompromissoDto(
    string Assunto,
    DateOnly DataOcorrencia,
    TimeOnly HoraInicio,
    TimeOnly HoraTermino,
    TipoCompromisso TipoCompromisso,
    string LocalOuLink,
    Guid? ContatoId
);

public record EditarCompromissoDto(
    Guid Id,
    string Assunto,
    DateOnly DataOcorrencia,
    TimeOnly HoraInicio,
    TimeOnly HoraTermino,
    TipoCompromisso TipoCompromisso,
    string LocalOuLink,
    Guid? ContatoId
);