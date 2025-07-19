using AgendamentoMedico.Application.Common;
using AgendamentoMedico.Application.Features.Auth.Dtos;

namespace AgendamentoMedico.Application.Features.Auth.Commands.Login;

/// <summary>
/// Command para login de usuário
/// </summary>
public record LoginCommand(
    string Email,
    string Senha
) : IRequest<LoginResult>
{
    /// <summary>
    /// Valida os dados do command
    /// </summary>
    public void Validar()
    {
        if (string.IsNullOrWhiteSpace(Email))
            throw new ArgumentException("Email é obrigatório", nameof(Email));

        if (string.IsNullOrWhiteSpace(Senha))
            throw new ArgumentException("Senha é obrigatória", nameof(Senha));
    }
}