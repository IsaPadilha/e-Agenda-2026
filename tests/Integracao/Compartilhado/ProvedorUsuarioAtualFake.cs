using eAgenda.Dominio.Compartilhado;

namespace eAgenda.Testes.Integracao.Compartilhado;

public sealed class ProvedorUsuarioAtualFake(Guid id) : IProvedorUsuarioAtual
{
    public Guid Id => id;
}