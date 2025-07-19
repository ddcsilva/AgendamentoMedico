using AgendamentoMedico.Domain.Entities;

namespace AgendamentoMedico.Domain.Interfaces;

/// <summary>
/// Interface especializada para repositório de pacientes
/// </summary>
public interface IPacienteRepository : IRepository<Paciente>
{
    /// <summary>
    /// Busca um paciente pelo seu CPF
    /// </summary>
    /// <param name="cpf">Número do CPF</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>O paciente encontrado ou null se não existir</returns>
    Task<Paciente?> BuscarPorCpfAsync(string cpf, CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtém um paciente com seu histórico de consultas
    /// </summary>
    /// <param name="id">Identificador do paciente</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>O paciente com suas consultas ou null se não existir</returns>
    Task<Paciente?> ObterComHistoricoConsultasAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Busca pacientes por nome (busca parcial)
    /// </summary>
    /// <param name="nome">Nome ou parte do nome do paciente</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Lista de pacientes cujo nome contém o termo especificado</returns>
    Task<IEnumerable<Paciente>> BuscarPorNomeAsync(string nome, CancellationToken cancellationToken = default);

    /// <summary>
    /// Verifica se um CPF já está em uso
    /// </summary>
    /// <param name="cpf">Número do CPF</param>
    /// <param name="pacienteId">ID do paciente a ser excluído da verificação (para atualização)</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>True se o CPF já estiver em uso</returns>
    Task<bool> CpfJaExisteAsync(string cpf, Guid? pacienteId = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtém pacientes com consultas no período especificado
    /// </summary>
    /// <param name="dataInicio">Data de início do período</param>
    /// <param name="dataFim">Data de fim do período</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Lista de pacientes com consultas no período</returns>
    Task<IEnumerable<Paciente>> ObterComConsultasNoPeriodoAsync(DateTime dataInicio, DateTime dataFim, CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtém pacientes aniversariantes no mês especificado
    /// </summary>
    /// <param name="mes">Mês (1-12)</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Lista de pacientes aniversariantes no mês</returns>
    Task<IEnumerable<Paciente>> ObterAniversariantesDoMesAsync(int mes, CancellationToken cancellationToken = default);
}