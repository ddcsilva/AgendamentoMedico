using AgendamentoMedico.Domain.Entities;
using AgendamentoMedico.Domain.Interfaces;
using AgendamentoMedico.Infrastructure.Data;

namespace AgendamentoMedico.Infrastructure.Repositories;

/// <summary>
/// Implementação especializada do repositório de consultas
/// </summary>
/// <param name="context">Contexto do EF Core</param>
public class ConsultaRepository(ApplicationDbContext context) : Repository<Consulta>(context), IConsultaRepository
{

    /// <summary>
    /// Obtém consultas em um período específico
    /// </summary>
    public async Task<IEnumerable<Consulta>> ObterPorPeriodoAsync(DateTime dataInicio, DateTime dataFim, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(c => c.Medico)
            .Include(c => c.Paciente)
            .Where(c => c.DataHora >= dataInicio && c.DataHora <= dataFim)
            .OrderBy(c => c.DataHora)
            .ToListAsync(cancellationToken);
    }

    /// <summary>
    /// Busca consultas de um médico específico
    /// </summary>
    public async Task<IEnumerable<Consulta>> BuscarPorMedicoAsync(Guid medicoId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(c => c.Paciente)
            .Where(c => c.MedicoId == medicoId)
            .OrderBy(c => c.DataHora)
            .ToListAsync(cancellationToken);
    }

    /// <summary>
    /// Busca consultas de um paciente específico
    /// </summary>
    public async Task<IEnumerable<Consulta>> BuscarPorPacienteAsync(Guid pacienteId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(c => c.Medico)
            .Where(c => c.PacienteId == pacienteId)
            .OrderBy(c => c.DataHora)
            .ToListAsync(cancellationToken);
    }

    /// <summary>
    /// Obtém uma consulta com todos os dados relacionados (médico e paciente)
    /// </summary>
    public async Task<Consulta?> ObterComDadosCompletosAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(c => c.Medico)
            .Include(c => c.Paciente)
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
    }

    /// <summary>
    /// Verifica se existe conflito de horário para um médico
    /// </summary>
    public async Task<bool> ExisteConflitoHorarioMedicoAsync(Guid medicoId, DateTime dataHora, Guid? consultaId = null, CancellationToken cancellationToken = default)
    {
        var query = _dbSet.Where(c => c.MedicoId == medicoId && c.DataHora == dataHora);

        if (consultaId.HasValue)
        {
            query = query.Where(c => c.Id != consultaId.Value);
        }

        return await query.AnyAsync(cancellationToken);
    }

    /// <summary>
    /// Obtém consultas do dia para um médico específico
    /// </summary>
    public async Task<IEnumerable<Consulta>> ObterConsultasDoDiaAsync(Guid medicoId, DateTime data, CancellationToken cancellationToken = default)
    {
        var inicioData = data.Date;
        var fimData = inicioData.AddDays(1).AddTicks(-1);

        return await _dbSet
            .Include(c => c.Paciente)
            .Where(c => c.MedicoId == medicoId &&
                       c.DataHora >= inicioData &&
                       c.DataHora <= fimData)
            .OrderBy(c => c.DataHora)
            .ToListAsync(cancellationToken);
    }

    /// <summary>
    /// Obtém consultas próximas (nas próximas horas)
    /// </summary>
    public async Task<IEnumerable<Consulta>> ObterConsultasProximasAsync(int horasAFrente = 2, CancellationToken cancellationToken = default)
    {
        var agora = DateTime.Now;
        var dataLimite = agora.AddHours(horasAFrente);

        return await _dbSet
            .Include(c => c.Medico)
            .Include(c => c.Paciente)
            .Where(c => c.DataHora >= agora && c.DataHora <= dataLimite)
            .OrderBy(c => c.DataHora)
            .ToListAsync(cancellationToken);
    }
}