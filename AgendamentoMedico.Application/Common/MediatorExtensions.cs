using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace AgendamentoMedico.Application.Common;

/// <summary>
/// Extensões para facilitar o registro do Mediator e handlers no container de DI
///
/// 🎯 OBJETIVO: Automatizar o tedioso processo de registrar cada handler manualmente.
/// Em vez de fazer:
/// services.AddScoped&lt;IRequestHandler&lt;CriarMedicoCommand, Medico&gt;, CriarMedicoHandler&gt;();
/// services.AddScoped&lt;IRequestHandler&lt;AtualizarPacienteCommand, Paciente&gt;, AtualizarPacienteHandler&gt;();
/// // ... para cada handler
///
/// Fazemos simplesmente:
/// services.AddSimpleMediator(typeof(CriarMedicoHandler).Assembly);
///
/// 🔍 COMO FUNCIONA:
/// 1. Scanneia um assembly procurando por classes que implementam IRequestHandler&lt;,&gt;
/// 2. Registra automaticamente cada uma no container de DI
/// 3. Registra o IMediator também
/// </summary>
public static class MediatorExtensions
{
    /// <summary>
    /// Registra o SimpleMediator e auto-descobre todos os handlers no assembly especificado
    ///
    /// 🔄 PROCESSO:
    /// 1. Registra IMediator → SimpleMediator
    /// 2. Varre o assembly procurando implementações de IRequestHandler&lt;,&gt;
    /// 3. Para cada handler encontrado, registra no DI como Scoped
    ///
    /// 📋 EXEMPLO DE USO:
    /// // No Program.cs
    /// builder.Services.AddSimpleMediator(typeof(CriarMedicoHandler).Assembly);
    ///
    /// // Ou para registrar handlers de múltiplos assemblies:
    /// builder.Services.AddSimpleMediator(
    ///     typeof(CriarMedicoHandler).Assembly,
    ///     typeof(CriarPacienteHandler).Assembly
    /// );
    /// </summary>
    public static IServiceCollection AddSimpleMediator(this IServiceCollection services, params Assembly[] assemblies)
    {
        // ✅ PASSO 1: Registrar o próprio mediator
        services.AddScoped<IMediator, SimpleMediator>();

        Console.WriteLine("🔧 Registrando SimpleMediator...");

        // ✅ PASSO 2: Auto-descobrir e registrar handlers
        foreach (var assembly in assemblies)
        {
            RegisterHandlersFromAssembly(services, assembly);
        }

        Console.WriteLine("🎉 SimpleMediator configurado com sucesso!");
        return services;
    }

    /// <summary>
    /// Registra handlers de um assembly específico
    ///
    /// 🔍 ALGORITMO DE DESCOBERTA:
    /// 1. Pega todos os tipos do assembly
    /// 2. Filtra apenas classes concretas (não interfaces, não abstratas)
    /// 3. Para cada classe, verifica se implementa IRequestHandler&lt;,&gt;
    /// 4. Se sim, registra no container de DI
    /// </summary>
    private static void RegisterHandlersFromAssembly(IServiceCollection services, Assembly assembly)
    {
        Console.WriteLine($"🔍 Scaneando assembly: {assembly.GetName().Name}");

        // 🎯 Tipo que procuramos: IRequestHandler<,> (generic type definition)
        var handlerInterface = typeof(IRequestHandler<,>);

        // 📋 Encontrar todos os tipos que implementam IRequestHandler<,>
        var handlerTypes = assembly.GetTypes()
            .Where(type =>
                type.IsClass &&                          // Deve ser uma classe
                !type.IsAbstract &&                      // Não pode ser abstrata
                type.GetInterfaces().Any(interfaceType => // Deve implementar IRequestHandler<,>
                    interfaceType.IsGenericType &&
                    interfaceType.GetGenericTypeDefinition() == handlerInterface))
            .ToList();

        Console.WriteLine($"📦 Encontrados {handlerTypes.Count} handlers:");

        // 🔄 Registrar cada handler encontrado
        foreach (var handlerType in handlerTypes)
        {
            // Para cada handler, pode implementar múltiplas interfaces IRequestHandler<,>
            var implementedInterfaces = handlerType.GetInterfaces()
                .Where(interfaceType =>
                    interfaceType.IsGenericType &&
                    interfaceType.GetGenericTypeDefinition() == handlerInterface)
                .ToList();

            foreach (var implementedInterface in implementedInterfaces)
            {
                // Registrar como Scoped (uma instância por requisição HTTP)
                services.AddScoped(implementedInterface, handlerType);

                Console.WriteLine($"  ✅ {implementedInterface.Name} → {handlerType.Name}");
            }
        }
    }
}

/// <summary>
/// 🤔 POR QUE SCOPED E NÃO SINGLETON OU TRANSIENT?
///
/// 🔄 SCOPED (recomendado):
/// - Uma instância por requisição HTTP
/// - Permite injetar repositórios que compartilham o mesmo DbContext
/// - Não há problema de concorrência entre requisições
/// - Performance boa (reutiliza na mesma requisição)
///
/// ❌ SINGLETON:
/// - Uma instância para toda a aplicação
/// - Problema: handlers podem ter estado mutável
/// - Problema: injeção de repositórios scoped não funciona
///
/// ⚡ TRANSIENT:
/// - Nova instância toda vez que é solicitado
/// - Funciona, mas é desnecessário overhead
/// - Handlers geralmente são stateless, então Scoped é suficiente
///
/// 💡 DICA: Se seus handlers são completamente stateless e não injetam
/// repositórios, você pode usar Transient para simplicidade.
/// </summary>