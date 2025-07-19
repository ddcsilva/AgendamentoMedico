using AgendamentoMedico.Domain.Entities;

namespace AgendamentoMedico.Application.Interfaces;

/// <summary>
/// Interface para abstrair o SignInManager do Identity
/// </summary>
public interface ISignInManager
{
    Task<bool> CheckPasswordSignInAsync(Usuario user, string password, bool lockoutOnFailure);
    Task SignOutAsync();
}