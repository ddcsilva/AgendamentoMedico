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
    /// <param name="nome">Nome completo do médico</param>
    /// <param name="crm">Número do CRM</param>
    /// <param name="especialidade">Especialidade médica</param>
    public Medico(string nome, string crm, string especialidade) : base()
    {
        Nome = nome ?? throw new ArgumentNullException(nameof(nome));
        CRM = crm ?? throw new ArgumentNullException(nameof(crm));
        Especialidade = especialidade ?? throw new ArgumentNullException(nameof(especialidade));
        _consultas = [];
    }

    /// <summary>
    /// Construtor privado para uso do Entity Framework
    /// </summary>
    private Medico() : base()
    {
        _consultas = [];
        Nome = string.Empty;
        CRM = string.Empty;
        Especialidade = string.Empty;
    }

    public string Nome { get; private set; }
    public string CRM { get; private set; }
    public string Especialidade { get; private set; }

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