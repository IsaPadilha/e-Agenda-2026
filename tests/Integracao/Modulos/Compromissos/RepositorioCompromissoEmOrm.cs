using eAgenda.Dominio.Modulos.Compromissos;
using eAgenda.Dominio.Modulos.Contatos;
using eAgenda.Infraestrutura.Modulos.Compromissos;

namespace eAgenda.Testes.Integracao.Modulos.Compromissos;

[TestClass]
public sealed class RepositorioCompromissoEmOrmTests : RepositorioEmOrmTestsBase
{
    [TestMethod]
    public void Cadastrar_DevePersistir_CompromissoNoBanco()
    {
        // Arranjo
        Guid compromissoId = Guid.CreateVersion7();

        Compromisso compromisso = new(
            "Reunião Online",
            new DateOnly(2026, 11, 06),
            new TimeOnly(11, 00),
            new TimeOnly(12, 00),
            TipoCompromisso.Remoto,
            "https://meet.google.com/meet/qdu-bbrw-fjd",
            null,
            null
        )
        {
            Id = compromissoId,
            UsuarioId = usuarioId
        };

        RepositorioCompromissoEmOrm repositorioCompromisso = new(dbContext);

        // Ação
        repositorioCompromisso.Cadastrar(compromisso);
        dbContext.ChangeTracker.Clear();

        Compromisso? compromissoSelecionado = repositorioCompromisso.SelecionarPorId(compromissoId);

        // Asserção
        Assert.IsNotNull(compromissoSelecionado);
        Assert.AreEqual("Reunião Online", compromissoSelecionado.Assunto);
        Assert.AreEqual(new DateOnly(2026, 11, 06), compromissoSelecionado.DataOcorrencia);
        Assert.AreEqual(new TimeOnly(11, 00), compromissoSelecionado.HoraInicio);
        Assert.AreEqual(new TimeOnly(12, 00), compromissoSelecionado.HoraTermino);
        Assert.AreEqual(TipoCompromisso.Remoto, compromissoSelecionado.TipoCompromisso);
        Assert.AreEqual("https://meet.google.com/meet/qdu-bbrw-fjd", compromissoSelecionado.LocalOuLink);
        Assert.IsNull(compromissoSelecionado.Contato);
    }

    [TestMethod]
    public void Cadastrar_DevePersistir_Compromisso_ComContatoNoBanco()
    {
        // Arranjo
        Guid contatoId = Guid.CreateVersion7();

        Contato contato = new(
            "Júnior Testes",
            "teste@academiadoprogramador.net",
            "(41) 98323-0093",
            "Testador",
            "Academia do Programador"
        )
        {
            Id = contatoId,
            UsuarioId = usuarioId
        };

        Guid compromissoId = Guid.CreateVersion7();

        Compromisso compromisso = new(
            "Reunião Presencial",
            new DateOnly(2026, 11, 06),
            new TimeOnly(11, 00),
            new TimeOnly(12, 00),
            TipoCompromisso.Presencial,
            "MidiLages",
            contatoId,
            contato
        )
        {
            Id = compromissoId,
            UsuarioId = usuarioId
        };

        RepositorioCompromissoEmOrm repositorioCompromisso = new(dbContext);

        // Ação
        repositorioCompromisso.Cadastrar(compromisso);
        dbContext.ChangeTracker.Clear();

        Compromisso? compromissoSelecionado = repositorioCompromisso.SelecionarPorId(compromissoId);

        // Asserção
        Assert.IsNotNull(compromissoSelecionado);
        Assert.AreEqual("Reunião Presencial", compromissoSelecionado.Assunto);
        Assert.AreEqual(new DateOnly(2026, 11, 06), compromissoSelecionado.DataOcorrencia);
        Assert.AreEqual(new TimeOnly(11, 00), compromissoSelecionado.HoraInicio);
        Assert.AreEqual(new TimeOnly(12, 00), compromissoSelecionado.HoraTermino);
        Assert.AreEqual(TipoCompromisso.Presencial, compromissoSelecionado.TipoCompromisso);
        Assert.AreEqual("MidiLages", compromissoSelecionado.LocalOuLink);
        Assert.IsNotNull(compromissoSelecionado.Contato);
        Assert.AreEqual(contato, compromissoSelecionado.Contato);
    }

    [TestMethod]
    public void Editar_DevePersistir_Compromisso_ComContatoNoBanco()
    {
        // Arranjo

        // Ação

        // Asserção
    }

    [TestMethod]
    public void Excluir_DevePersistir_CompromissoNoBanco()
    {
        // Arranjo

        // Ação

        // Asserção
    }
}