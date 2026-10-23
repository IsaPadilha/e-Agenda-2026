using System;
using eAgenda.Dominio.Compartilhado;
using eAgenda.Dominio.Modulos.Categorias;

namespace eAgenda.Dominio.Modulos.Despesas;

public sealed class Despesa : EntidadeBase<Despesa>
{
    public string Descricao { get; set; } = string.Empty;
    public DateOnly Data { get; set; } = DateOnly.FromDateTime(DateTime.UtcNow);
    public decimal Valor { get; set; }
    public FormaPagamento FormaPagamento { get; set; }
    public List<Categoria> Categorias { get; set; } = [];

    public Despesa() { }

    public Despesa(
        string descricao,
        DateOnly data,
        decimal valor,
        FormaPagamento formaPagamento,
        List<Categoria> categorias
    )
    {
        Descricao = descricao ?? string.Empty;
        Data = data;
        Valor = valor;
        FormaPagamento = formaPagamento;
        Categorias = categorias;
    }

    public override List<ErroValidacao> Validar()
    {
        List<ErroValidacao> erros = [];

        if (string.IsNullOrWhiteSpace(Descricao) || Descricao.Length < 2 || Descricao.Length > 100)
        {
            erros.Add(new ErroValidacao
            {
                Campo = nameof(Descricao),
                Mensagem = "O campo \"Descrição\" deve conter entre 2 e 100 caracteres."
            });
        }

        if (Data == DateOnly.MinValue)
        {
            erros.Add(new ErroValidacao
            {
                Campo = nameof(Data),
                Mensagem = "O campo \"Data de Ocorrência\" deve conter uma data válida."
            });
        }

        if (Valor <= 0)
        {
            erros.Add(new ErroValidacao
            {
                Campo = nameof(Valor),
                Mensagem = "O campo \"Valor\" deve conter um valor maior que 0."
            });
        }

        if (Categorias.Count == 0)
        {
            erros.Add(new ErroValidacao
            {
                Campo = nameof(Categorias),
                Mensagem = "O campo \"Categorias\" deve conter pelo menos um valor."
            });
        }

        return erros;
    }

    public override void Atualizar(Despesa entidadeAtualizada)
    {
        Descricao = entidadeAtualizada.Descricao;
        Data = entidadeAtualizada.Data;
        Valor = entidadeAtualizada.Valor;
        FormaPagamento = entidadeAtualizada.FormaPagamento;
        Categorias = entidadeAtualizada.Categorias;
    }
}