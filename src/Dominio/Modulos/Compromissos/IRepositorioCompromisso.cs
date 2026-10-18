using System;
using eAgenda.Dominio.Compartilhado;

namespace eAgenda.Dominio.Modulos.Compromissos;

public interface IRepositorioCompromisso : IRepositorio<Compromisso>
{
    bool ExisteConflitoDeHorario(DateOnly data, TimeOnly horaInicio, TimeOnly horaTermino, Guid? idIgnorado = null);
}