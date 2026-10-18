using eAgenda.Dominio.Modulos.Compromissos;
using eAgenda.Infraestrutura.Compartilhado.Orm;
using Microsoft.EntityFrameworkCore;

namespace eAgenda.Infraestrutura.Modulos.Compromissos;

public sealed class RepositorioCompromissoEmOrm(EAgendaDbContext dbContext) :
    RepositorioBaseEmOrm<Compromisso>(dbContext), IRepositorioCompromisso
{
    public override Compromisso? SelecionarPorId(Guid id)
    {
        return registros
            .Include(c => c.Contato)
            .FirstOrDefault(c => c.Id == id);
    }

    public override List<Compromisso> SelecionarTodos()
    {
        return registros
            .Include(c => c.Contato)
            .OrderBy(c => c.DataOcorrencia)
            .ThenBy(c => c.HoraInicio)
            .ToList();
    }

    public bool ExisteConflitoDeHorario(DateOnly data, TimeOnly horaInicio, TimeOnly horaTermino, Guid? idIgnorado = null)
    {
        // Há conflito se for no mesmo dia e os horários se sobrepõem:
        // (InicioA < TerminoB) e (TerminoA > InicioB)
        return registros.Any(c =>
            c.Id != idIgnorado &&
            c.DataOcorrencia == data &&
            c.HoraInicio < horaTermino &&
            c.HoraTermino > horaInicio
        );
    }
}