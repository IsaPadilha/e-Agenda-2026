using System.Security.Claims;
using eAgenda.Dominio.Compartilhado;

public sealed class ProvedorUsuarioAtualHttp(IHttpContextAccessor httpContextAccessor)
    : IProvedorUsuarioAtual
{
    public Guid Id
    {
        get
        {
            // No sistema de Claims, NameIdentifier geralmente se refere ao ID
            string? contextId = httpContextAccessor.HttpContext?
                .User
                .FindFirstValue(ClaimTypes.NameIdentifier);

            bool conseguiuConverter = Guid.TryParse(contextId, out Guid usuarioId);

            return conseguiuConverter ? usuarioId : Guid.Empty;
        }
    }
}