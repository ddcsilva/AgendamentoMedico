using AgendamentoMedico.Application.Common;
using AgendamentoMedico.Application.Features.Auth.Dtos;
using AgendamentoMedico.Application.Interfaces;
using AgendamentoMedico.Domain.Entities;

namespace AgendamentoMedico.Application.Features.Auth.Commands.Login;

/// <summary>
/// Handler para login de usuário
/// </summary>
public class LoginHandler(
    IUserManager userManager,
    ISignInManager signInManager,
    IJwtTokenService jwtTokenService
) : IRequestHandler<LoginCommand, LoginResult>
{
    public async Task<LoginResult> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        request.Validar();

        var usuario = await userManager.FindByEmailAsync(request.Email)
            ?? throw new UnauthorizedAccessException("Credenciais inválidas");

        if (!usuario.PodeFazerLogin())
        {
            throw new UnauthorizedAccessException("Usuário inativo ou email não confirmado");
        }

        var senhaCorreta = await signInManager.CheckPasswordSignInAsync(usuario, request.Senha, false);
        if (!senhaCorreta)
        {
            throw new UnauthorizedAccessException("Credenciais inválidas");
        }

        usuario.RegistrarLogin();
        await userManager.UpdateAsync(usuario);

        var claims = await userManager.GetClaimsAsync(usuario);
        var claimValues = claims.Select(c => c.Value).ToList();

        var token = jwtTokenService.GerarToken(usuario, claims);
        var expiresAt = DateTime.UtcNow.AddHours(8);

        return new LoginResult(
            usuario.Id,
            usuario.Nome,
            usuario.Email!,
            token,
            expiresAt,
            claimValues
        );
    }
}