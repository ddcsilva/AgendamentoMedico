using AgendamentoMedico.Domain.Entities;
using AgendamentoMedico.Application.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace AgendamentoMedico.Infrastructure.Data;

/// <summary>
/// Contexto do banco de dados da aplicação
/// </summary>
public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : IdentityDbContext<Usuario, IdentityRole<Guid>, Guid>(options), IApplicationDbContext
{
    public DbSet<Medico> Medicos { get; set; } = null!;
    public DbSet<Paciente> Pacientes { get; set; } = null!;
    public DbSet<Consulta> Consultas { get; set; } = null!;
    public DbSet<PerfilUsuario> PerfisUsuarios { get; set; } = null!;

    /// <summary>
    /// Verifica se é possível conectar ao banco de dados
    /// </summary>
    /// <returns>True se a conexão for bem-sucedida</returns>
    public async Task<bool> CanConnectAsync()
    {
        try
        {
            return await Database.CanConnectAsync();
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// Executa as migrations pendentes
    /// </summary>
    /// <returns>Task representando a operação assíncrona</returns>
    public async Task MigrateAsync()
    {
        await Database.MigrateAsync();
    }

    /// <summary>
    /// Verifica se há migrations pendentes
    /// </summary>
    /// <returns>True se há migrations pendentes</returns>
    public async Task<bool> HasPendingMigrationsAsync()
    {
        var pendingMigrations = await Database.GetPendingMigrationsAsync();
        return pendingMigrations.Any();
    }

    /// <summary>
    /// Configuração do modelo de dados
    /// </summary>
    /// <param name="modelBuilder">Construtor do modelo</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Aplica todas as configurações do assembly atual
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

        // Configurações do Identity para usar Guid
        ConfigureIdentity(modelBuilder);

        // Configuração global para SQLite
        ConfigureSqliteConventions(modelBuilder);
    }

    /// <summary>
    /// Configurações do ASP.NET Core Identity
    /// </summary>
    /// <param name="modelBuilder">Construtor do modelo</param>
    private static void ConfigureIdentity(ModelBuilder modelBuilder)
    {
        // Renomear tabelas do Identity para português
        modelBuilder.Entity<Usuario>().ToTable("Usuarios");
        modelBuilder.Entity<IdentityRole<Guid>>().ToTable("Roles");
        modelBuilder.Entity<IdentityUserRole<Guid>>().ToTable("UsuarioRoles");
        modelBuilder.Entity<IdentityUserClaim<Guid>>().ToTable("UsuarioClaims");
        modelBuilder.Entity<IdentityUserLogin<Guid>>().ToTable("UsuarioLogins");
        modelBuilder.Entity<IdentityUserToken<Guid>>().ToTable("UsuarioTokens");
        modelBuilder.Entity<IdentityRoleClaim<Guid>>().ToTable("RoleClaims");
    }

    /// <summary>
    /// Configurações específicas para SQLite
    /// </summary>
    /// <param name="modelBuilder">Construtor do modelo</param>
    private static void ConfigureSqliteConventions(ModelBuilder modelBuilder)
    {
        // Remove convenções de cascade delete para evitar problemas no SQLite
        foreach (var foreignKey in modelBuilder.Model.GetEntityTypes()
            .SelectMany(e => e.GetForeignKeys()))
        {
            foreignKey.DeleteBehavior = DeleteBehavior.Restrict;
        }

        // Configuração de precisão para DateTime no SQLite
        foreach (var property in modelBuilder.Model.GetEntityTypes()
            .SelectMany(e => e.GetProperties())
            .Where(p => p.ClrType == typeof(DateTime) || p.ClrType == typeof(DateTime?)))
        {
            property.SetColumnType("TEXT");
        }
    }
}