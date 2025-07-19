using AgendamentoMedico.Application.Common;
using AgendamentoMedico.Application.Interfaces;
using AgendamentoMedico.Domain.Entities;
using System.Security.Claims;

namespace AgendamentoMedico.Application.Features.Auth.Commands.RegistrarUsuario;

/// <summary>
/// Handler para registrar um novo usuário
/// </summary>
public class RegistrarUsuarioHandler(
    IUserManager userManager
) : IRequestHandler<RegistrarUsuarioCommand, Usuario>
{
    public async Task<Usuario> Handle(RegistrarUsuarioCommand request, CancellationToken cancellationToken)
    {
        request.Validar();

        var usuarioExistente = await userManager.FindByEmailAsync(request.Email);
        if (usuarioExistente != null)
            throw new InvalidOperationException($"Email {request.Email} já está em uso");

        var usuario = new Usuario
        {
            Nome = request.Nome,
            Email = request.Email,
            UserName = request.Email,
            EmailConfirmed = true
        };

        usuario.DefinirCriador("System");

        var sucesso = await userManager.CreateAsync(usuario, request.Senha);
        if (!sucesso)
        {
            throw new InvalidOperationException("Erro ao criar usuário");
        }

        if (request.Claims?.Any() == true)
        {
            var claims = request.Claims.Select(c => new Claim("permission", c)).ToList();
            await userManager.AddClaimsAsync(usuario, claims);
        }

        return usuario;
    }
}