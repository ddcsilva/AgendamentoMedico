using System.Reflection;

namespace AgendamentoMedico.Application.Common;

/// <summary>
/// Implementação simples e eficaz do Mediator Pattern
///
/// 🎯 AQUI É ONDE A MÁGICA ACONTECE!
/// Esta classe é o "cérebro" do sistema. Ela usa Reflection e Dependency Injection
/// para descobrir e executar o handler correto para cada requisição.
///
/// 🔍 TÉCNICAS UTILIZADAS:
/// - Reflection: Para descobrir tipos em runtime
/// - Dependency Injection: Para resolver handlers
/// - Generics: Para manter type safety
/// - Dynamic: Para chamadas polimórficas
/// </summary>
public class SimpleMediator(IServiceProvider serviceProvider) : IMediator
{
    /// <summary>
    /// Método principal que processa qualquer requisição
    ///
    /// 🔄 ALGORITMO PASSO A PASSO:
    /// 1. Pega o tipo concreto da requisição (ex: CriarMedicoCommand)
    /// 2. Constrói o tipo do handler necessário (ex: IRequestHandler&lt;CriarMedicoCommand, Medico&gt;)
    /// 3. Pede para o container de DI resolver esse handler
    /// 4. Executa o método Handle() usando dynamic
    /// 5. Retorna a resposta
    /// </summary>
    public async Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
    {
        // 🔍 PASSO 1: Descobrir o tipo concreto da requisição
        // request pode ser CriarMedicoCommand, AtualizarPacienteCommand, etc.
        var requestType = request.GetType();

        Console.WriteLine($"📥 Processando requisição: {requestType.Name}");

        // 🔍 PASSO 2: Construir o tipo do handler necessário
        // Se requestType = CriarMedicoCommand e TResponse = Medico
        // Então handlerType = IRequestHandler<CriarMedicoCommand, Medico>
        var handlerType = typeof(IRequestHandler<,>).MakeGenericType(requestType, typeof(TResponse));

        Console.WriteLine($"🔍 Procurando handler: {handlerType.Name}");

        // 🔍 PASSO 3: Resolver o handler via Dependency Injection
        // O container procura uma classe que implementa IRequestHandler<TRequest, TResponse>
        var handler = serviceProvider.GetService(handlerType);

        if (handler == null)
        {
            throw new InvalidOperationException(
                $"❌ Nenhum handler encontrado para {requestType.Name}. " +
                $"Certifique-se de registrar uma classe que implementa {handlerType.Name}");
        }

        Console.WriteLine($"✅ Handler encontrado: {handler.GetType().Name}");

        // 🔍 PASSO 4: Executar o método Handle() dinamicamente
        // Usamos dynamic para evitar reflection complexa
        // É como fazer: ((IRequestHandler<TRequest, TResponse>)handler).Handle(request, cancellationToken)
        // Mas funciona para qualquer tipo de TRequest e TResponse
        try
        {
            var result = await ((dynamic)handler).Handle((dynamic)request, cancellationToken);

            Console.WriteLine($"🎉 Requisição processada com sucesso!");

            return result;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"💥 Erro ao processar requisição: {ex.Message}");
            throw;
        }
    }
}

/// <summary>
/// 🤔 POR QUE USAMOS DYNAMIC?
///
/// Sem dynamic, teríamos que fazer algo assim:
///
/// var method = handlerType.GetMethod("Handle");
/// var result = method.Invoke(handler, new object[] { request, cancellationToken });
/// return (TResponse)await (Task<TResponse>)result;
///
/// O dynamic é mais simples e legível, embora tenha um pequeno overhead.
/// Para a maioria das aplicações, a diferença é insignificante.
///
/// 🚀 ALTERNATIVAS SEM DYNAMIC:
/// Se quiser performance máxima, pode usar Expression Trees ou criar
/// um cache de delegates compilados. Mas isso adiciona complexidade.
/// </summary>