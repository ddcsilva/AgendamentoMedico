using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using AgendamentoMedico.Infrastructure.Data;

namespace AgendamentoMedico.Infrastructure;

/// <summary>
/// Extensões para configuração de dependências da camada de infraestrutura
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Adiciona os serviços de infraestrutura ao container de DI
    /// </summary>
    /// <param name="services">Coleção de serviços</param>
    /// <param name="configuration">Configuração da aplicação</param>
    /// <returns>Coleção de serviços configurada</returns>
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Configuração do banco de dados
        services.AddDatabase(configuration);

        return services;
    }

    /// <summary>
    /// Configura o banco de dados SQLite com EF Core
    /// </summary>
    /// <param name="services">Coleção de serviços</param>
    /// <param name="configuration">Configuração da aplicação</param>
    /// <returns>Coleção de serviços configurada</returns>
    private static IServiceCollection AddDatabase(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Obter string de conexão com fallback para SQLite em memória
        var connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? "Data Source=agendamento.db;Cache=Shared;";

        // Configurar o DbContext com SQLite
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseSqlite(connectionString, sqliteOptions =>
            {
                // Configurações específicas do SQLite
                sqliteOptions.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName);
                sqliteOptions.CommandTimeout(30); // 30 segundos de timeout
            });

            // Configurações de desenvolvimento
            if (IsInDevelopment())
            {
                options.EnableSensitiveDataLogging();
                options.EnableDetailedErrors();
            }

            // Configurações de performance
            options.EnableServiceProviderCaching();
            options.EnableThreadSafetyChecks();
        });

        // Registrar a interface do contexto
        services.AddScoped<IApplicationDbContext>(provider =>
            provider.GetRequiredService<ApplicationDbContext>());

        return services;
    }

    /// <summary>
    /// Verifica se a aplicação está em ambiente de desenvolvimento
    /// </summary>
    /// <returns>True se estiver em desenvolvimento</returns>
    private static bool IsInDevelopment()
    {
        var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")
            ?? Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT");

        return string.Equals(environment, "Development", StringComparison.OrdinalIgnoreCase);
    }
}