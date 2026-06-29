# 📚 BibliotecaMVC — Sistema de Gerenciamento de Biblioteca

Sistema web desenvolvido em **C# ASP.NET MVC (.NET 8)** para gerenciamento interno de biblioteca, com controle de acervo, usuários, categorias e empréstimos.

---

## 🚀 Tecnologias Utilizadas

| Tecnologia | Versão |
|---|---|
| .NET | 8.0 |
| ASP.NET MVC | 8.0 |
| Entity Framework Core | 8.0 |
| SQL Server / LocalDB | — |
| BCrypt.Net-Next | 4.0.3 |
| Razor Views | — |
| HTML + CSS puro | — |

---

## 📁 Estrutura do Projeto

```
BibliotecaMVC/
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
│   ├── LivroViewModel.cs          # Inclui filtros de busca
│   └── EmprestimoViewModel.cs
│
├── Services/
│   ├── AuthService.cs             # Autenticação via Session
│   └── UsuarioService.cs          # Lógica de negócio de usuários
│
├── Views/
│   ├── Auth/Login.cshtml
│   ├── Home/Index.cshtml          # Dashboard
│   ├── Livro/                     # Index, Create, Edit, Delete, Details
│   ├── Categoria/                 # Index, Create, Edit, Delete, Details
│   ├── Emprestimo/                # Index, Create, Devolver, Details
│   ├── Usuario/                   # Index, Create, Edit, Delete, Details
│   └── Shared/_Layout.cshtml      # Layout com sidebar
│
├── wwwroot/
│   ├── css/site.css               # Estilos globais
│   ├── css/login.css              # Estilos da tela de login
│   └── js/site.js                 # Scripts auxiliares
│
├── Migrations/                    # Migration inicial gerada
├── appsettings.json
├── Program.cs
└── BibliotecaMVC.csproj
```

---

## ⚙️ Como Executar

### Pré-requisitos

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8)
- SQL Server ou SQL Server Express / LocalDB
- Visual Studio 2022 ou VS Code

### Passo a Passo

**1. Clone ou extraia o projeto**

```bash
cd BibliotecaMVC
```

**2. Configure a string de conexão**

Edite o arquivo `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=BibliotecaMVC;Trusted_Connection=True;"
  }
}
```

> Para SQL Server Express, use: `Server=.\\SQLEXPRESS;Database=BibliotecaMVC;Trusted_Connection=True;`

**3. Restaure os pacotes NuGet**

```bash
dotnet restore
```

**4. Execute as migrations (banco será criado automaticamente)**

O banco é criado automaticamente via `EnsureCreated()` no `DbSeeder` ao iniciar a aplicação.

Opcionalmente, para usar migrations:

```bash
dotnet ef database update
```

**5. Execute o projeto**

```bash
dotnet run
```

Acesse: `https://localhost:5001` ou `http://localhost:5000`

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

## 🔌 Endpoints Disponíveis

### Auth
| Método | Rota | Descrição |
|---|---|---|
| GET | /Auth/Login | Tela de login |
| POST | /Auth/Login | Autenticar |
| POST | /Auth/Logout | Encerrar sessão |

### Livro
| Método | Rota | Descrição |
|---|---|---|
| GET | /Livro | Listar (com busca) |
| GET | /Livro/Create | Formulário |
| POST | /Livro/Create | Cadastrar |
| GET | /Livro/Edit/{id} | Formulário edição |
| POST | /Livro/Edit/{id} | Atualizar |
| GET | /Livro/Delete/{id} | Confirmar exclusão |
| POST | /Livro/Delete/{id} | Excluir |
| GET | /Livro/Details/{id} | Detalhes |

### Categoria
| Método | Rota | Descrição |
|---|---|---|
| GET | /Categoria | Listar |
| GET/POST | /Categoria/Create | Cadastrar |
| GET/POST | /Categoria/Edit/{id} | Editar |
| GET/POST | /Categoria/Delete/{id} | Excluir |
| GET | /Categoria/Details/{id} | Detalhes |

### Emprestimo
| Método | Rota | Descrição |
|---|---|---|
| GET | /Emprestimo | Listar todos |
| GET | /Emprestimo/Create | Formulário |
| POST | /Emprestimo/Create | Registrar empréstimo |
| GET | /Emprestimo/Devolver/{id} | Tela de devolução |
| POST | /Emprestimo/Devolver/{id} | Registrar devolução |
| GET | /Emprestimo/Details/{id} | Detalhes |

### Usuario (Admin only)
| Método | Rota | Descrição |
|---|---|---|
| GET | /Usuario | Listar |
| GET/POST | /Usuario/Create | Cadastrar |
| GET/POST | /Usuario/Edit/{id} | Editar |
| GET/POST | /Usuario/Delete/{id} | Excluir |
| GET | /Usuario/Details/{id} | Detalhes |

---

## 💡 Funcionalidades Destacadas

- **Dashboard** com estatísticas em tempo real (livros, empréstimos, atrasos)
- **Busca de livros** por título, autor e categoria
- **Detecção automática de atrasos** ao acessar a lista de empréstimos
- **Seed automático** de dados iniciais na primeira execução
- **Senhas criptografadas** com BCrypt
- **Proteção CSRF** com AntiForgeryToken em todos os formulários
- **Controle de sessão** via ASP.NET Session
- **Interface responsiva** com sidebar e layout moderno
- **Validação server-side** com Data Annotations + jQuery Validate client-side

---

## 📦 Pacotes NuGet

```xml
<PackageReference Include="Microsoft.EntityFrameworkCore.SqlServer" Version="8.0.0" />
<PackageReference Include="Microsoft.EntityFrameworkCore.Tools" Version="8.0.0" />
<PackageReference Include="Microsoft.EntityFrameworkCore.Design" Version="8.0.0" />
<PackageReference Include="BCrypt.Net-Next" Version="4.0.3" />
```

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

*BibliotecaMVC — Projeto acadêmico de Sistema de Gerenciamento de Biblioteca em ASP.NET MVC*
