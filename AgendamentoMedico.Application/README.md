# 🚀 Mediator Personalizado & CQRS

## 📋 **Índice**

1. [Introdução](#-introdução)
2. [Como Funciona Nosso Mediator](#-como-funciona-nosso-mediator)
3. [CQRS - Command Query Responsibility Segregation](#-cqrs---command-query-responsibility-segregation)
4. [Implementação Prática](#-implementação-prática)
5. [Comparação com MediatR](#-comparação-com-mediatr)
6. [Vantagens e Desvantagens](#-vantagens-e-desvantagens)
7. [Quando Usar](#-quando-usar)
8. [Exemplos Avançados](#-exemplos-avançados)

---

## 🎯 **Introdução**

Este projeto implementa um **Mediator Pattern personalizado** como alternativa ao MediatR, que recentemente se tornou comercial. Nossa implementação é:

- ✅ **100% gratuita** - nunca vira comercial
- ✅ **Zero dependências** externas
- ✅ **Auto-descoberta** de handlers
- ✅ **Performance otimizada**
- ✅ **Fácil de entender** e modificar
- ✅ **Suporte completo a CQRS**

---

## 🔧 **Como Funciona Nosso Mediator**

### **🏗️ Arquitetura**

```
┌─────────────────┐    ┌─────────────────┐    ┌─────────────────┐
│   Controller    │    │   IMediator     │    │   Handler       │
│                 │───▶│                 │───▶│                 │
│ Send(Command)   │    │ Send<TResponse> │    │ Handle(request) │
└─────────────────┘    └─────────────────┘    └─────────────────┘
```

### **🎭 Componentes Principais**

#### **1. IRequest<TResponse>**
```csharp
/// <summary>
/// Marker interface que identifica uma requisição
/// </summary>
public interface IRequest<out TResponse>
{
    // Marker interface - sem métodos!
    // A presença desta interface é suficiente
}
```

#### **2. IRequestHandler<TRequest, TResponse>**
```csharp
/// <summary>
/// Interface que define como processar uma requisição
/// </summary>
public interface IRequestHandler<in TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken);
}
```

#### **3. IMediator**
```csharp
/// <summary>
/// Interface central que despacha requisições para handlers
/// </summary>
public interface IMediator
{
    Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default);
}
```

### **⚡ Funcionamento Interno**

#### **Passo a Passo:**

```csharp
public async Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
{
    // 🔍 PASSO 1: Descobrir o tipo concreto
    var requestType = request.GetType(); // Ex: CriarMedicoCommand

    // 🔍 PASSO 2: Construir tipo do handler necessário
    var handlerType = typeof(IRequestHandler<,>).MakeGenericType(requestType, typeof(TResponse));
    // Resultado: IRequestHandler<CriarMedicoCommand, Medico>

    // 🔍 PASSO 3: Resolver via Dependency Injection
    var handler = serviceProvider.GetService(handlerType); // CriarMedicoHandler

    // 🔍 PASSO 4: Executar dinamicamente
    return await ((dynamic)handler).Handle((dynamic)request, cancellationToken);
}
```

#### **🤔 Por que Dynamic?**

```csharp
// ❌ SEM DYNAMIC (complexo):
var method = handlerType.GetMethod("Handle");
var result = method.Invoke(handler, new object[] { request, cancellationToken });
return (TResponse)await (Task<TResponse>)result;

// ✅ COM DYNAMIC (simples):
return await ((dynamic)handler).Handle((dynamic)request, cancellationToken);
```

### **🤖 Auto-Descoberta de Handlers**

```csharp
public static IServiceCollection AddSimpleMediator(this IServiceCollection services, params Assembly[] assemblies)
{
    // 1. Registrar o mediator
    services.AddScoped<IMediator, SimpleMediator>();

    // 2. Encontrar todos os handlers automaticamente
    var handlerInterface = typeof(IRequestHandler<,>);

    foreach (var assembly in assemblies)
    {
        var handlerTypes = assembly.GetTypes()
            .Where(type =>
                type.IsClass &&
                !type.IsAbstract &&
                type.GetInterfaces().Any(i =>
                    i.IsGenericType &&
                    i.GetGenericTypeDefinition() == handlerInterface))
            .ToList();

        // 3. Registrar cada handler encontrado
        foreach (var handlerType in handlerTypes)
        {
            var interfaces = handlerType.GetInterfaces()
                .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == handlerInterface);

            foreach (var @interface in interfaces)
            {
                services.AddScoped(@interface, handlerType);
            }
        }
    }
}
```

---

## 📋 **CQRS - Command Query Responsibility Segregation**

### **🎯 Conceito**

CQRS separa **operações de leitura** (Queries) das **operações de escrita** (Commands):

```
┌─────────────────────────────────────────────────────────────┐
│                        CQRS                                 │
├─────────────────────────────┬───────────────────────────────┤
│           COMMANDS          │           QUERIES             │
│        (Escrita)            │          (Leitura)            │
├─────────────────────────────┼───────────────────────────────┤
│ • Modificam estado          │ • NÃO modificam estado        │
│ • CREATE, UPDATE, DELETE    │ • SELECT (apenas leitura)     │
│ • Validações rigorosas      │ • Otimizadas para performance │
│ • Regras de negócio         │ • Podem usar cache            │
│ • Podem falhar              │ • Raramente falham            │
│ • Retornam entidade criada  │ • Retornam dados para UI      │
└─────────────────────────────┴───────────────────────────────┘
```

### **📝 Commands (Escrita)**

#### **Definição:**
```csharp
/// <summary>
/// Command representa uma INTENÇÃO de fazer algo que muda estado
/// </summary>
public record CriarMedicoCommand(
    string Nome,
    string CRM,
    string Especialidade
) : IRequest<Medico>  // 👈 Vai retornar o médico criado
{
    public void Validar()
    {
        if (string.IsNullOrWhiteSpace(Nome))
            throw new ArgumentException("Nome é obrigatório");

        if (string.IsNullOrWhiteSpace(CRM))
            throw new ArgumentException("CRM é obrigatório");
    }
}
```

#### **Handler:**
```csharp
/// <summary>
/// Handler processa o Command com foco em CONSISTÊNCIA
/// </summary>
public class CriarMedicoHandler(IMedicoRepository repository) : IRequestHandler<CriarMedicoCommand, Medico>
{
    public async Task<Medico> Handle(CriarMedicoCommand request, CancellationToken cancellationToken)
    {
        // ✅ 1. Validação rigorosa
        request.Validar();

        // ✅ 2. Regras de negócio
        var crmExiste = await repository.CrmJaExisteAsync(request.CRM, cancellationToken);
        if (crmExiste)
            throw new InvalidOperationException($"CRM {request.CRM} já existe");

        // ✅ 3. Criar entidade
        var medico = new Medico
        {
            Nome = request.Nome,
            CRM = request.CRM,
            Especialidade = request.Especialidade
        };

        // ✅ 4. Persistir
        await repository.AdicionarAsync(medico, cancellationToken);
        await repository.SalvarAlteracoesAsync(cancellationToken);

        return medico; // Retorna entidade criada
    }
}
```

### **🔍 Queries (Leitura)**

#### **Definição:**
```csharp
/// <summary>
/// Query representa uma CONSULTA que NÃO modifica dados
/// </summary>
public record ObterMedicoPorIdQuery(Guid MedicoId) : IRequest<Medico?>
{
    public void Validar()
    {
        if (MedicoId == Guid.Empty)
            throw new ArgumentException("ID inválido");
    }
}
```

#### **Handler:**
```csharp
/// <summary>
/// Handler processa Query com foco em PERFORMANCE
/// </summary>
public class ObterMedicoPorIdHandler(IMedicoRepository repository) : IRequestHandler<ObterMedicoPorIdQuery, Medico?>
{
    public async Task<Medico?> Handle(ObterMedicoPorIdQuery request, CancellationToken cancellationToken)
    {
        // ✅ 1. Validação mínima
        request.Validar();

        // ✅ 2. Busca otimizada (pode usar cache)
        return await repository.ObterPorIdAsync(request.MedicoId, cancellationToken);
    }
}
```

### **🏆 Benefícios do CQRS**

1. **Separação Clara**: Escrita vs Leitura bem definidas
2. **Otimização Específica**: Cada lado pode ser otimizado independentemente
3. **Escalabilidade**: Pode usar bancos diferentes para read/write
4. **Manutenibilidade**: Responsabilidades bem separadas
5. **Testabilidade**: Cada handler testa independentemente

---

## 🛠️ **Implementação Prática**

### **🔧 Configuração**

#### **1. Program.cs:**
```csharp
using AgendamentoMedico.Application.Common;

var builder = WebApplication.CreateBuilder(args);

// ✅ Registrar nosso Mediator personalizado
builder.Services.AddSimpleMediator(typeof(CriarMedicoHandler).Assembly);

var app = builder.Build();
```

#### **2. Controller:**
```csharp
[ApiController]
[Route("api/[controller]")]
public class MedicosController(IMediator mediator) : ControllerBase
{
    // 📝 COMMAND - Criar médico
    [HttpPost]
    public async Task<ActionResult<Medico>> Criar(CriarMedicoCommand command)
    {
        try
        {
            var medico = await mediator.Send(command);
            return CreatedAtAction(nameof(ObterPorId), new { id = medico.Id }, medico);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(ex.Message);
        }
    }

    // 🔍 QUERY - Buscar médico
    [HttpGet("{id}")]
    public async Task<ActionResult<Medico>> ObterPorId(Guid id)
    {
        var medico = await mediator.Send(new ObterMedicoPorIdQuery(id));
        return medico == null ? NotFound() : Ok(medico);
    }
}
```

#### **3. Minimal API:**
```csharp
// 📝 COMMAND
app.MapPost("/api/medicos", async (CriarMedicoCommand command, IMediator mediator) =>
{
    var medico = await mediator.Send(command);
    return Results.Created($"/api/medicos/{medico.Id}", medico);
});

// 🔍 QUERY
app.MapGet("/api/medicos/{id}", async (Guid id, IMediator mediator) =>
{
    var medico = await mediator.Send(new ObterMedicoPorIdQuery(id));
    return medico != null ? Results.Ok(medico) : Results.NotFound();
});
```

### **📊 Fluxo Completo**

```mermaid
graph TB
    A[Client] --> B[Controller]
    B --> C[Mediator.Send()]
    C --> D{Tipo?}
    D -->|Command| E[CriarMedicoHandler]
    D -->|Query| F[ObterMedicoPorIdHandler]
    E --> G[Validações + Regras]
    E --> H[Repository.Add()]
    E --> I[SaveChanges()]
    F --> J[Repository.GetById()]
    I --> K[Return Entity]
    J --> K
    K --> B
    B --> A
```

---

## ⚖️ **Comparação com MediatR**

| Aspecto | **Nosso Mediator** | **MediatR** |
|---------|-------------------|-------------|
| 💰 **Custo** | ✅ Sempre gratuito | ❌ Comercial para empresas |
| 📦 **Dependências** | ✅ Zero dependências | ❌ NuGet package |
| 🏗️ **Controle** | ✅ Código fonte total | ❌ Biblioteca externa |
| ⚡ **Performance** | ✅ Otimizado e leve | ⚠️ Overhead adicional |
| 📚 **Aprendizado** | ✅ Entende internamente | ❌ "Caixa preta" |
| 🔧 **Personalização** | ✅ 100% customizável | ❌ Limitado à API |
| 🧪 **Testes** | ✅ Fácil de mockar | ✅ Fácil de mockar |
| 📖 **Documentação** | ⚠️ Nossa documentação | ✅ Extensa documentação |
| 🌟 **Features** | ⚠️ Básico (extensível) | ✅ Pipeline Behaviors, Notifications |

### **🎯 Funcionalidades**

#### **✅ O que nosso Mediator TEM:**
- ✅ Commands e Queries
- ✅ Auto-descoberta de handlers
- ✅ Dependency Injection
- ✅ Async/await support
- ✅ Generic constraints
- ✅ Type safety
- ✅ Performance otimizada

#### **⚠️ O que nosso Mediator NÃO TEM (ainda):**
- ⚠️ Pipeline Behaviors (logging, validação, cache)
- ⚠️ Notifications/Events
- ⚠️ Stream requests
- ⚠️ Validation pipeline
- ⚠️ Caching automático

#### **🚀 Mas podemos ADICIONAR facilmente:**

```csharp
// Exemplo: Pipeline Behavior para logging
public interface IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken);
}

public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        Console.WriteLine($"Processando: {typeof(TRequest).Name}");
        var result = await next();
        Console.WriteLine($"Concluído: {typeof(TRequest).Name}");
        return result;
    }
}
```

---

## ✅ **Vantagens e Desvantagens**

### **✅ Vantagens**

#### **🆓 Econômicas:**
- **Zero custo** - nunca vira paga
- **Sem licenças** comerciais
- **Sem vendor lock-in**

#### **🏗️ Técnicas:**
- **Controle total** do código
- **Performance otimizada**
- **Zero dependências externas**
- **Fácil de debugar**
- **Customização ilimitada**

#### **📚 Educacionais:**
- **Entendimento profundo** dos patterns
- **Aprendizado valioso**
- **Demonstra competência técnica**

#### **🔧 Operacionais:**
- **Manutenção simplificada**
- **Deploy mais leve**
- **Menos pontos de falha**

### **❌ Desvantagens**

#### **⏰ Tempo:**
- **Desenvolvimento inicial** mais longo
- **Manutenção** é nossa responsabilidade
- **Features avançadas** precisam ser implementadas

#### **📖 Documentação:**
- **Menos documentação** disponível online
- **Menos exemplos** da comunidade
- **Team onboarding** pode ser mais lento

#### **🌟 Features:**
- **Menos features** out-of-the-box
- **Pipeline behaviors** precisam ser implementados
- **Ecossistema menor**

---

## 🎯 **Quando Usar**

### **✅ Use nosso Mediator quando:**

1. **💰 Orçamento limitado** - não quer pagar licenças
2. **🏗️ Controle necessário** - precisa customizar profundamente
3. **📚 Time sênior** - equipe consegue manter código customizado
4. **⚡ Performance crítica** - cada milissegundo importa
5. **🎓 Aprendizado** - quer entender patterns profundamente
6. **🔒 Segurança** - não quer bibliotecas externas
7. **📦 Deploy simples** - menos dependências

### **❌ Use MediatR quando:**

1. **⏰ Time to market** - precisa entregar rápido
2. **👥 Time júnior** - equipe não tem experiência com patterns
3. **🌟 Features avançadas** - precisa de pipeline behaviors complexos
4. **📖 Documentação** - precisa de referências extensas
5. **🤝 Suporte comercial** - precisa de suporte oficial
6. **🔄 Migração** - já usa MediatR e funciona bem

---

## 🚀 **Exemplos Avançados**

### **📋 Lista com Paginação**

```csharp
// Query
public record ListarMedicosQuery(
    int Pagina = 1,
    int TamanhoPagina = 10,
    string? Especialidade = null,
    string? Nome = null
) : IRequest<PagedResult<MedicoDto>>;

// DTO otimizado para listagem
public record MedicoDto(
    Guid Id,
    string Nome,
    string CRM,
    string Especialidade
);

// Result com paginação
public record PagedResult<T>(
    IEnumerable<T> Items,
    int TotalItems,
    int Pagina,
    int TamanhoPagina
);

// Handler otimizado
public class ListarMedicosHandler(IMedicoRepository repository)
    : IRequestHandler<ListarMedicosQuery, PagedResult<MedicoDto>>
{
    public async Task<PagedResult<MedicoDto>> Handle(ListarMedicosQuery request, CancellationToken cancellationToken)
    {
        var query = repository.Query();

        // Filtros opcionais
        if (!string.IsNullOrEmpty(request.Especialidade))
            query = query.Where(m => m.Especialidade == request.Especialidade);

        if (!string.IsNullOrEmpty(request.Nome))
            query = query.Where(m => m.Nome.Contains(request.Nome));

        var totalItems = await query.CountAsync(cancellationToken);

        var items = await query
            .Skip((request.Pagina - 1) * request.TamanhoPagina)
            .Take(request.TamanhoPagina)
            .Select(m => new MedicoDto(m.Id, m.Nome, m.CRM, m.Especialidade))
            .ToListAsync(cancellationToken);

        return new PagedResult<MedicoDto>(items, totalItems, request.Pagina, request.TamanhoPagina);
    }
}
```

### **🔄 Command com Atualização**

```csharp
// Command
public record AtualizarMedicoCommand(
    Guid Id,
    string Nome,
    string Especialidade
) : IRequest<Medico>;

// Handler com validações
public class AtualizarMedicoHandler(IMedicoRepository repository)
    : IRequestHandler<AtualizarMedicoCommand, Medico>
{
    public async Task<Medico> Handle(AtualizarMedicoCommand request, CancellationToken cancellationToken)
    {
        // Buscar existente
        var medico = await repository.ObterPorIdAsync(request.Id, cancellationToken);
        if (medico == null)
            throw new NotFoundException($"Médico {request.Id} não encontrado");

        // Atualizar propriedades
        medico.Nome = request.Nome;
        medico.Especialidade = request.Especialidade;
        medico.DefinirAtualizador("Sistema"); // Auditoria

        // Salvar
        repository.Atualizar(medico);
        await repository.SalvarAlteracoesAsync(cancellationToken);

        return medico;
    }
}
```

### **🗑️ Command com Deleção**

```csharp
// Command
public record ExcluirMedicoCommand(Guid Id) : IRequest<bool>;

// Handler
public class ExcluirMedicoHandler(IMedicoRepository repository, IConsultaRepository consultaRepository)
    : IRequestHandler<ExcluirMedicoCommand, bool>
{
    public async Task<bool> Handle(ExcluirMedicoCommand request, CancellationToken cancellationToken)
    {
        // Verificar se existe
        var medico = await repository.ObterPorIdAsync(request.Id, cancellationToken);
        if (medico == null)
            return false; // Já foi excluído

        // Regra de negócio: não pode excluir se tem consultas
        var temConsultas = await consultaRepository.MedicoTemConsultasAsync(request.Id, cancellationToken);
        if (temConsultas)
            throw new InvalidOperationException("Não é possível excluir médico com consultas agendadas");

        // Excluir
        repository.Remover(medico);
        await repository.SalvarAlteracoesAsync(cancellationToken);

        return true;
    }
}
```

### **🔍 Query com Agregação**

```csharp
// Query
public record ObterEstatisticasMedicosQuery() : IRequest<EstatisticasMedicosDto>;

// DTO
public record EstatisticasMedicosDto(
    int TotalMedicos,
    Dictionary<string, int> MedicosPorEspecialidade,
    DateTime UltimoCadastro
);

// Handler
public class ObterEstatisticasMedicosHandler(IMedicoRepository repository)
    : IRequestHandler<ObterEstatisticasMedicosQuery, EstatisticasMedicosDto>
{
    public async Task<EstatisticasMedicosDto> Handle(ObterEstatisticasMedicosQuery request, CancellationToken cancellationToken)
    {
        var medicos = await repository.ObterTodosAsync(cancellationToken);

        var total = medicos.Count();

        var porEspecialidade = medicos
            .GroupBy(m => m.Especialidade)
            .ToDictionary(g => g.Key, g => g.Count());

        var ultimoCadastro = medicos.Any()
            ? medicos.Max(m => m.CriadoEm)
            : DateTime.MinValue;

        return new EstatisticasMedicosDto(total, porEspecialidade, ultimoCadastro);
    }
}
```

---

## 🎉 **Conclusão**

Nosso **Mediator personalizado** é uma solução:

- 🆓 **Econômica** - zero custo
- 🏗️ **Flexível** - 100% customizável
- ⚡ **Performática** - otimizada
- 📚 **Educativa** - aprendizado valioso
- 🔧 **Prática** - funciona perfeitamente

É uma **excelente alternativa** ao MediatR, especialmente para:
- 💰 Projetos com orçamento limitado
- 🎓 Times que querem aprender
- 🏗️ Aplicações que precisam de controle total
- ⚡ Sistemas com requisitos de performance

**Resultado**: Uma implementação robusta, eficiente e **completamente nossa**! 🚀

---

## 📞 **Suporte**

Para dúvidas sobre esta implementação:

1. 📖 Consulte esta documentação
2. 🔍 Analise o código fonte nos namespaces:
   - `AgendamentoMedico.Application.Common`
   - `AgendamentoMedico.Application.Features`
3. 🧪 Execute os testes na Controller `MedicoController`
4. 📋 Consulte os logs detalhados da aplicação

**Happy Coding!** 👨‍💻👩‍💻