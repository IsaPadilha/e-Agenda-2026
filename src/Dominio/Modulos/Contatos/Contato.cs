using eAgenda.Dominio.Compartilhado;

namespace eAgenda.Dominio.Modulos.Contatos;

public sealed class Contato : EntidadeBase<Contato>
{
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Telefone { get; set; } = string.Empty;
    public string? Cargo { get; set; }
    public string? Empresa { get; set; }

    //public List<Compromisso> Compromissos { get; set; } = [];

    public Contato() { }
    public Contato(string nome, string email, string telefone, string? cargo, string? empresa) : this()
    {
        Nome = nome;
        Email = email;
        Telefone = telefone;
        Cargo = cargo;
        Empresa = empresa;
    }

    public override List<ErroValidacao> Validar()
    {
        List<ErroValidacao> erros = [];

        if (string.IsNullOrWhiteSpace(Nome))
        {
            erros.Add(new ErroValidacao
            {
                Campo = nameof(Nome),
                Mensagem = "O campo\"Nome\" deve ser preenchido"
            });
        }
        else if (Nome.Length < 2 || Nome.Length > 100)
        {
            erros.Add(new ErroValidacao
            {
                Campo = nameof(Nome),
                Mensagem = "O campo \"Nome\" deve conter entre 2 e 100 caracteres"
            });
        }

        if (string.IsNullOrWhiteSpace(Email))
        {
            erros.Add(new ErroValidacao
            {
                Campo = nameof(Email),
                Mensagem = "O campo \"Email\" deve ser preenchido"
            });
        }
        else if (!Email.Contains('@') || !Email.Contains('.'))
        {
            erros.Add(new ErroValidacao
            {
                Campo = nameof(Email),
                Mensagem = "O campo \"Email\" deve possuir um formato válido."
            });
        }

        if (string.IsNullOrWhiteSpace(Telefone))
        {
            erros.Add(new ErroValidacao
            {
                Campo = nameof(Telefone),
                Mensagem = "O campo \"Telefone\" deve ser preenchido"
            });
        }

        else
        {
            string padraoTelefone = @"^\(\d{2}\)\s\d{4,5}-\d{4}$";
            if (!System.Text.RegularExpressions.Regex.IsMatch(Telefone, padraoTelefone))
            {
                erros.Add(new ErroValidacao
                {
                    Campo = nameof(Telefone),
                    Mensagem = "O campo \"Telefone\" deve estar no formato (XX) XXXX-XXXX ou (XX) XXXXX-XXXX"
                });
            }
        }

        return erros;
    }

    public override void Atualizar(Contato entidadeAtualizada)
    {
        {
            Nome = entidadeAtualizada.Nome;
            Email = entidadeAtualizada.Email;
            Telefone = entidadeAtualizada.Telefone;
            Cargo = entidadeAtualizada.Cargo;
            Empresa = entidadeAtualizada.Empresa;
        }
    }

}