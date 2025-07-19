using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using AgendamentoMedico.Application.Interfaces;
using AgendamentoMedico.Domain.Interfaces;
using AgendamentoMedico.Domain.Entities;
using AgendamentoMedico.Infrastructure.Data;
using AgendamentoMedico.Infrastructure.Repositories;
using AgendamentoMedico.Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

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
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            options.UseSqlite(connectionString);

            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development")
            {
                options.EnableSensitiveDataLogging();
                options.EnableDetailedErrors();
            }
        });

        AddIdentity(services);

        AddJwtAuthentication(services, configuration);

        services.AddScoped<IApplicationDbContext>(provider =>
            provider.GetRequiredService<ApplicationDbContext>());

        AddServices(services);

        AddRepositories(services);

        return services;
    }

    /// <summary>
    /// Configura o ASP.NET Core Identity
    /// </summary>
    /// <param name="services">Coleção de serviços</param>
    private static void AddIdentity(IServiceCollection services)
    {
        services.AddIdentity<Usuario, IdentityRole<Guid>>(options =>
        {
            options.Password.RequiredLength = 6;
            options.Password.RequireDigit = true;
            options.Password.RequireLowercase = true;
            options.Password.RequireUppercase = true;
            options.Password.RequireNonAlphanumeric = false;

            options.User.RequireUniqueEmail = true;

            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
            options.Lockout.MaxFailedAccessAttempts = 5;
            options.Lockout.AllowedForNewUsers = true;
        })
        .AddEntityFrameworkStores<ApplicationDbContext>()
        .AddDefaultTokenProviders();
    }

    /// <summary>
    /// Configura autenticação JWT
    /// </summary>
    /// <param name="services">Coleção de serviços</param>
    /// <param name="configuration">Configuração da aplicação</param>
    private static void AddJwtAuthentication(IServiceCollection services, IConfiguration configuration)
    {
        var jwtSettings = configuration.GetSection("JwtSettings");
        var secretKey = jwtSettings["SecretKey"] ?? throw new InvalidOperationException("JWT SecretKey não configurada");

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = IdentityConstants.ApplicationScheme;
            options.DefaultChallengeScheme = IdentityConstants.ApplicationScheme;
            options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
                ValidateIssuer = true,
                ValidIssuer = jwtSettings["Issuer"] ?? "AgendamentoMedico",
                ValidateAudience = true,
                ValidAudience = jwtSettings["Audience"] ?? "AgendamentoMedico",
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };
        });

        services.AddAuthorization();
    }

    /// <summary>
    /// Registra os serviços no container de DI
    /// </summary>
    /// <param name="services">Coleção de serviços</param>
    private static void AddServices(IServiceCollection services)
    {
        services.AddScoped<IJwtTokenService, JwtTokenService>();
        services.AddScoped<IUserManager, UserManagerService>();
        services.AddScoped<ISignInManager, SignInManagerService>();
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