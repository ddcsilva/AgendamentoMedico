using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AgendamentoMedico.Domain.Entities;

namespace AgendamentoMedico.Infrastructure.Data.Configurations;

/// <summary>
/// Configuração da entidade Usuario para o Entity Framework Core
/// </summary>
public class UsuarioConfiguration : IEntityTypeConfiguration<Usuario>
{
    /// <summary>
    /// Configura a entidade Usuario
    /// </summary>
    /// <param name="builder">Construtor de configuração da entidade</param>
    public void Configure(EntityTypeBuilder<Usuario> builder)
    {
        // A tabela já é configurada pelo Identity, apenas adicionamos propriedades customizadas

        // Configuração das propriedades customizadas
        builder.Property(u => u.Nome)
            .HasMaxLength(200)
            .IsRequired()
            .HasComment("Nome completo do usuário");

        builder.Property(u => u.Ativo)
            .IsRequired()
            .HasDefaultValue(true)
            .HasComment("Indica se o usuário está ativo");

        builder.Property(u => u.UltimoLogin)
            .HasComment("Data e hora do último login");

        // Propriedades de auditoria
        builder.Property(u => u.CriadoEm)
            .IsRequired()
            .HasDefaultValueSql("datetime('now')")
            .HasComment("Data e hora de criação");

        builder.Property(u => u.AtualizadoEm)
            .HasComment("Data e hora da última atualização");

        builder.Property(u => u.CriadoPor)
            .HasMaxLength(100)
            .HasComment("Usuário que criou o registro");

        builder.Property(u => u.AtualizadoPor)
            .HasMaxLength(100)
            .HasComment("Usuário que fez a última atualização");

        // Relacionamentos
        builder.HasMany(u => u.Perfis)
            .WithOne(p => p.Usuario)
            .HasForeignKey(p => p.UsuarioId)
            .OnDelete(DeleteBehavior.Cascade);

        // Configuração de backing field para collections encapsuladas
        builder.Navigation(u => u.Perfis)
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasField("_perfis");

        // Índices customizados
        builder.HasIndex(u => u.Nome)
            .HasDatabaseName("IX_Usuarios_Nome");

        builder.HasIndex(u => u.Ativo)
            .HasDatabaseName("IX_Usuarios_Ativo");

        builder.HasIndex(u => u.UltimoLogin)
            .HasDatabaseName("IX_Usuarios_UltimoLogin");

        builder.HasIndex(u => u.CriadoEm)
            .HasDatabaseName("IX_Usuarios_CriadoEm");
    }
}