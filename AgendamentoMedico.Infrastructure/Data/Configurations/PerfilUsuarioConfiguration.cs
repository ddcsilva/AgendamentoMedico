using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AgendamentoMedico.Domain.Entities;

namespace AgendamentoMedico.Infrastructure.Data.Configurations;

/// <summary>
/// Configuração da entidade PerfilUsuario para o Entity Framework Core
/// </summary>
public class PerfilUsuarioConfiguration : IEntityTypeConfiguration<PerfilUsuario>
{
    /// <summary>
    /// Configura a entidade PerfilUsuario
    /// </summary>
    /// <param name="builder">Construtor de configuração da entidade</param>
    public void Configure(EntityTypeBuilder<PerfilUsuario> builder)
    {
        // Configuração da tabela
        builder.ToTable("PerfisUsuarios");

        // Configuração da chave primária
        builder.HasKey(p => p.Id);

        // Configuração das propriedades
        builder.Property(p => p.Id)
            .ValueGeneratedNever() // Guid gerado pela aplicação
            .HasComment("Identificador único do perfil de usuário");

        builder.Property(p => p.UsuarioId)
            .IsRequired()
            .HasComment("Identificador do usuário");

        builder.Property(p => p.TipoClaim)
            .HasMaxLength(100)
            .IsRequired()
            .HasComment("Tipo do claim (ex: permission, role, especialidade)");

        builder.Property(p => p.ValorClaim)
            .HasMaxLength(200)
            .IsRequired()
            .HasComment("Valor do claim");

        builder.Property(p => p.Descricao)
            .HasMaxLength(500)
            .HasComment("Descrição opcional do perfil");

        // Propriedades de auditoria
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

        // Relacionamentos
        builder.HasOne(p => p.Usuario)
            .WithMany(u => u.Perfis)
            .HasForeignKey(p => p.UsuarioId)
            .OnDelete(DeleteBehavior.Cascade);

        // Índices
        builder.HasIndex(p => p.UsuarioId)
            .HasDatabaseName("IX_PerfisUsuarios_UsuarioId");

        builder.HasIndex(p => p.TipoClaim)
            .HasDatabaseName("IX_PerfisUsuarios_TipoClaim");

        builder.HasIndex(p => new { p.UsuarioId, p.TipoClaim, p.ValorClaim })
            .IsUnique()
            .HasDatabaseName("UK_PerfisUsuarios_Usuario_Claim");

        builder.HasIndex(p => p.CriadoEm)
            .HasDatabaseName("IX_PerfisUsuarios_CriadoEm");
    }
}