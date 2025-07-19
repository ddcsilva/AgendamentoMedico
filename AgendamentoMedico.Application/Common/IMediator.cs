namespace AgendamentoMedico.Application.Common;

/// <summary>
/// Interface que define o contrato para o mediator
/// </summary>
public interface IMediator
{
    /// <summary>
    /// Envia uma requisição e retorna a resposta
    /// </summary>
    /// <typeparam name="TResponse">Tipo da resposta que será retornada</typeparam>
    /// <param name="request">A requisição a ser processada</param>
    /// <param name="cancellationToken">Token para cancelar a operação se necessário</param>
    /// <returns>A resposta do tipo TResponse</returns>
    Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default);
}