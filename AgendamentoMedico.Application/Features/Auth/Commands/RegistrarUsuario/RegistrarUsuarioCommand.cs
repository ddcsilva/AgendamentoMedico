using AgendamentoMedico.Application.Common;
using AgendamentoMedico.Domain.Entities;

namespace AgendamentoMedico.Application.Features.Auth.Commands.RegistrarUsuario;

/// <summary>
/// Command para registrar um novo usuário no sistema
/// </summary>
public record RegistrarUsuarioCommand(
    string Nome,
    string Email,
    string Senha,
    IEnumerable<string> Claims
) : IRequest<Usuario>
{
    /// <summary>
    /// Valida os dados do command
    /// </summary>
    public void Validar()
    {
        if (string.IsNullOrWhiteSpace(Nome))
            throw new ArgumentException("Nome é obrigatório", nameof(Nome));

        if (string.IsNullOrWhiteSpace(Email))
            throw new ArgumentException("Email é obrigatório", nameof(Email));

        if (!Email.Contains('@'))
            throw new ArgumentException("Email inválido", nameof(Email));

        if (string.IsNullOrWhiteSpace(Senha))
            throw new ArgumentException("Senha é obrigatória", nameof(Senha));

        if (Senha.Length < 6)
            throw new ArgumentException("Senha deve ter pelo menos 6 caracteres", nameof(Senha));
    }
}