namespace AgendamentoMedico.Application.Common;

/// <summary>
/// Interface marker que representa uma requisição que retorna uma resposta do tipo TResponse
/// </summary>
/// <typeparam name="TResponse">Tipo da resposta que será retornada pelo handler</typeparam>
public interface IRequest<out TResponse>
{
}