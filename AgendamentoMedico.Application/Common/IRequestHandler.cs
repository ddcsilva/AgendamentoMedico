namespace AgendamentoMedico.Application.Common;

/// <summary>
/// Interface que define o contrato para handlers que processam requisições
/// </summary>
/// <typeparam name="TRequest">Tipo da requisição que este handler pode processar</typeparam>
/// <typeparam name="TResponse">Tipo da resposta que este handler retorna</typeparam>
public interface IRequestHandler<in TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    /// <summary>
    /// Processa a requisição e retorna a resposta
    /// </summary>
    /// <param name="request">A requisição a ser processada</param>
    /// <param name="cancellationToken">Token para cancelar a operação se necessário</param>
    /// <returns>A resposta do tipo TResponse</returns>
    Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken);
}