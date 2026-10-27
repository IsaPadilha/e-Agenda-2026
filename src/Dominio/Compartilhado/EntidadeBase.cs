namespace eAgenda.Dominio.Compartilhado;

public sealed class ErroValidacao
{
    public string Campo { get; set; }
    public string Mensagem { get; set; }

    public ErroValidacao() { }

    public ErroValidacao(string campo, string mensagem)
    {
        Campo = campo;
        Mensagem = mensagem;
    }
}
public abstract class EntidadeBase<T>
{
    public Guid Id { get; set; } = Guid.CreateVersion7();
    public abstract List<ErroValidacao> Validar();
    public abstract void Atualizar(T entidadeAtualizada);
}