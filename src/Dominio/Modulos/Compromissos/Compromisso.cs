using System;
using eAgenda.Dominio.Compartilhado;
using eAgenda.Dominio.Modulos.Contatos;
using eAgenda.Dominio.Modulos.Compromissos;

namespace eAgenda.Dominio.Modulos.Compromissos;

public sealed class Compromisso : EntidadeBase<Compromisso>
{
    public string Assunto { get; set; } = string.Empty;
    public DateOnly DataOcorrencia { get; set; } = DateOnly.FromDateTime(DateTime.UtcNow);
    public TimeOnly HoraInicio { get; set; }
    public TimeOnly HoraTermino { get; set; }
    public TipoCompromisso TipoCompromisso { get; set; }
    public string LocalOuLink { get; set; } = string.Empty;

    public Guid? ContatoId { get; set; }
    public Contato? Contato { get; set; }

    public Compromisso() { }

    public Compromisso(
        string assunto,
        DateOnly dataOcorrencia,
        TimeOnly horaInicio,
        TimeOnly horaTermino,
        TipoCompromisso tipoCompromisso,
        string localOuLink,
        Guid? contatoId,
        Contato? contato
    )
    {
        Assunto = assunto ?? string.Empty;
        DataOcorrencia = dataOcorrencia;
        HoraInicio = horaInicio;
        HoraTermino = horaTermino;
        TipoCompromisso = tipoCompromisso;
        LocalOuLink = localOuLink ?? string.Empty;
        ContatoId = contatoId;
        Contato = contato;
    }

    public override List<ErroValidacao> Validar()
    {
        List<ErroValidacao> erros = [];

        if (string.IsNullOrWhiteSpace(Assunto) || Assunto.Length < 2 || Assunto.Length > 100)
        {
            erros.Add(new ErroValidacao
            {
                Campo = nameof(Assunto),
                Mensagem = "O campo \"Assunto\" deve conter entre 2 e 100 caracteres."
            });
        }

        if (DataOcorrencia == DateOnly.MinValue)
        {
            erros.Add(new ErroValidacao
            {
                Campo = nameof(DataOcorrencia),
                Mensagem = "O campo \"Data de Ocorrência\" deve conter uma data válida."
            });
        }

        if (HoraTermino <= HoraInicio)
        {
            erros.Add(new ErroValidacao
            {
                Campo = nameof(HoraTermino),
                Mensagem = "O campo \"Hora de Término\" deve ser posterior à Hora de Início."
            });
        }

        if (string.IsNullOrWhiteSpace(LocalOuLink))
        {
            erros.Add(new ErroValidacao
            {
                Campo = nameof(LocalOuLink),
                Mensagem = "O campo \"Local ou Link\" é obrigatório."
            });
        }

        return erros;
    }

    public override void Atualizar(Compromisso entidadeAtualizada)
    {
        Assunto = entidadeAtualizada.Assunto;
        DataOcorrencia = entidadeAtualizada.DataOcorrencia;
        HoraInicio = entidadeAtualizada.HoraInicio;
        HoraTermino = entidadeAtualizada.HoraTermino;
        TipoCompromisso = entidadeAtualizada.TipoCompromisso;
        LocalOuLink = entidadeAtualizada.LocalOuLink;
        ContatoId = entidadeAtualizada.ContatoId;
        Contato = entidadeAtualizada.Contato;
    }
}