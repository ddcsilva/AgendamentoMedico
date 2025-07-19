namespace AgendamentoMedico.Domain.Entities;

/// <summary>
/// Entidade que representa um perfil/claim customizado de usuário
/// Permite granularidade no controle de acesso além dos roles padrão
/// </summary>
public class PerfilUsuario : AuditableEntity
{
    public PerfilUsuario()
    {
    }

    public PerfilUsuario(Guid usuarioId, string tipoClaim, string valorClaim)
    {
        UsuarioId = usuarioId;
        TipoClaim = tipoClaim ?? throw new ArgumentNullException(nameof(tipoClaim));
        ValorClaim = valorClaim ?? throw new ArgumentNullException(nameof(valorClaim));
    }

    public required Guid UsuarioId { get; set; }
    public required string TipoClaim { get; set; }
    public required string ValorClaim { get; set; }
    public string? Descricao { get; set; }

    // Propriedades de navegação
    public Usuario Usuario { get; private set; } = null!;

    public void AtualizarInformacoes(string novoValor, string? novaDescricao = null, string? atualizadoPor = null)
    {
        ValorClaim = novoValor ?? throw new ArgumentNullException(nameof(novoValor));
        Descricao = novaDescricao;

        MarcarComoAtualizada(atualizadoPor);
    }

    public bool Corresponde(string tipo, string valor)
    {
        return TipoClaim.Equals(tipo, StringComparison.OrdinalIgnoreCase) &&
               ValorClaim.Equals(valor, StringComparison.OrdinalIgnoreCase);
    }

    public bool EhDoTipo(string tipo)
    {
        return TipoClaim.Equals(tipo, StringComparison.OrdinalIgnoreCase);
    }
}