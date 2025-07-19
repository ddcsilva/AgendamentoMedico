using AgendamentoMedico.Application.Common;
using AgendamentoMedico.Application.Features.Auth.Commands.Login;
using AgendamentoMedico.Application.Features.Auth.Commands.Logout;
using AgendamentoMedico.Application.Features.Auth.Commands.RegistrarUsuario;
using AgendamentoMedico.Presentation.Models.Account;

namespace AgendamentoMedico.Presentation.Controllers;

public class AccountController(IMediator mediator, ILogger<AccountController> logger) : Controller
{
    private readonly IMediator _mediator = mediator;
    private readonly ILogger<AccountController> _logger = logger;

    [HttpGet]
    public IActionResult Login(string? returnUrl = null)
    {
        if (User.Identity?.IsAuthenticated == true)
        {
            return RedirectToAction("Index", "Home");
        }

        ViewData["ReturnUrl"] = returnUrl;
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        try
        {
            var command = new LoginCommand(model.Email, model.Password);
            var result = await _mediator.Send(command);

            _logger.LogInformation("Usuário {Email} logado com sucesso", model.Email);

            TempData["AuthToken"] = result.Token;
            TempData["UserName"] = result.Nome;

            return RedirectToLocal(returnUrl);
        }
        catch (UnauthorizedAccessException ex)
        {
            _logger.LogWarning("Tentativa de login falhada para {Email}: {Message}", model.Email, ex.Message);
            ModelState.AddModelError(string.Empty, "Email ou senha inválidos.");
            return View(model);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro durante login para {Email}", model.Email);
            ModelState.AddModelError(string.Empty, "Erro interno. Tente novamente.");
            return View(model);
        }
    }

    [HttpGet]
    public IActionResult Register()
    {
        if (User.Identity?.IsAuthenticated == true)
        {
            return RedirectToAction("Index", "Home");
        }

        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        try
        {
            var claims = new List<string>();

            if (!string.IsNullOrEmpty(model.Role))
            {
                claims.Add(model.Role);
            }

            var command = new RegistrarUsuarioCommand(
                model.Nome,
                model.Email,
                model.Password,
                claims
            );

            var usuario = await _mediator.Send(command);

            _logger.LogInformation("Usuário {Email} registrado com sucesso", model.Email);

            TempData["SuccessMessage"] = "Usuário registrado com sucesso! Faça login para continuar.";
            return RedirectToAction(nameof(Login));
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning("Erro ao registrar usuário {Email}: {Message}", model.Email, ex.Message);
            ModelState.AddModelError(string.Empty, ex.Message);
            return View(model);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro durante registro para {Email}", model.Email);
            ModelState.AddModelError(string.Empty, "Erro interno. Tente novamente.");
            return View(model);
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        try
        {
            var userId = User.FindFirst("sub")?.Value;
            if (Guid.TryParse(userId, out var guidUserId))
            {
                var command = new LogoutCommand(guidUserId);
                await _mediator.Send(command);
            }

            _logger.LogInformation("Usuário fez logout");

            TempData["LogoutMessage"] = "Logout realizado com sucesso.";
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro durante logout");
        }

        return RedirectToAction("Index", "Home");
    }

    [HttpGet]
    public IActionResult AccessDenied()
    {
        return View();
    }

    private IActionResult RedirectToLocal(string? returnUrl)
    {
        if (Url.IsLocalUrl(returnUrl))
        {
            return Redirect(returnUrl);
        }

        return RedirectToAction("Index", "Home");
    }
}