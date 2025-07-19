namespace AgendamentoMedico.Application.Features.Auth.Dtos;

/// <summary>
/// DTO para dados do usuário
/// </summary>
public record UsuarioDto(
    Guid Id,
    string Nome,
    string Email,
    bool Ativo,
    DateTime CriadoEm,
    DateTime? UltimoLogin,
    IEnumerable<string> Claims
);