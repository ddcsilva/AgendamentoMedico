using Microsoft.AspNetCore.Identity;

namespace AgendamentoMedico.Domain.Entities;

/// <summary>
/// Entidade que representa um usuário do sistema
/// Integra ASP.NET Core Identity com nossos padrões de auditoria
/// </summary>
public class Usuario : IdentityUser<Guid>
{
    private readonly List<PerfilUsuario> _perfis;

    public Usuario() : base()
    {
        Id = Guid.NewGuid();
        _perfis = [];
        CriadoEm = DateTime.UtcNow;
    }

    public Usuario(Guid id) : base()
    {
        Id = id;
        _perfis = [];
        CriadoEm = DateTime.UtcNow;
    }

    // Propriedades de auditoria
    public DateTime CriadoEm { get; protected set; }
    public DateTime? AtualizadoEm { get; protected set; }
    public string? CriadoPor { get; protected set; }
    public string? AtualizadoPor { get; protected set; }

    // Propriedades específicas
    public required string Nome { get; set; }
    public bool Ativo { get; protected set; } = true;
    public DateTime? UltimoLogin { get; protected set; }

    public IReadOnlyList<PerfilUsuario> Perfis => _perfis.AsReadOnly();

    public void MarcarComoAtualizada(string? atualizadoPor = null)
    {
        AtualizadoEm = DateTime.UtcNow;
        AtualizadoPor = atualizadoPor;
    }

    public void DefinirCriador(string criadoPor)
    {
        CriadoPor = criadoPor;
    }

    public void AdicionarPerfil(PerfilUsuario perfil)
    {
        ArgumentNullException.ThrowIfNull(perfil);

        if (perfil.UsuarioId != Id)
        {
            throw new InvalidOperationException("O perfil deve pertencer a este usuário");
        }

        if (_perfis.Any(p => p.TipoClaim == perfil.TipoClaim && p.ValorClaim == perfil.ValorClaim))
        {
            throw new InvalidOperationException($"Usuário já possui o claim '{perfil.TipoClaim}': '{perfil.ValorClaim}'");
        }

        _perfis.Add(perfil);
        MarcarComoAtualizada();
    }

    public void RemoverPerfil(string tipoClaim, string valorClaim)
    {
        var perfil = _perfis.FirstOrDefault(p => p.TipoClaim == tipoClaim && p.ValorClaim == valorClaim);
        if (perfil != null)
        {
            _perfis.Remove(perfil);
            MarcarComoAtualizada();
        }
    }

    public bool PossuiClaim(string tipoClaim, string? valorClaim = null)
    {
        if (string.IsNullOrEmpty(valorClaim))
            return _perfis.Any(p => p.TipoClaim == tipoClaim);

        return _perfis.Any(p => p.TipoClaim == tipoClaim && p.ValorClaim == valorClaim);
    }

    public void Ativar(string? atualizadoPor = null)
    {
        Ativo = true;
        MarcarComoAtualizada(atualizadoPor);
    }

    public void Desativar(string? atualizadoPor = null)
    {
        Ativo = false;
        MarcarComoAtualizada(atualizadoPor);
    }

    public void RegistrarLogin()
    {
        UltimoLogin = DateTime.UtcNow;
        MarcarComoAtualizada();
    }

    public void AtualizarInformacoes(string nome, string email, string? atualizadoPor = null)
    {
        Nome = nome ?? throw new ArgumentNullException(nameof(nome));
        Email = email ?? throw new ArgumentNullException(nameof(email));
        UserName = email;
        NormalizedEmail = email.ToUpperInvariant();
        NormalizedUserName = email.ToUpperInvariant();

        MarcarComoAtualizada(atualizadoPor);
    }

    public bool PodeFazerLogin()
    {
        return Ativo && EmailConfirmed;
    }
}