namespace AgendamentoMedico.Domain.Entities;

/// <summary>
/// Entidade que representa um médico no sistema
/// </summary>
public class Medico : AuditableEntity
{
    private readonly List<Consulta> _consultas;

    /// <summary>
    /// Construtor para criar um novo médico
    /// </summary>
    public Medico()
    {
        _consultas = [];
    }

    public required string Nome { get; set; }
    public required string CRM { get; set; }
    public required string Especialidade { get; set; }

    /// <summary>
    /// Consultas do médico (somente leitura)
    /// </summary>
    public IReadOnlyList<Consulta> Consultas => _consultas.AsReadOnly();

    /// <summary>
    /// Adiciona uma consulta ao médico
    /// </summary>
    /// <param name="consulta">Consulta a ser adicionada</param>
    public void AdicionarConsulta(Consulta consulta)
    {
        ArgumentNullException.ThrowIfNull(consulta);

        if (consulta.MedicoId != Id)
            throw new InvalidOperationException("A consulta deve pertencer a este médico");

        _consultas.Add(consulta);
    }

    /// <summary>
    /// Atualiza informações do médico
    /// </summary>
    /// <param name="nome">Novo nome</param>
    /// <param name="especialidade">Nova especialidade</param>
    /// <param name="atualizadoPor">Usuário que fez a atualização</param>
    public void AtualizarInformacoes(string nome, string especialidade, string? atualizadoPor = null)
    {
        Nome = nome ?? throw new ArgumentNullException(nameof(nome));
        Especialidade = especialidade ?? throw new ArgumentNullException(nameof(especialidade));

        // Marca como atualizada automaticamente
        MarcarComoAtualizada(atualizadoPor);
    }
}