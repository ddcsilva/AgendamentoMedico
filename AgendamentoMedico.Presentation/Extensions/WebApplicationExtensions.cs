using AgendamentoMedico.Application.Interfaces;

namespace AgendamentoMedico.Presentation.Extensions;

/// <summary>
/// Extensões para configuração da aplicação web
/// </summary>
public static class WebApplicationExtensions
{
    /// <summary>
    /// Executa migrations automaticamente no ambiente de desenvolvimento
    /// </summary>
    /// <param name="app">Instância da aplicação web</param>
    /// <returns>Task representando a operação assíncrona</returns>
    public static async Task<WebApplication> UseDevelopmentMigrationsAsync(this WebApplication app)
    {
        if (!app.Environment.IsDevelopment())
            return app;

        using var scope = app.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<IApplicationDbContext>();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

        try
        {
            await dbContext.MigrateAsync();
            logger.LogInformation("✅ Migrations executadas com sucesso!");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "❌ Erro ao executar migrations");
        }

        return app;
    }
}