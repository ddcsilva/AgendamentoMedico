namespace AgendamentoMedico.Domain.Entities;

/// <summary>
/// Entidade que representa uma consulta médica no sistema
/// </summary>
public class Consulta : AuditableEntity
{
    public Consulta() { }

    public required Guid MedicoId { get; set; }
    public required Guid PacienteId { get; set; }
    public required DateTime DataHora { get; set; }
    public string? Observacoes { get; set; }

    // Propriedades de navegação
    public Medico Medico { get; private set; } = null!;
    public Paciente Paciente { get; private set; } = null!;

    public void Reagendar(DateTime novaDataHora, string? atualizadoPor = null)
    {
        if (novaDataHora <= DateTime.Now)
        {
            throw new InvalidOperationException("A data da consulta deve ser no futuro");
        }

        DataHora = novaDataHora;

        MarcarComoAtualizada(atualizadoPor);
    }

    public void AtualizarObservacoes(string? observacoes, string? atualizadoPor = null)
    {
        Observacoes = observacoes;

        MarcarComoAtualizada(atualizadoPor);
    }

    public bool JaOcorreu()
    {
        return DataHora < DateTime.Now;
    }

    public bool EstaProxima()
    {
        var agora = DateTime.Now;
        var diferenca = DataHora - agora;
        return diferenca.TotalHours <= 24 && diferenca.TotalHours > 0;
    }
}