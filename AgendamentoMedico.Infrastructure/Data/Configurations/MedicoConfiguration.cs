using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AgendamentoMedico.Domain.Entities;

namespace AgendamentoMedico.Infrastructure.Data.Configurations;

/// <summary>
/// Configuração da entidade Medico para o Entity Framework Core
/// </summary>
public class MedicoConfiguration : IEntityTypeConfiguration<Medico>
{
    /// <summary>
    /// Configura a entidade Medico
    /// </summary>
    /// <param name="builder">Construtor de configuração da entidade</param>
    public void Configure(EntityTypeBuilder<Medico> builder)
    {
        // Configuração da tabela
        builder.ToTable("Medicos");

        // Configuração da chave primária
        builder.HasKey(m => m.Id);

        // Configuração das propriedades
        builder.Property(m => m.Id)
            .ValueGeneratedNever() // Guid gerado pela aplicação
            .HasComment("Identificador único do médico");

        builder.Property(m => m.Nome)
            .HasMaxLength(200)
            .IsRequired()
            .HasComment("Nome completo do médico");

        builder.Property(m => m.CRM)
            .HasMaxLength(20)
            .IsRequired()
            .HasComment("Número do CRM do médico");

        builder.Property(m => m.Especialidade)
            .HasMaxLength(100)
            .IsRequired()
            .HasComment("Especialidade médica");

        // Configuração das propriedades de auditoria
        builder.Property(m => m.CriadoEm)
            .IsRequired()
            .HasDefaultValueSql("datetime('now')")
            .HasComment("Data e hora de criação");

        builder.Property(m => m.AtualizadoEm)
            .HasComment("Data e hora da última atualização");

        builder.Property(m => m.CriadoPor)
            .HasMaxLength(100)
            .HasComment("Usuário que criou o registro");

        builder.Property(m => m.AtualizadoPor)
            .HasMaxLength(100)
            .HasComment("Usuário que fez a última atualização");

        // Configuração dos relacionamentos
        builder.HasMany(m => m.Consultas)
            .WithOne(c => c.Medico)
            .HasForeignKey(c => c.MedicoId)
            .OnDelete(DeleteBehavior.Restrict);

        // Configuração de backing field para collections encapsuladas
        builder.Navigation(m => m.Consultas)
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasField("_consultas");

        // Configuração de índices
        builder.HasIndex(m => m.CRM)
            .IsUnique()
            .HasDatabaseName("IX_Medicos_CRM");

        builder.HasIndex(m => m.Nome)
            .HasDatabaseName("IX_Medicos_Nome");

        builder.HasIndex(m => m.Especialidade)
            .HasDatabaseName("IX_Medicos_Especialidade");

        // Configuração de auditoria
        builder.HasIndex(m => m.CriadoEm)
            .HasDatabaseName("IX_Medicos_CriadoEm");
    }
}