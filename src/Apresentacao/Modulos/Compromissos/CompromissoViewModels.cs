using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace eAgenda.WebApp.Modulos.Compromissos;

public record CadastrarCompromissoViewModel(
    [ValidateNever]
    string Assunto,

    [ValidateNever]
    string? Local,

    [ValidateNever]
    string? Link,

    [ValidateNever]
    DateTime Data,

    [ValidateNever]
    TimeSpan HoraInicio,

    [ValidateNever]
    TimeSpan HoraTermino,

    [ValidateNever]
    string TipoLocal, // Ex: "Presencial" ou "Remoto"

    [ValidateNever]
    Guid? ContatoId,

    [ValidateNever]
    IEnumerable<SelectListItem>? Contatos
)
{
    public CadastrarCompromissoViewModel() : this(string.Empty, null, null, DateTime.Today, TimeSpan.Zero, TimeSpan.Zero, "Presencial", null, null) { }
}

public record EditarCompromissoViewModel(
    Guid Id,

    [ValidateNever]
    string Assunto,

    [ValidateNever]
    string? Local,

    [ValidateNever]
    string? Link,

    [ValidateNever]
    DateTime Data,

    [ValidateNever]
    TimeSpan HoraInicio,

    [ValidateNever]
    TimeSpan HoraTermino,

    [ValidateNever]
    string TipoLocal,

    [ValidateNever]
    Guid? ContatoId,

    [ValidateNever]
    IEnumerable<SelectListItem>? Contatos
);

public record ExcluirCompromissoViewModel(
    Guid Id,
    string Assunto
);