namespace AgendamentoMedico.Domain.Entities;

/// <summary>
/// Classe base para entidades que requerem auditoria
/// </summary>
public abstract class AuditableEntity : Entity
{
    public DateTime CriadoEm { get; protected set; }
    public DateTime? AtualizadoEm { get; protected set; }
    public string? CriadoPor { get; protected set; }
    public string? AtualizadoPor { get; protected set; }

    protected AuditableEntity() : base()
    {
        CriadoEm = DateTime.UtcNow;
    }

    protected AuditableEntity(Guid id) : base(id)
    {
        CriadoEm = DateTime.UtcNow;
    }

    public void MarcarComoAtualizada(string? atualizadoPor = null)
    {
        AtualizadoEm = DateTime.UtcNow;
        AtualizadoPor = atualizadoPor;
    }

    public void DefinirCriador(string criadoPor)
    {
        CriadoPor = criadoPor;
    }
}