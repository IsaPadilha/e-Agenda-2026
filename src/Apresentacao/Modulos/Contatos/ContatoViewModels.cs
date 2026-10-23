namespace eAgenda.WebApp.Modulos.Contatos;

public class CadastrarContatoViewModel
{
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Telefone { get; set; } = string.Empty;
    public string? Empresa { get; set; }
    public string? Cargo { get; set; }
}

public class EditarContatoViewModel
{
    public Guid Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Telefone { get; set; } = string.Empty;
    public string? Empresa { get; set; }
    public string? Cargo { get; set; }
}

public class ExcluirContatoViewModel
{
    public Guid Id { get; set; }
    public string Nome { get; set; } = string.Empty;
}