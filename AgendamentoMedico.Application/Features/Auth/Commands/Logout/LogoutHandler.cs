using AgendamentoMedico.Application.Common;
using AgendamentoMedico.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace AgendamentoMedico.Application.Features.Auth.Commands.Logout;

/// <summary>
/// Handler para logout de usuário
/// </summary>
public class LogoutHandler(
    UserManager<Usuario> userManager,
    SignInManager<Usuario> signInManager,
    ILogger<LogoutHandler> logger
) : IRequestHandler<LogoutCommand, bool>
{
    public async Task<bool> Handle(LogoutCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // Buscar usuário para logs
            var usuario = await userManager.FindByIdAsync(request.UsuarioId.ToString());
            if (usuario != null)
            {
                logger.LogInformation("Usuário {Email} fez logout", usuario.Email);
            }

            // SignOut (mais para consistência, JWT é stateless)
            await signInManager.SignOutAsync();

            return true;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erro durante logout do usuário {UsuarioId}", request.UsuarioId);
            return false;
        }
    }
}