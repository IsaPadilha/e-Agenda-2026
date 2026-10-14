using eAgenda.Aplicacao.Modulos.Categorias;
using eAgenda.Aplicacao.Modulos.Despesas;
using Microsoft.Extensions.DependencyInjection;
namespace eAgenda.Aplicacao.Compartilhado;

public static class InjecaoDependencia
{
    public static void AdicionarCamadaAplicacao(this IServiceCollection services)
    {
        services.AddScoped<ServicoCategoria>();
        services.AddScoped<ServicoDespesa>();
    }
}