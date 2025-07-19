using AgendamentoMedico.Application.Common;
using AgendamentoMedico.Application.Features.Auth.Dtos;
using AgendamentoMedico.Application.Interfaces;
using AgendamentoMedico.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace AgendamentoMedico.Application.Features.Auth.Commands.Login;

/// <summary>
/// Handler para login de usuário
/// </summary>
public class LoginHandler(
    UserManager<Usuario> userManager,
    SignInManager<Usuario> signInManager,
    IJwtTokenService jwtTokenService
) : IRequestHandler<LoginCommand, LoginResult>
{
    public async Task<LoginResult> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        // Validar dados
        request.Validar();

        // Buscar usuário
        var usuario = await userManager.FindByEmailAsync(request.Email);
        if (usuario == null)
            throw new UnauthorizedAccessException("Credenciais inválidas");

        // Verificar se usuário está ativo
        if (!usuario.PodeFazerLogin())
            throw new UnauthorizedAccessException("Usuário inativo ou email não confirmado");

        // Verificar senha
        var resultado = await signInManager.CheckPasswordSignInAsync(usuario, request.Senha, false);
        if (!resultado.Succeeded)
            throw new UnauthorizedAccessException("Credenciais inválidas");

        // Registrar login
        usuario.RegistrarLogin();
        await userManager.UpdateAsync(usuario);

        // Buscar claims do usuário
        var claims = await userManager.GetClaimsAsync(usuario);
        var claimValues = claims.Select(c => c.Value).ToList();

        // Gerar token JWT
        var token = jwtTokenService.GerarToken(usuario, claims);
        var expiresAt = DateTime.UtcNow.AddHours(8); // 8 horas de validade

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