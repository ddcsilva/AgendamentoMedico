namespace AgendamentoMedico.Domain.Entities;

/// <summary>
/// Classe base para todas as entidades de domínio
/// </summary>
public abstract class Entity
{
    /// <summary>
    /// Identificador único da entidade
    /// </summary>
    public Guid Id { get; protected set; }

    /// <summary>
    /// Construtor protegido para gerar novo ID
    /// </summary>
    protected Entity()
    {
        Id = Guid.NewGuid();
    }

    /// <summary>
    /// Construtor protegido para definir ID específico (usado pelo EF ou testes)
    /// </summary>
    protected Entity(Guid id)
    {
        Id = id;
    }

    /// <summary>
    /// Implementação de igualdade baseada no Id
    /// </summary>
    public override bool Equals(object? obj)
    {
        if (obj is not Entity other)
            return false;

        if (ReferenceEquals(this, other))
            return true;

        if (GetType() != other.GetType())
            return false;

        return Id == other.Id;
    }

    /// <summary>
    /// Hash code baseado no Id
    /// </summary>
    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }

    /// <summary>
    /// Operador de igualdade
    /// </summary>
    public static bool operator ==(Entity? left, Entity? right)
    {
        return left?.Equals(right) ?? right is null;
    }

    /// <summary>
    /// Operador de desigualdade
    /// </summary>
    public static bool operator !=(Entity? left, Entity? right)
    {
        return !(left == right);
    }
}