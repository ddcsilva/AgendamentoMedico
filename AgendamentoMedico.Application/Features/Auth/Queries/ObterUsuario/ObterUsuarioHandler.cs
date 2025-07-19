using AgendamentoMedico.Application.Common;
using AgendamentoMedico.Application.Features.Auth.Dtos;
using AgendamentoMedico.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace AgendamentoMedico.Application.Features.Auth.Queries.ObterUsuario;

/// <summary>
/// Handler para obter usuário por ID
/// </summary>
public class ObterUsuarioHandler(
    UserManager<Usuario> userManager
) : IRequestHandler<ObterUsuarioQuery, UsuarioDto?>
{
    public async Task<UsuarioDto?> Handle(ObterUsuarioQuery request, CancellationToken cancellationToken)
    {
        // Validar dados
        request.Validar();

        // Buscar usuário
        var usuario = await userManager.FindByIdAsync(request.UsuarioId.ToString());
        if (usuario == null)
            return null;

        // Buscar claims do usuário
        var claims = await userManager.GetClaimsAsync(usuario);
        var claimValues = claims.Select(c => c.Value).ToList();

        // Mapear para DTO
        return new UsuarioDto(
            usuario.Id,
            usuario.Nome,
            usuario.Email!,
            usuario.Ativo,
            usuario.CriadoEm,
            usuario.UltimoLogin,
            claimValues
        );
    }
}