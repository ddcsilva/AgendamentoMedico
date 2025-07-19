using AgendamentoMedico.Application.Common;

namespace AgendamentoMedico.Application.Features.Auth.Commands.Logout;

/// <summary>
/// Command para logout de usuário
/// </summary>
public record LogoutCommand(
    Guid UsuarioId
) : IRequest<bool>;