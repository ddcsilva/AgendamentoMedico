using AgendamentoMedico.Application.Common;
using AgendamentoMedico.Application.Features.Auth.Dtos;
using AgendamentoMedico.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AgendamentoMedico.Application.Features.Auth.Queries.ObterUsuarios;

/// <summary>
/// Handler para obter lista de usuários
/// </summary>
public class ObterUsuariosHandler(
    UserManager<Usuario> userManager
) : IRequestHandler<ObterUsuariosQuery, ListaUsuariosResult>
{
    public async Task<ListaUsuariosResult> Handle(ObterUsuariosQuery request, CancellationToken cancellationToken)
    {
        // Validar dados
        request.Validar();

        // Começar com todos os usuários
        var query = userManager.Users.AsQueryable();

        // Aplicar filtros
        if (!string.IsNullOrWhiteSpace(request.Nome))
        {
            query = query.Where(u => u.Nome.Contains(request.Nome));
        }

        if (!string.IsNullOrWhiteSpace(request.Email))
        {
            query = query.Where(u => u.Email!.Contains(request.Email));
        }

        if (request.Ativo.HasValue)
        {
            query = query.Where(u => u.Ativo == request.Ativo.Value);
        }

        // Contar total
        var total = await query.CountAsync(cancellationToken);

        // Aplicar paginação
        var skip = (request.Pagina - 1) * request.TamanhoPagina;
        var usuarios = await query
            .OrderBy(u => u.Nome)
            .Skip(skip)
            .Take(request.TamanhoPagina)
            .ToListAsync(cancellationToken);

        // Mapear para DTOs (sem claims para performance na listagem)
        var usuariosDto = usuarios.Select(u => new UsuarioDto(
            u.Id,
            u.Nome,
            u.Email!,
            u.Ativo,
            u.CriadoEm,
            u.UltimoLogin,
            new List<string>() // Claims vazios na listagem por performance
        )).ToList();

        // Calcular total de páginas
        var totalPaginas = (int)Math.Ceiling((double)total / request.TamanhoPagina);

        return new ListaUsuariosResult(
            usuariosDto,
            total,
            request.Pagina,
            request.TamanhoPagina,
            totalPaginas
        );
    }
}