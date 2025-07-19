using AgendamentoMedico.Domain.Entities;
using System.Security.Claims;

namespace AgendamentoMedico.Application.Interfaces;

/// <summary>
/// Interface para abstrair o UserManager do Identity
/// </summary>
public interface IUserManager
{
    Task<Usuario?> FindByEmailAsync(string email);
    Task<Usuario?> FindByIdAsync(string userId);
    Task<bool> CreateAsync(Usuario user, string password);
    Task<bool> UpdateAsync(Usuario user);
    Task<IList<Claim>> GetClaimsAsync(Usuario user);
    Task<bool> AddClaimsAsync(Usuario user, IEnumerable<Claim> claims);
    Task<bool> CheckPasswordAsync(Usuario user, string password);
}