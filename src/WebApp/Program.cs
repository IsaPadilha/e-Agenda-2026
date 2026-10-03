using eAgenda.Aplicacao.Compartilhado;

var builder = WebApplication.CreateBuilder(args);

// Configura Camada de Infraestrutura

// Configura Camada de Aplicação
builder.Services.AdicionarCamadaAplicacao();

// Configura Camada de Apresentação
builder.Services.AddControllersWithViews().AddRazorOptions(options =>
{
    // Reseta a configuração padrão do MVC
    options.ViewLocationFormats.Clear();

    // Localização das Views dos módulos: Modulos/ModuloAluno/Apresentacao/Views/Listar.cshtml
    options.ViewLocationFormats.Add("/Modulos/Modulo{1}/Views/{0}.cshtml");

    // Localização das Views compartilhadas: /Compartilhado/Apresentacao/Views/_Layout.cshtml
    options.ViewLocationFormats.Add("/Compartilhado/Views/{0}.cshtml");
});

var app = builder.Build();

app.UseRouting();
app.MapDefaultControllerRoute();

app.Run();