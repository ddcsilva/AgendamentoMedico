using AgendamentoMedico.Infrastructure;
using AgendamentoMedico.Application.Common;
using AgendamentoMedico.Presentation.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Configuração dos serviços
builder.Services.AddControllersWithViews();

// Adicionar Mediator personalizado
builder.Services.AddSimpleMediator(typeof(AgendamentoMedico.Application.Features.Auth.Commands.Login.LoginHandler).Assembly);

// Adicionar infraestrutura (Identity + JWT + EF)
builder.Services.AddInfrastructure(builder.Configuration);

// Configurar redirecionamentos de autenticação
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.LogoutPath = "/Account/Logout";
    options.AccessDeniedPath = "/Account/AccessDenied";
    options.ReturnUrlParameter = "returnUrl";
});

var app = builder.Build();

// Configuração do pipeline de requisições
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

// Pipeline de autenticação
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Executa migrations automaticamente no desenvolvimento
await app.UseDevelopmentMigrationsAsync();

app.Run();
