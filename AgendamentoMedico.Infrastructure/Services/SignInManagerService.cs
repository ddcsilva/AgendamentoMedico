using AgendamentoMedico.Application.Interfaces;
using AgendamentoMedico.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace AgendamentoMedico.Infrastructure.Services;

/// <summary>
/// Implementação da interface ISignInManager usando SignInManager do Identity
/// </summary>
public class SignInManagerService(SignInManager<Usuario> signInManager) : ISignInManager
{
    private readonly SignInManager<Usuario> _signInManager = signInManager;

    public async Task<bool> CheckPasswordSignInAsync(Usuario user, string password, bool lockoutOnFailure)
    {
        var result = await _signInManager.CheckPasswordSignInAsync(user, password, lockoutOnFailure);
        return result.Succeeded;
    }

    public async Task SignOutAsync()
    {
        await _signInManager.SignOutAsync();
    }
}