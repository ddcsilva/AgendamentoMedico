namespace AgendamentoMedico.Domain.Entities;

/// <summary>
/// Constantes para tipos de claims customizados do sistema médico
/// </summary>
public static class ClaimTypes
{
    /// <summary>
    /// Claims relacionados a permissões gerais do sistema
    /// </summary>
    public static class Permissoes
    {
        public const string GerenciarMedicos = "permissao.gerenciar_medicos";
        public const string GerenciarPacientes = "permissao.gerenciar_pacientes";
        public const string GerenciarConsultas = "permissao.gerenciar_consultas";
        public const string GerenciarUsuarios = "permissao.gerenciar_usuarios";
        public const string VisualizarRelatorios = "permissao.visualizar_relatorios";
        public const string GerenciarSistema = "permissao.gerenciar_sistema";
    }

    /// <summary>
    /// Claims relacionados a especialidades médicas
    /// </summary>
    public static class Especialidades
    {
        public const string TipoEspecialidade = "especialidade";

        // Especialidades médicas comuns
        public const string Cardiologia = "Cardiologia";
        public const string Dermatologia = "Dermatologia";
        public const string Ortopedia = "Ortopedia";
        public const string Pediatria = "Pediatria";
        public const string Psiquiatria = "Psiquiatria";
        public const string ClinicaGeral = "Clínica Geral";
        public const string Neurologia = "Neurologia";
        public const string Oftalmologia = "Oftalmologia";
        public const string Ginecologia = "Ginecologia";
        public const string Urologia = "Urologia";
    }

    /// <summary>
    /// Claims relacionados a roles/perfis do sistema
    /// </summary>
    public static class Roles
    {
        public const string TipoRole = "role";

        // Perfis de usuário no sistema
        public const string Administrador = "Admin";
        public const string Medico = "Medico";
        public const string Recepcionista = "Recepcionista";
        public const string Enfermeiro = "Enfermeiro";
        public const string Supervisor = "Supervisor";
    }

    /// <summary>
    /// Claims relacionados a departamentos/setores hospitalares
    /// </summary>
    public static class Departamentos
    {
        public const string TipoDepartamento = "departamento";

        // Departamentos do hospital/clínica
        public const string Emergencia = "Emergencia";
        public const string ConsultorioExterno = "ConsultorioExterno";
        public const string Internacao = "Internacao";
        public const string Administracao = "Administracao";
        public const string Laboratorio = "Laboratorio";
        public const string Farmacia = "Farmacia";
        public const string Radiologia = "Radiologia";
    }
}