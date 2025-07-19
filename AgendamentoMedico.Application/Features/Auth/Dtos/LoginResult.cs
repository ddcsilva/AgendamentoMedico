namespace AgendamentoMedico.Application.Features.Auth.Dtos;

/// <summary>
/// Resultado do login
/// </summary>
public record LoginResult(
    Guid UsuarioId,
    string Nome,
    string Email,
    string Token,
    DateTime ExpiresAt,
    IEnumerable<string> Claims
);