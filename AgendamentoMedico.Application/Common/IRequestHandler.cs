namespace AgendamentoMedico.Application.Common;

/// <summary>
/// Interface que define o contrato para handlers que processam requisições
///
/// 🎯 CONCEITO: Um Handler é uma classe que contém a LÓGICA DE NEGÓCIO para processar
/// uma requisição específica. Cada requisição tem exatamente UM handler.
///
/// 📋 PRINCÍPIOS:
/// - Single Responsibility: Cada handler faz apenas uma coisa
/// - Separation of Concerns: A lógica fica isolada do controller/endpoint
/// - Testabilidade: Cada handler pode ser testado independentemente
///
/// Exemplo de uso:
/// public class CriarMedicoHandler : IRequestHandler&lt;CriarMedicoCommand, Medico&gt;
/// {
///     public async Task&lt;Medico&gt; Handle(CriarMedicoCommand request, CancellationToken cancellationToken)
///     {
///         // Lógica para criar médico aqui
///         return new Medico { Nome = request.Nome, CRM = request.CRM };
///     }
/// }
/// </summary>
/// <typeparam name="TRequest">Tipo da requisição que este handler pode processar</typeparam>
/// <typeparam name="TResponse">Tipo da resposta que este handler retorna</typeparam>
public interface IRequestHandler<in TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    /// <summary>
    /// Processa a requisição e retorna a resposta
    /// </summary>
    /// <param name="request">A requisição a ser processada</param>
    /// <param name="cancellationToken">Token para cancelar a operação se necessário</param>
    /// <returns>A resposta do tipo TResponse</returns>
    Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken);
}