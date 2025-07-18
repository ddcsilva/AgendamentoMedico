using Microsoft.EntityFrameworkCore;
using AgendamentoMedico.Domain.Entities;
using AgendamentoMedico.Application.Interfaces;

namespace AgendamentoMedico.Infrastructure.Data;

/// <summary>
/// Contexto do banco de dados da aplicação
/// Classe interna para evitar uso direto fora da camada de infraestrutura
/// </summary>
internal class ApplicationDbContext : DbContext, IApplicationDbContext
{
    /// <summary>
    /// Construtor que recebe as opções de configuração
    /// </summary>
    /// <param name="options">Opções de configuração do DbContext</param>
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    /// <summary>
    /// DbSet para Médicos
    /// </summary>
    public DbSet<Medico> Medicos { get; set; } = null!;

    /// <summary>
    /// DbSet para Pacientes
    /// </summary>
    public DbSet<Paciente> Pacientes { get; set; } = null!;

    /// <summary>
    /// DbSet para Consultas
    /// </summary>
    public DbSet<Consulta> Consultas { get; set; } = null!;

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

        // Configuração global para SQLite
        ConfigureSqliteConventions(modelBuilder);
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