using AgendamentoMedico.Application.Common;
using AgendamentoMedico.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace AgendamentoMedico.Application.Features.Auth.Commands.RegistrarUsuario;

/// <summary>
/// Handler para registrar um novo usuário
/// </summary>
public class RegistrarUsuarioHandler(
    UserManager<Usuario> userManager
) : IRequestHandler<RegistrarUsuarioCommand, Usuario>
{
    public async Task<Usuario> Handle(RegistrarUsuarioCommand request, CancellationToken cancellationToken)
    {
        // Validar dados
        request.Validar();

        // Verificar se email já existe
        var usuarioExistente = await userManager.FindByEmailAsync(request.Email);
        if (usuarioExistente != null)
            throw new InvalidOperationException($"Email {request.Email} já está em uso");

        // Criar usuário
        var usuario = new Usuario
        {
            Nome = request.Nome,
            Email = request.Email,
            UserName = request.Email,
            EmailConfirmed = true // Admin registra usuários já confirmados
        };

        usuario.DefinirCriador("System"); // Ou pegar do contexto atual

        // Criar no banco
        var resultado = await userManager.CreateAsync(usuario, request.Senha);
        if (!resultado.Succeeded)
        {
            var erros = string.Join(", ", resultado.Errors.Select(e => e.Description));
            throw new InvalidOperationException($"Erro ao criar usuário: {erros}");
        }

        // Adicionar claims se fornecidos
        if (request.Claims?.Any() == true)
        {
            var claims = request.Claims.Select(c => new Claim("permission", c)).ToList();
            await userManager.AddClaimsAsync(usuario, claims);
        }

        return usuario;
    }
}