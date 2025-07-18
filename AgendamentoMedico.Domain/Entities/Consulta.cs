namespace AgendamentoMedico.Domain.Entities;

/// <summary>
/// Entidade que representa uma consulta médica no sistema
/// </summary>
public class Consulta : AuditableEntity
{
    /// <summary>
    /// Construtor para criar uma nova consulta
    /// </summary>
    /// <param name="medicoId">ID do médico</param>
    /// <param name="pacienteId">ID do paciente</param>
    /// <param name="dataHora">Data e hora da consulta</param>
    /// <param name="observacoes">Observações da consulta (opcional)</param>
    public Consulta(Guid medicoId, Guid pacienteId, DateTime dataHora, string? observacoes = null) : base()
    {
        MedicoId = medicoId;
        PacienteId = pacienteId;
        DataHora = dataHora;
        Observacoes = observacoes;
    }

    /// <summary>
    /// Construtor privado para uso do Entity Framework
    /// </summary>
    private Consulta() : base() { }

    public Guid MedicoId { get; private set; }
    public Guid PacienteId { get; private set; }
    public DateTime DataHora { get; private set; }
    public string? Observacoes { get; private set; }

    /// <summary>
    /// Navigation property para o médico (privada para EF)
    /// </summary>
    public Medico Medico { get; private set; } = null!;

    /// <summary>
    /// Navigation property para o paciente (privada para EF)
    /// </summary>
    public Paciente Paciente { get; private set; } = null!;

    /// <summary>
    /// Reagenda a consulta para uma nova data/hora
    /// </summary>
    /// <param name="novaDataHora">Nova data e hora</param>
    /// <param name="atualizadoPor">Usuário que fez a atualização</param>
    public void Reagendar(DateTime novaDataHora, string? atualizadoPor = null)
    {
        if (novaDataHora <= DateTime.Now)
        {
            throw new InvalidOperationException("A data da consulta deve ser no futuro");
        }

        DataHora = novaDataHora;

        // Marca como atualizada automaticamente
        MarcarComoAtualizada(atualizadoPor);
    }

    /// <summary>
    /// Atualiza as observações da consulta
    /// </summary>
    /// <param name="observacoes">Novas observações</param>
    /// <param name="atualizadoPor">Usuário que fez a atualização</param>
    public void AtualizarObservacoes(string? observacoes, string? atualizadoPor = null)
    {
        Observacoes = observacoes;

        // Marca como atualizada automaticamente
        MarcarComoAtualizada(atualizadoPor);
    }

    /// <summary>
    /// Verifica se a consulta já ocorreu
    /// </summary>
    /// <returns>True se a consulta já passou</returns>
    public bool JaOcorreu()
    {
        return DataHora < DateTime.Now;
    }

    /// <summary>
    /// Verifica se a consulta está próxima (dentro de 24 horas)
    /// </summary>
    /// <returns>True se a consulta está próxima</returns>
    public bool EstaProxima()
    {
        var agora = DateTime.Now;
        var diferenca = DataHora - agora;
        return diferenca.TotalHours <= 24 && diferenca.TotalHours > 0;
    }
}