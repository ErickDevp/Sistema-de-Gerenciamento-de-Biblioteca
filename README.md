# 📚 Biblioteca — Sistema de Gerenciamento de Biblioteca

Sistema web desenvolvido em **C# ASP.NET MVC** para gerenciamento interno de biblioteca, com controle de acervo, usuários, categorias e empréstimos.

---

## 🚀 Tecnologias Utilizadas

| Tecnologia | Versão |
|---|---|
| .NET | 9.0 |
| ASP.NET MVC | 9.0 |
| Entity Framework Core | 9.0 |
| SQL Server / LocalDB | — |
| BCrypt.Net-Next | 4.0.3 |
| Razor Views | — |
| HTML + CSS puro | — |

---

## 📁 Estrutura do Projeto

```
Biblioteca/
│
├── Controllers/
│   ├── BaseController.cs          # Verificação de sessão (herança)
│   ├── HomeController.cs          # Dashboard com estatísticas
│   ├── AuthController.cs          # Login / Logout
│   ├── UsuarioController.cs       # CRUD de usuários (Admin only)
│   ├── LivroController.cs         # CRUD de livros + busca
│   ├── CategoriaController.cs     # CRUD de categorias
│   └── EmprestimoController.cs    # Empréstimos e devoluções
│
├── Models/
│   ├── Usuario.cs
│   ├── Livro.cs
│   ├── Categoria.cs
│   └── Emprestimo.cs
│
├── Data/
│   ├── BibliotecaContext.cs       # DbContext EF Core
│   └── DbSeeder.cs                # Dados iniciais (seed)
│
├── ViewModels/
│   ├── LoginViewModel.cs
│   ├── LivroViewModel.cs
│   └── EmprestimoViewModel.cs
│
├── Services/
│   ├── AuthService.cs
│   └── UsuarioService.cs
│
├── Views/
│   ├── Auth/Login.cshtml
│   ├── Home/Index.cshtml
│   ├── Livro/
│   ├── Categoria/
│   ├── Emprestimo/
│   ├── Usuario/
│   └── Shared/_Layout.cshtml
│
├── Properties/
│   └── launchSettings.json        # Garante ambiente Development local
│
├── wwwroot/
│   ├── css/site.css
│   ├── css/login.css
│   └── js/site.js
│
├── Migrations/
├── appsettings.json
├── Program.cs
└── Biblioteca.csproj
```

---

## ⚙️ Como Executar

### Pré-requisitos

- [.NET SDK](https://dotnet.microsoft.com/download) (versão 8 ou 9)
- SQL Server ou SQL Server Express / LocalDB
- Visual Studio 2022 ou VS Code

### Passo a Passo

```bash
# 1. Ajuste a connection string em appsettings.json se necessário
# 2. Restaure os pacotes
dotnet restore

# 3. Execute (banco criado automaticamente)
dotnet run
```

Acesse: `http://localhost:5000`

---

## 🔑 Credenciais Padrão (Seed)

| Usuário | E-mail | Senha | Cargo |
|---|---|---|---|
| Administrador | admin@biblioteca.com | Admin@123 | Administrador |
| Funcionário Padrão | funcionario@biblioteca.com | Func@123 | Funcionário |

---

## 👥 Controle de Acesso por Cargo

| Funcionalidade | Administrador | Funcionário |
|---|:---:|:---:|
| Dashboard | ✅ | ✅ |
| Gerenciar Livros | ✅ | ✅ |
| Gerenciar Categorias | ✅ | ✅ |
| Registrar Empréstimos | ✅ | ✅ |
| Registrar Devoluções | ✅ | ✅ |
| Gerenciar Usuários | ✅ | ❌ |

---

## 📋 Regras de Negócio Implementadas

| Regra | Descrição |
|---|---|
| RN01 | Apenas usuários autenticados acessam o sistema |
| RN02 | Apenas Administradores gerenciam usuários |
| RN03 | Livro só pode ser emprestado se estiver disponível |
| RN04 | Livro emprestado não pode ser re-emprestado |
| RN05 | Devolução retorna o livro ao status "Disponível" |
| RN06 | Data prevista de devolução definida no empréstimo |
| RN07 | Empréstimos com data vencida são marcados "Atrasado" |
| RN08 | Empréstimo marca livro como indisponível automaticamente |

---

## 🗄️ Modelo de Dados

```
Usuario
  Id | Nome | Email | SenhaHash | Role

Categoria
  Id | Nome

Livro
  Id | Titulo | Autor | AnoPublicacao | CategoriaId (FK) | Disponivel

Emprestimo
  Id | LivroId (FK) | NomeLeitor | DataEmprestimo
     | DataPrevistaDevolucao | DataDevolucao | Status
```

### Relacionamentos

```
Categoria (1) ──── (N) Livro
Livro     (1) ──── (N) Emprestimo
```

---

## 🔧 Correção Aplicada — Rotas Internas

Se o sistema redirecionava de volta para o Login (ou dava erro) ao acessar `/Usuario`, `/Livro` ou `/Categoria`, o motivo era o middleware `UseHttpsRedirection()` sendo executado **antes** de `UseSession()`, em ambiente `Production` sem certificado HTTPS configurado — isso invalidava o cookie de sessão a cada requisição.

Correção aplicada em `Program.cs`:
- `UseHttpsRedirection()` agora só roda em ambiente não-Development (produção real, com HTTPS configurado)
- Adicionado `Properties/launchSettings.json` para forçar `ASPNETCORE_ENVIRONMENT=Development` ao rodar localmente via `dotnet run`

---

## 🧪 Testando o Sistema

1. Faça login como **Administrador** → acesso total
2. Cadastre uma nova **Categoria**
3. Cadastre um **Livro** nessa categoria
4. Registre um **Empréstimo** para esse livro
5. Verifique que o livro ficou **indisponível**
6. Registre a **Devolução** → livro volta a ficar disponível
7. Faça login como **Funcionário** → acesso a `/Usuario` deve ser bloqueado

---

*Biblioteca — Sistema de Gerenciamento de Biblioteca em ASP.NET MVC*
