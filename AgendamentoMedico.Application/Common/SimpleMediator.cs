namespace AgendamentoMedico.Application.Common;

/// <summary>
/// Implementação simples e eficaz do Mediator Pattern
/// </summary>
public class SimpleMediator(IServiceProvider serviceProvider) : IMediator
{
    public async Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
    {
        var requestType = request.GetType();
        var handlerType = typeof(IRequestHandler<,>).MakeGenericType(requestType, typeof(TResponse));
        var handler = serviceProvider.GetService(handlerType) ?? throw new InvalidOperationException(
                $"Nenhum handler encontrado para {requestType.Name}. " +
                $"Certifique-se de registrar uma classe que implementa {handlerType.Name}");
        try
        {
            var result = await ((dynamic)handler).Handle((dynamic)request, cancellationToken);
            return result;
        }
        catch (Exception)
        {
            throw;
        }
    }
}