using AgendamentoMedico.Domain.Entities;

namespace AgendamentoMedico.Domain.Interfaces;

/// <summary>
/// Interface especializada para repositório de médicos
/// </summary>
public interface IMedicoRepository : IRepository<Medico>
{
    /// <summary>
    /// Busca um médico pelo seu CRM
    /// </summary>
    /// <param name="crm">Número do CRM</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>O médico encontrado ou null se não existir</returns>
    Task<Medico?> BuscarPorCrmAsync(string crm, CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtém um médico com suas consultas
    /// </summary>
    /// <param name="id">Identificador do médico</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>O médico com suas consultas ou null se não existir</returns>
    Task<Medico?> ObterComConsultasAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Busca médicos por especialidade
    /// </summary>
    /// <param name="especialidade">Especialidade médica</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Lista de médicos da especialidade especificada</returns>
    Task<IEnumerable<Medico>> BuscarPorEspecialidadeAsync(string especialidade, CancellationToken cancellationToken = default);

    /// <summary>
    /// Verifica se um CRM já está em uso
    /// </summary>
    /// <param name="crm">Número do CRM</param>
    /// <param name="medicoId">ID do médico a ser excluído da verificação (para atualização)</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>True se o CRM já estiver em uso</returns>
    Task<bool> CrmJaExisteAsync(string crm, int? medicoId = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtém médicos com consultas no período especificado
    /// </summary>
    /// <param name="dataInicio">Data de início do período</param>
    /// <param name="dataFim">Data de fim do período</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Lista de médicos com consultas no período</returns>
    Task<IEnumerable<Medico>> ObterComConsultasNoPeriodoAsync(DateTime dataInicio, DateTime dataFim, CancellationToken cancellationToken = default);
}