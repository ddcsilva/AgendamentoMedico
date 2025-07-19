using Microsoft.EntityFrameworkCore;
using AgendamentoMedico.Domain.Entities;
using AgendamentoMedico.Domain.Interfaces;
using AgendamentoMedico.Infrastructure.Data;

namespace AgendamentoMedico.Infrastructure.Repositories;

/// <summary>
/// Implementação especializada do repositório de pacientes
/// </summary>
public class PacienteRepository : Repository<Paciente>, IPacienteRepository
{
    /// <summary>
    /// Construtor que recebe o contexto do banco de dados
    /// </summary>
    /// <param name="context">Contexto do EF Core</param>
    public PacienteRepository(ApplicationDbContext context) : base(context)
    {
    }

    /// <summary>
    /// Busca um paciente pelo seu CPF
    /// </summary>
    public async Task<Paciente?> BuscarPorCpfAsync(string cpf, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .FirstOrDefaultAsync(p => p.CPF == cpf, cancellationToken);
    }

    /// <summary>
    /// Obtém um paciente com seu histórico de consultas
    /// </summary>
    public async Task<Paciente?> ObterComHistoricoConsultasAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(p => p.Consultas)
                .ThenInclude(c => c.Medico)
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
    }

    /// <summary>
    /// Busca pacientes por nome (busca parcial)
    /// </summary>
    public async Task<IEnumerable<Paciente>> BuscarPorNomeAsync(string nome, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(p => p.Nome.Contains(nome))
            .OrderBy(p => p.Nome)
            .ToListAsync(cancellationToken);
    }

    /// <summary>
    /// Verifica se um CPF já está em uso
    /// </summary>
    public async Task<bool> CpfJaExisteAsync(string cpf, Guid? pacienteId = null, CancellationToken cancellationToken = default)
    {
        var query = _dbSet.Where(p => p.CPF == cpf);

        if (pacienteId.HasValue)
        {
            query = query.Where(p => p.Id != pacienteId.Value);
        }

        return await query.AnyAsync(cancellationToken);
    }

    /// <summary>
    /// Obtém pacientes com consultas no período especificado
    /// </summary>
    public async Task<IEnumerable<Paciente>> ObterComConsultasNoPeriodoAsync(DateTime dataInicio, DateTime dataFim, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(p => p.Consultas.Where(c => c.DataHora >= dataInicio && c.DataHora <= dataFim))
                .ThenInclude(c => c.Medico)
            .Where(p => p.Consultas.Any(c => c.DataHora >= dataInicio && c.DataHora <= dataFim))
            .OrderBy(p => p.Nome)
            .ToListAsync(cancellationToken);
    }

    /// <summary>
    /// Obtém pacientes aniversariantes no mês especificado
    /// </summary>
    public async Task<IEnumerable<Paciente>> ObterAniversariantesDoMesAsync(int mes, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(p => p.DataNascimento.Month == mes)
            .OrderBy(p => p.DataNascimento.Day)
            .ThenBy(p => p.Nome)
            .ToListAsync(cancellationToken);
    }
}