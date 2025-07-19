using AgendamentoMedico.Application.Common;
using AgendamentoMedico.Application.Features.Auth.Dtos;

namespace AgendamentoMedico.Application.Features.Auth.Queries.ObterUsuario;

/// <summary>
/// Query para obter usuário por ID
/// </summary>
public record ObterUsuarioQuery(
    Guid UsuarioId
) : IRequest<UsuarioDto?>
{
    /// <summary>
    /// Valida os dados da query
    /// </summary>
    public void Validar()
    {
        if (UsuarioId == Guid.Empty)
            throw new ArgumentException("ID do usuário inválido", nameof(UsuarioId));
    }
}