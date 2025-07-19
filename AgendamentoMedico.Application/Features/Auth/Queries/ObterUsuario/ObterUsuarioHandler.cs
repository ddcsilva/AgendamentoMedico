using AgendamentoMedico.Application.Common;
using AgendamentoMedico.Application.Features.Auth.Dtos;
using AgendamentoMedico.Application.Interfaces;

namespace AgendamentoMedico.Application.Features.Auth.Queries.ObterUsuario;

/// <summary>
/// Handler para obter usuário por ID
/// </summary>
public class ObterUsuarioHandler(
    IUserManager userManager
) : IRequestHandler<ObterUsuarioQuery, UsuarioDto?>
{
    public async Task<UsuarioDto?> Handle(ObterUsuarioQuery request, CancellationToken cancellationToken)
    {
        request.Validar();

        var usuario = await userManager.FindByIdAsync(request.UsuarioId.ToString());
        if (usuario == null)
            return null;

        var claims = await userManager.GetClaimsAsync(usuario);
        var claimValues = claims.Select(c => c.Value).ToList();

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