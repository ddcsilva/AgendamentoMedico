namespace AgendamentoMedico.Application.Features.Auth.Dtos;

/// <summary>
/// Resultado da listagem de usuários
/// </summary>
public record ListaUsuariosResult(
    IEnumerable<UsuarioDto> Usuarios,
    int Total,
    int Pagina,
    int TamanhoPagina,
    int TotalPaginas
);