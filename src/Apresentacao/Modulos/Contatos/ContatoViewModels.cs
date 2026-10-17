using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace eAgenda.WebApp.Modulos.Contatos;

public record CadastrarContatoViewModel(
    [ValidateNever]
    string Nome,

    [ValidateNever]
    string Email,

    [ValidateNever]
    string Telefone,

    [ValidateNever]
    string? Cargo,

    [ValidateNever]
    string? Empresa
);

public record EditarContatoViewModel(
    Guid Id,

    [ValidateNever]
    string Nome,

    [ValidateNever]
    string Email,

    [ValidateNever]
    string Telefone,

    [ValidateNever]
    string? Cargo,

    [ValidateNever]
    string? Empresa
);

public record ExcluirContatoViewModel(
    Guid Id,
    string Nome
);