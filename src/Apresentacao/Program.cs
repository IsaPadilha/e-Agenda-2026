using eAgenda.Aplicacao.Compartilhado;
using eAgenda.Infraestrutura.Compartilhado;
using eAgenda.Infraestrutura.Compartilhado.Orm;
using eAgenda.Infraestrutura.Modulos.Contatos;
using eAgenda.Aplicacao.Modulos.Contatos;
using eAgenda.Dominio.Modulos.Contatos;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IRepositorioContato, RepositorioContatoEmOrm>();
builder.Services.AddScoped<ServicoContato>();
// Configura Camada de Infraestrutura
builder.Services.AdicionarCamadaInfraestrutura(builder.Configuration);
// Configura Camada de Aplicação
builder.Services.AdicionarCamadaAplicacao();

// Configura Camada de Apresentação
builder.Services.AddControllersWithViews().AddRazorOptions(options =>
{
    // Reseta a configuração padrão do MVC
    options.ViewLocationFormats.Clear();

    // Procura na estrutura de pastas: Modulos/[NomeDoModulo]/Views/[Acao].cshtml
    options.ViewLocationFormats.Add("/Modulos/{1}/Views/{0}.cshtml");
    options.ViewLocationFormats.Add("/Apresentacao/Modulos/{1}/Views/{0}.cshtml");

    // Localização das Views compartilhadas e Layouts
    options.ViewLocationFormats.Add("/Views/Shared/{0}.cshtml");
    options.ViewLocationFormats.Add("/Views/{1}/{0}.cshtml");
    options.ViewLocationFormats.Add("/Compartilhado/Views/{0}.cshtml");
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();

    var dbContext = scope.ServiceProvider.GetRequiredService<EAgendaDbContext>();

    if (dbContext.Database.IsSqlServer())
        dbContext.Database.Migrate();
}


app.UseRouting();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();