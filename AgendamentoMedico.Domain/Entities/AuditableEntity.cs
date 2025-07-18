namespace AgendamentoMedico.Domain.Entities;

/// <summary>
/// Classe base para entidades que requerem auditoria
/// </summary>
public abstract class AuditableEntity : Entity
{
    /// <summary>
    /// Data e hora de criação da entidade
    /// </summary>
    public DateTime CriadoEm { get; protected set; }

    /// <summary>
    /// Data e hora da última atualização
    /// </summary>
    public DateTime? AtualizadoEm { get; protected set; }

    /// <summary>
    /// Usuário que criou a entidade
    /// </summary>
    public string? CriadoPor { get; protected set; }

    /// <summary>
    /// Usuário que fez a última atualização
    /// </summary>
    public string? AtualizadoPor { get; protected set; }

    /// <summary>
    /// Construtor protegido para gerar novo ID e definir data de criação
    /// </summary>
    protected AuditableEntity() : base()
    {
        CriadoEm = DateTime.UtcNow;
    }

    /// <summary>
    /// Construtor protegido para definir ID específico e data de criação
    /// </summary>
    protected AuditableEntity(Guid id) : base(id)
    {
        CriadoEm = DateTime.UtcNow;
    }

    /// <summary>
    /// Marca a entidade como atualizada
    /// </summary>
    /// <param name="atualizadoPor">Usuário que fez a atualização</param>
    public void MarcarComoAtualizada(string? atualizadoPor = null)
    {
        AtualizadoEm = DateTime.UtcNow;
        AtualizadoPor = atualizadoPor;
    }

    /// <summary>
    /// Define o usuário que criou a entidade
    /// </summary>
    /// <param name="criadoPor">Usuário criador</param>
    public void DefinirCriador(string criadoPor)
    {
        CriadoPor = criadoPor;
    }
}