using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using AgendamentoMedico.Application.Interfaces;
using AgendamentoMedico.Domain.Interfaces;
using AgendamentoMedico.Infrastructure.Data;
using AgendamentoMedico.Infrastructure.Repositories;

namespace AgendamentoMedico.Infrastructure;

/// <summary>
/// Extensões para injeção de dependência da camada de infraestrutura
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Adiciona os serviços de infraestrutura ao container de DI
    /// </summary>
    /// <param name="services">Coleção de serviços</param>
    /// <param name="configuration">Configuração da aplicação</param>
    /// <returns>Coleção de serviços para encadeamento</returns>
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // Configuração do Entity Framework com SQLite
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            options.UseSqlite(connectionString);

            // Configurações específicas para desenvolvimento
            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development")
            {
                options.EnableSensitiveDataLogging();
                options.EnableDetailedErrors();
            }
        });

        // Registra a interface do contexto
        services.AddScoped<IApplicationDbContext>(provider =>
            provider.GetRequiredService<ApplicationDbContext>());

        // Registra os repositórios com lifetime Scoped
        AddRepositories(services);

        return services;
    }

    /// <summary>
    /// Registra todos os repositórios no container de DI
    /// </summary>
    /// <param name="services">Coleção de serviços</param>
    private static void AddRepositories(IServiceCollection services)
    {
        services.AddScoped<IMedicoRepository, MedicoRepository>();
        services.AddScoped<IPacienteRepository, PacienteRepository>();
        services.AddScoped<IConsultaRepository, ConsultaRepository>();
    }
}