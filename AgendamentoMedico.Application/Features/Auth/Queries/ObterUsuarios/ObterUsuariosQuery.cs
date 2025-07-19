using AgendamentoMedico.Application.Common;
using AgendamentoMedico.Application.Features.Auth.Dtos;

namespace AgendamentoMedico.Application.Features.Auth.Queries.ObterUsuarios;

/// <summary>
/// Query para obter lista de usuários com filtros
/// </summary>
public record ObterUsuariosQuery(
    string? Nome = null,
    string? Email = null,
    bool? Ativo = null,
    int Pagina = 1,
    int TamanhoPagina = 10
) : IRequest<ListaUsuariosResult>
{
    /// <summary>
    /// Valida os dados da query
    /// </summary>
    public void Validar()
    {
        if (Pagina < 1)
            throw new ArgumentException("Página deve ser maior que zero", nameof(Pagina));

        if (TamanhoPagina < 1 || TamanhoPagina > 100)
            throw new ArgumentException("Tamanho da página deve estar entre 1 e 100", nameof(TamanhoPagina));
    }
}