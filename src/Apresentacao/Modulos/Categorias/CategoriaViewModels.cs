using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace eAgenda.WebApp.Modulos.Categorias;

public class CadastrarCategoriaViewModel
{
    public string Titulo { get; set; } = string.Empty;
}

public class EditarCategoriaViewModel
{
    public Guid Id { get; set; }

    [ValidateNever]
    public string Titulo { get; set; } = string.Empty;
}

public class ExcluirCategoriaViewModel
{
    public Guid Id { get; set; }
    public string Titulo { get; set; } = string.Empty;
}