namespace AgendamentoMedico.Domain.Entities;

/// <summary>
/// Entidade que representa um paciente no sistema
/// </summary>
public class Paciente : AuditableEntity
{
    private readonly List<Consulta> _consultas;

    public Paciente()
    {
        _consultas = [];
    }

    public required string Nome { get; set; }
    public required string CPF { get; set; }
    public required DateTime DataNascimento { get; set; }

    public IReadOnlyList<Consulta> Consultas => _consultas.AsReadOnly();

    public void AdicionarConsulta(Consulta consulta)
    {
        ArgumentNullException.ThrowIfNull(consulta);

        if (consulta.PacienteId != Id)
        {
            throw new InvalidOperationException("A consulta deve pertencer a este paciente");
        }

        _consultas.Add(consulta);
    }

    public void AtualizarInformacoes(string nome, string? atualizadoPor = null)
    {
        Nome = nome ?? throw new ArgumentNullException(nameof(nome));

        MarcarComoAtualizada(atualizadoPor);
    }

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