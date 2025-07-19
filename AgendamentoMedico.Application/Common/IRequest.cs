namespace AgendamentoMedico.Application.Common;

/// <summary>
/// Interface marker que representa uma requisição que retorna uma resposta do tipo TResponse
///
/// 🎯 CONCEITO: Esta é uma "marker interface" - não tem métodos, apenas marca que uma classe
/// é uma requisição válida para o Mediator. É o contrato base para Commands e Queries.
///
/// Exemplo de uso:
/// public record CriarMedicoCommand(string Nome, string CRM) : IRequest&lt;Medico&gt;;
///
/// O tipo TResponse define o que essa requisição vai retornar quando processada.
/// </summary>
/// <typeparam name="TResponse">Tipo da resposta que será retornada pelo handler</typeparam>
public interface IRequest<out TResponse>
{
    // Marker interface - não precisa de membros!
    // A presença desta interface é suficiente para identificar que é uma requisição
}