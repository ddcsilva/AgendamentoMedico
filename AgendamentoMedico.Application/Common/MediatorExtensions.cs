using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace AgendamentoMedico.Application.Common;

/// <summary>
/// Extensões para facilitar o registro do Mediator e handlers no container de DI
/// </summary>
public static class MediatorExtensions
{
    public static IServiceCollection AddSimpleMediator(this IServiceCollection services, params Assembly[] assemblies)
    {
        services.AddScoped<IMediator, SimpleMediator>();

        foreach (var assembly in assemblies)
        {
            RegisterHandlersFromAssembly(services, assembly);
        }

        return services;
    }

    private static void RegisterHandlersFromAssembly(IServiceCollection services, Assembly assembly)
    {
        var handlerInterface = typeof(IRequestHandler<,>);

        var handlerTypes = assembly.GetTypes()
            .Where(type =>
                type.IsClass &&
                !type.IsAbstract &&
                type.GetInterfaces().Any(interfaceType =>
                    interfaceType.IsGenericType &&
                    interfaceType.GetGenericTypeDefinition() == handlerInterface))
            .ToList();

        foreach (var handlerType in handlerTypes)
        {
            var implementedInterfaces = handlerType.GetInterfaces()
                .Where(interfaceType =>
                    interfaceType.IsGenericType &&
                    interfaceType.GetGenericTypeDefinition() == handlerInterface)
                .ToList();

            foreach (var implementedInterface in implementedInterfaces)
            {
                services.AddScoped(implementedInterface, handlerType);
            }
        }
    }
}