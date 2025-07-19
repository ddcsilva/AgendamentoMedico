using System.ComponentModel.DataAnnotations;

namespace AgendamentoMedico.Presentation.Models.Account;

/// <summary>
/// ViewModel para o formulário de login
/// </summary>
public class LoginViewModel
{
    [Required(ErrorMessage = "Email é obrigatório")]
    [EmailAddress(ErrorMessage = "Email inválido")]
    [Display(Name = "Email")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Senha é obrigatória")]
    [DataType(DataType.Password)]
    [Display(Name = "Senha")]
    public string Password { get; set; } = string.Empty;

    [Display(Name = "Lembrar de mim")]
    public bool RememberMe { get; set; } = false;
}