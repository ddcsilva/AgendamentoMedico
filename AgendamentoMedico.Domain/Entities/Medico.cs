namespace AgendamentoMedico.Domain.Entities;

/// <summary>
/// Entidade que representa um médico no sistema
/// </summary>
public class Medico : AuditableEntity
{
    private readonly List<Consulta> _consultas;

    public Medico()
    {
        _consultas = [];
    }

    public required string Nome { get; set; }
    public required string CRM { get; set; }
    public required string Especialidade { get; set; }

    public IReadOnlyList<Consulta> Consultas => _consultas.AsReadOnly();

    public void AdicionarConsulta(Consulta consulta)
    {
        ArgumentNullException.ThrowIfNull(consulta);

        if (consulta.MedicoId != Id)
            throw new InvalidOperationException("A consulta deve pertencer a este médico");

        _consultas.Add(consulta);
    }

    public void AtualizarInformacoes(string nome, string especialidade, string? atualizadoPor = null)
    {
        Nome = nome ?? throw new ArgumentNullException(nameof(nome));
        Especialidade = especialidade ?? throw new ArgumentNullException(nameof(especialidade));

        MarcarComoAtualizada(atualizadoPor);
    }
}