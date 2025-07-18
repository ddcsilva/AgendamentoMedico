using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AgendamentoMedico.Domain.Entities;

namespace AgendamentoMedico.Infrastructure.Data.Configurations;

/// <summary>
/// Configuração da entidade Paciente para o Entity Framework Core
/// </summary>
public class PacienteConfiguration : IEntityTypeConfiguration<Paciente>
{
    /// <summary>
    /// Configura a entidade Paciente
    /// </summary>
    /// <param name="builder">Construtor de configuração da entidade</param>
    public void Configure(EntityTypeBuilder<Paciente> builder)
    {
        // Configuração da tabela
        builder.ToTable("Pacientes");

        // Configuração da chave primária
        builder.HasKey(p => p.Id);

        // Configuração das propriedades
        builder.Property(p => p.Id)
            .ValueGeneratedNever() // Guid gerado pela aplicação
            .HasComment("Identificador único do paciente");

        builder.Property(p => p.Nome)
            .HasMaxLength(200)
            .IsRequired()
            .HasComment("Nome completo do paciente");

        builder.Property(p => p.CPF)
            .HasMaxLength(14)
            .IsRequired()
            .HasComment("CPF do paciente");

        builder.Property(p => p.DataNascimento)
            .IsRequired()
            .HasColumnType("DATE")
            .HasComment("Data de nascimento do paciente");

        // Configuração das propriedades de auditoria
        builder.Property(p => p.CriadoEm)
            .IsRequired()
            .HasDefaultValueSql("datetime('now')")
            .HasComment("Data e hora de criação");

        builder.Property(p => p.AtualizadoEm)
            .HasComment("Data e hora da última atualização");

        builder.Property(p => p.CriadoPor)
            .HasMaxLength(100)
            .HasComment("Usuário que criou o registro");

        builder.Property(p => p.AtualizadoPor)
            .HasMaxLength(100)
            .HasComment("Usuário que fez a última atualização");

        // Configuração dos relacionamentos
        builder.HasMany(p => p.Consultas)
            .WithOne(c => c.Paciente)
            .HasForeignKey(c => c.PacienteId)
            .OnDelete(DeleteBehavior.Restrict);

        // Configuração de backing field para collections encapsuladas
        builder.Navigation(p => p.Consultas)
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasField("_consultas");

        // Configuração de índices
        builder.HasIndex(p => p.CPF)
            .IsUnique()
            .HasDatabaseName("IX_Pacientes_CPF");

        builder.HasIndex(p => p.Nome)
            .HasDatabaseName("IX_Pacientes_Nome");

        builder.HasIndex(p => p.DataNascimento)
            .HasDatabaseName("IX_Pacientes_DataNascimento");

        // Configuração de auditoria
        builder.HasIndex(p => p.CriadoEm)
            .HasDatabaseName("IX_Pacientes_CriadoEm");
    }
}