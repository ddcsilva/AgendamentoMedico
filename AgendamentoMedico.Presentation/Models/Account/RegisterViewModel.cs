using System.ComponentModel.DataAnnotations;

namespace AgendamentoMedico.Presentation.Models.Account;

/// <summary>
/// ViewModel para o formulário de registro de usuário
/// </summary>
public class RegisterViewModel
{
    [Required(ErrorMessage = "Nome é obrigatório")]
    [StringLength(200, ErrorMessage = "Nome deve ter no máximo 200 caracteres")]
    [Display(Name = "Nome completo")]
    public string Nome { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email é obrigatório")]
    [EmailAddress(ErrorMessage = "Email inválido")]
    [Display(Name = "Email")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Senha é obrigatória")]
    [StringLength(100, ErrorMessage = "A senha deve ter pelo menos {2} e no máximo {1} caracteres.", MinimumLength = 6)]
    [DataType(DataType.Password)]
    [Display(Name = "Senha")]
    public string Password { get; set; } = string.Empty;

    [DataType(DataType.Password)]
    [Display(Name = "Confirmar senha")]
    [Compare("Password", ErrorMessage = "A senha e a confirmação não coincidem.")]
    public string ConfirmPassword { get; set; } = string.Empty;

    [Display(Name = "Perfil")]
    public string? Role { get; set; }
}