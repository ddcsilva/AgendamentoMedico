using AgendamentoMedico.Domain.Entities;

namespace AgendamentoMedico.Domain.Interfaces;

/// <summary>
/// Interface especializada para repositório de consultas
/// </summary>
public interface IConsultaRepository : IRepository<Consulta>
{
    /// <summary>
    /// Obtém consultas em um período específico
    /// </summary>
    /// <param name="dataInicio">Data de início do período</param>
    /// <param name="dataFim">Data de fim do período</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Lista de consultas no período especificado</returns>
    Task<IEnumerable<Consulta>> ObterPorPeriodoAsync(DateTime dataInicio, DateTime dataFim, CancellationToken cancellationToken = default);

    /// <summary>
    /// Busca consultas de um médico específico
    /// </summary>
    /// <param name="medicoId">Identificador do médico</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Lista de consultas do médico</returns>
    Task<IEnumerable<Consulta>> BuscarPorMedicoAsync(int medicoId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Busca consultas de um paciente específico
    /// </summary>
    /// <param name="pacienteId">Identificador do paciente</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Lista de consultas do paciente</returns>
    Task<IEnumerable<Consulta>> BuscarPorPacienteAsync(int pacienteId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtém uma consulta com todos os dados relacionados (médico e paciente)
    /// </summary>
    /// <param name="id">Identificador da consulta</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>A consulta com dados relacionados ou null se não existir</returns>
    Task<Consulta?> ObterComDadosCompletosAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Verifica se existe conflito de horário para um médico
    /// </summary>
    /// <param name="medicoId">Identificador do médico</param>
    /// <param name="dataHora">Data e hora da consulta</param>
    /// <param name="consultaId">ID da consulta a ser excluída da verificação (para reagendamento)</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>True se existir conflito de horário</returns>
    Task<bool> ExisteConflitoHorarioMedicoAsync(int medicoId, DateTime dataHora, int? consultaId = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtém consultas do dia para um médico específico
    /// </summary>
    /// <param name="medicoId">Identificador do médico</param>
    /// <param name="data">Data das consultas</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Lista de consultas do dia para o médico</returns>
    Task<IEnumerable<Consulta>> ObterConsultasDoDiaAsync(int medicoId, DateTime data, CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtém consultas próximas (nas próximas horas)
    /// </summary>
    /// <param name="horasAFrente">Número de horas à frente para buscar</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Lista de consultas próximas</returns>
    Task<IEnumerable<Consulta>> ObterConsultasProximasAsync(int horasAFrente = 2, CancellationToken cancellationToken = default);
}