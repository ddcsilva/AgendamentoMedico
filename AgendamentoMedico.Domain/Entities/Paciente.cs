namespace AgendamentoMedico.Domain.Entities;

/// <summary>
/// Entidade que representa um paciente no sistema
/// </summary>
public class Paciente : AuditableEntity
{
    private readonly List<Consulta> _consultas;

    /// <summary>
    /// Construtor para criar um novo paciente
    /// </summary>
    /// <param name="nome">Nome completo do paciente</param>
    /// <param name="cpf">CPF do paciente</param>
    /// <param name="dataNascimento">Data de nascimento</param>
    public Paciente(string nome, string cpf, DateTime dataNascimento) : base()
    {
        Nome = nome ?? throw new ArgumentNullException(nameof(nome));
        CPF = cpf ?? throw new ArgumentNullException(nameof(cpf));
        DataNascimento = dataNascimento;
        _consultas = [];
    }

    /// <summary>
    /// Construtor privado para uso do Entity Framework
    /// </summary>
    private Paciente() : base()
    {
        _consultas = [];
        Nome = string.Empty;
        CPF = string.Empty;
    }

    public string Nome { get; private set; }
    public string CPF { get; private set; }
    public DateTime DataNascimento { get; private set; }

    /// <summary>
    /// Consultas do paciente (somente leitura)
    /// </summary>
    public IReadOnlyList<Consulta> Consultas => _consultas.AsReadOnly();

    /// <summary>
    /// Adiciona uma consulta ao paciente
    /// </summary>
    /// <param name="consulta">Consulta a ser adicionada</param>
    public void AdicionarConsulta(Consulta consulta)
    {
        ArgumentNullException.ThrowIfNull(consulta);

        if (consulta.PacienteId != Id)
        {
            throw new InvalidOperationException("A consulta deve pertencer a este paciente");
        }

        _consultas.Add(consulta);
    }

    /// <summary>
    /// Atualiza informações do paciente
    /// </summary>
    /// <param name="nome">Novo nome</param>
    /// <param name="atualizadoPor">Usuário que fez a atualização</param>
    public void AtualizarInformacoes(string nome, string? atualizadoPor = null)
    {
        Nome = nome ?? throw new ArgumentNullException(nameof(nome));

        // Marca como atualizada automaticamente
        MarcarComoAtualizada(atualizadoPor);
    }

    /// <summary>
    /// Calcula a idade do paciente
    /// </summary>
    /// <returns>Idade em anos</returns>
    public int CalcularIdade()
    {
        var hoje = DateTime.Today;
        var idade = hoje.Year - DataNascimento.Year;

        if (DataNascimento.Date > hoje.AddYears(-idade))
        {
            idade--;
        }

        return idade;
    }
}