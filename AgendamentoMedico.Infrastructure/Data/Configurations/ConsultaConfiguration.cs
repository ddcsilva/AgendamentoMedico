using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AgendamentoMedico.Domain.Entities;

namespace AgendamentoMedico.Infrastructure.Data.Configurations;

/// <summary>
/// Configuração da entidade Consulta para o Entity Framework Core
/// </summary>
public class ConsultaConfiguration : IEntityTypeConfiguration<Consulta>
{
    /// <summary>
    /// Configura a entidade Consulta
    /// </summary>
    /// <param name="builder">Construtor de configuração da entidade</param>
    public void Configure(EntityTypeBuilder<Consulta> builder)
    {
        // Configuração da tabela
        builder.ToTable("Consultas");

        // Configuração da chave primária
        builder.HasKey(c => c.Id);

        // Configuração das propriedades
        builder.Property(c => c.Id)
            .ValueGeneratedNever() // Guid gerado pela aplicação
            .HasComment("Identificador único da consulta");

        builder.Property(c => c.MedicoId)
            .IsRequired()
            .HasComment("Identificador do médico");

        builder.Property(c => c.PacienteId)
            .IsRequired()
            .HasComment("Identificador do paciente");

        builder.Property(c => c.DataHora)
            .IsRequired()
            .HasColumnType("DATETIME")
            .HasComment("Data e hora da consulta");

        builder.Property(c => c.Observacoes)
            .HasMaxLength(1000)
            .HasComment("Observações da consulta");

        // Configuração das propriedades de auditoria
        builder.Property(c => c.CriadoEm)
            .IsRequired()
            .HasDefaultValueSql("datetime('now')")
            .HasComment("Data e hora de criação");

        builder.Property(c => c.AtualizadoEm)
            .HasComment("Data e hora da última atualização");

        builder.Property(c => c.CriadoPor)
            .HasMaxLength(100)
            .HasComment("Usuário que criou o registro");

        builder.Property(c => c.AtualizadoPor)
            .HasMaxLength(100)
            .HasComment("Usuário que fez a última atualização");

        // Configuração dos relacionamentos
        builder.HasOne(c => c.Medico)
            .WithMany(m => m.Consultas)
            .HasForeignKey(c => c.MedicoId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(c => c.Paciente)
            .WithMany(p => p.Consultas)
            .HasForeignKey(c => c.PacienteId)
            .OnDelete(DeleteBehavior.Restrict);

        // Configuração de índices
        builder.HasIndex(c => c.MedicoId)
            .HasDatabaseName("IX_Consultas_MedicoId");

        builder.HasIndex(c => c.PacienteId)
            .HasDatabaseName("IX_Consultas_PacienteId");

        builder.HasIndex(c => c.DataHora)
            .HasDatabaseName("IX_Consultas_DataHora");

        // Índice composto para consultas por médico e data
        builder.HasIndex(c => new { c.MedicoId, c.DataHora })
            .HasDatabaseName("IX_Consultas_MedicoId_DataHora");

        // Índice composto para consultas por paciente e data
        builder.HasIndex(c => new { c.PacienteId, c.DataHora })
            .HasDatabaseName("IX_Consultas_PacienteId_DataHora");

        // Configuração de auditoria
        builder.HasIndex(c => c.CriadoEm)
            .HasDatabaseName("IX_Consultas_CriadoEm");

        // Constraint para evitar conflitos de horário (mesmo médico no mesmo horário)
        builder.HasIndex(c => new { c.MedicoId, c.DataHora })
            .IsUnique()
            .HasDatabaseName("UK_Consultas_MedicoId_DataHora");
    }
}