namespace AgendamentoMedico.Application.Common;

/// <summary>
/// Interface central do Mediator Pattern - o "despachador" de requisições
///
/// 🎯 CONCEITO: O Mediator é como um "correio" que:
/// 1. Recebe uma requisição (carta)
/// 2. Encontra quem pode processá-la (destinatário correto)
/// 3. Entrega a requisição para o handler apropriado
/// 4. Retorna a resposta
///
/// 📋 BENEFÍCIOS:
/// ✅ Desacoplamento: Controllers não conhecem handlers diretamente
/// ✅ Single Point of Entry: Todas as operações passam pelo mediator
/// ✅ Cross-cutting Concerns: Pode adicionar logging, validação, etc.
/// ✅ Testabilidade: Fácil de mockar e testar
///
/// 🔄 FLUXO:
/// Controller → Mediator.Send(request) → Handler.Handle(request) → Response
///
/// Exemplo de uso:
/// // No controller
/// var command = new CriarMedicoCommand("Dr. João", "12345");
/// var medico = await mediator.Send(command);
/// </summary>
public interface IMediator
{
    /// <summary>
    /// Envia uma requisição para ser processada pelo handler apropriado
    ///
    /// 🔍 COMO FUNCIONA:
    /// 1. Recebe a requisição do tipo IRequest&lt;TResponse&gt;
    /// 2. Usa Reflection para descobrir qual handler pode processá-la
    /// 3. Resolve o handler via Dependency Injection
    /// 4. Chama o método Handle() do handler
    /// 5. Retorna a resposta
    /// </summary>
    /// <typeparam name="TResponse">Tipo da resposta esperada</typeparam>
    /// <param name="request">A requisição a ser processada</param>
    /// <param name="cancellationToken">Token para cancelar a operação</param>
    /// <returns>A resposta do tipo TResponse</returns>
    Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default);
}