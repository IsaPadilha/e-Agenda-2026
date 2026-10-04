using eAgenda.Dominio.Modulos.Categorias;
using eAgenda.Infraestrutura.Compartilhado.Orm;
using eAgenda.Infraestrutura.Modulos.Categorias;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace eAgenda.Infraestrutura.Compartilhado;

public static class InjecaoDependencia
{
    public static void AdicionarCamadaInfraestrutura(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<EAgendaDbContext>(options =>
        {
            string? connectionString = configuration.GetConnectionString("SqlServer");

            if (string.IsNullOrWhiteSpace(connectionString))
                throw new InvalidOperationException("A ConnectionStrings:SqlServer não foi configurada.");

            options.UseSqlServer(connectionString, cfg => cfg.EnableRetryOnFailure(3));
        });

        services.AddScoped<IRepositorioCategoria, RepositorioCategoriaEmOrm>();
    }
}