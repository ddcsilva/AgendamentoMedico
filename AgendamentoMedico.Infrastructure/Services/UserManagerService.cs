using AgendamentoMedico.Application.Interfaces;
using AgendamentoMedico.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace AgendamentoMedico.Infrastructure.Services;

/// <summary>
/// Implementação da interface IUserManager usando UserManager do Identity
/// </summary>
public class UserManagerService(UserManager<Usuario> userManager) : IUserManager
{
    private readonly UserManager<Usuario> _userManager = userManager;

    public async Task<Usuario?> FindByEmailAsync(string email)
    {
        return await _userManager.FindByEmailAsync(email);
    }

    public async Task<Usuario?> FindByIdAsync(string userId)
    {
        return await _userManager.FindByIdAsync(userId);
    }

    public async Task<bool> CreateAsync(Usuario user, string password)
    {
        var result = await _userManager.CreateAsync(user, password);
        return result.Succeeded;
    }

    public async Task<bool> UpdateAsync(Usuario user)
    {
        var result = await _userManager.UpdateAsync(user);
        return result.Succeeded;
    }

    public async Task<IList<Claim>> GetClaimsAsync(Usuario user)
    {
        return await _userManager.GetClaimsAsync(user);
    }

    public async Task<bool> AddClaimsAsync(Usuario user, IEnumerable<Claim> claims)
    {
        var result = await _userManager.AddClaimsAsync(user, claims);
        return result.Succeeded;
    }

    public async Task<bool> CheckPasswordAsync(Usuario user, string password)
    {
        return await _userManager.CheckPasswordAsync(user, password);
    }
}