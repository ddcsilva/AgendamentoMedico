using AgendamentoMedico.Application.Common;
using AgendamentoMedico.Application.Interfaces;
using Microsoft.Extensions.Logging;

namespace AgendamentoMedico.Application.Features.Auth.Commands.Logout;

/// <summary>
/// Handler para logout de usuário
/// </summary>
public class LogoutHandler(
    IUserManager userManager,
    ISignInManager signInManager,
    ILogger<LogoutHandler> logger
) : IRequestHandler<LogoutCommand, bool>
{
    public async Task<bool> Handle(LogoutCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var usuario = await userManager.FindByIdAsync(request.UsuarioId.ToString());
            if (usuario != null)
            {
                logger.LogInformation("Usuário {Email} fez logout", usuario.Email);
            }

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