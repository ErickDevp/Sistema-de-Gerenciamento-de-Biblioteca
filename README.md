# 📚 Biblioteca — Sistema de Gerenciamento de Biblioteca

<div align="center">

![ASP.NET Core](https://img.shields.io/badge/ASP.NET_Core-9.0-512BD4?style=for-the-badge&logo=dotnet)
![C#](https://img.shields.io/badge/C%23-13.0-239120?style=for-the-badge&logo=csharp)
![Entity Framework](https://img.shields.io/badge/Entity_Framework-9.0-512BD4?style=for-the-badge)
![SQL Server](https://img.shields.io/badge/SQL_Server-LocalDB-CC2927?style=for-the-badge&logo=microsoftsqlserver)

**Sistema web completo para gerenciamento interno de bibliotecas**

</div>

---

## 📋 Sobre o Projeto

O **Biblioteca** é um sistema web desenvolvido em **C# com ASP.NET MVC** para gerenciamento interno de uma biblioteca. Permite controle completo do acervo, leitores, empréstimos e exemplares físicos, com autenticação, controle de acesso por cargo e interface moderna e responsiva.

Projeto desenvolvido como trabalho acadêmico para a disciplina de **Programação WEB II** — Instituto Federal de Sergipe (IFS).

---

## ✨ Funcionalidades

### 🔐 Autenticação e Acesso
- Login e logout com controle de sessão
- Controle de acesso por **cargo** (Administrador e Funcionário)
- **Foto de perfil** personalizada por URL
- Sessão com timeout configurável

### 📖 Gerenciamento de Livros
- Cadastro completo com título, autor, ano e categoria
- **Capa do livro** via URL com pré-visualização em tempo real
- Busca por título, autor e filtro por categoria
- Visualização em **modo grid (cards)** ou **modo lista**
- Paginação (12 por página)

### 📦 Controle de Exemplares
- Cada livro pode ter **múltiplos exemplares físicos**
- Geração automática de **número de tombo** (ex: `DOM-001`, `DOM-002`)
- Controle de situação: Bom estado / Danificado / Perdido
- Adicionar exemplares avulsos a qualquer momento
- Disponibilidade controlada por exemplar (não por título)

### 🏷️ Categorias
- Cadastro e edição de categorias
- Associação de livros por categoria
- Proteção contra exclusão de categorias com livros vinculados

### 👤 Leitores
- Cadastro completo com **CPF, e-mail, telefone e endereço**
- Máscara automática de CPF e telefone
- Status ativo/inativo
- Histórico de empréstimos por leitor
- Busca por nome, CPF ou e-mail

### 📋 Empréstimos
- Registro com seleção de leitor cadastrado e livro disponível
- **Seleção automática** do primeiro exemplar disponível (menor tombo)
- Filtros por status: Todos / Ativos / Devolvidos / Atrasados
- Detecção automática de empréstimos **atrasados**
- Registro de devolução com restauração automática do exemplar
- Histórico completo com datas e tombos

### 👥 Usuários do Sistema
- Cadastro de administradores e funcionários
- Foto de perfil exibida no topbar e na lista
- Permissões diferenciadas por cargo

### 📊 Dashboard
- Estatísticas em tempo real: títulos, exemplares, disponíveis, emprestados
- Alerta de empréstimos atrasados
- Tabela dos últimos 5 empréstimos registrados

---

## 🗂️ Estrutura do Projeto

```
Biblioteca/
│
├── Controllers/
│   ├── BaseController.cs          # Verificação de sessão (herança)
│   ├── HomeController.cs          # Dashboard
│   ├── AuthController.cs          # Login / Logout
│   ├── UsuarioController.cs       # CRUD de usuários (Admin)
│   ├── LivroController.cs         # CRUD de livros + exemplares
│   ├── CategoriaController.cs     # CRUD de categorias
│   ├── EmprestimoController.cs    # Empréstimos e devoluções
│   └── LeitorController.cs        # CRUD de leitores
│
├── Models/
│   ├── Usuario.cs
│   ├── Livro.cs
│   ├── Categoria.cs
│   ├── Exemplar.cs
│   ├── Emprestimo.cs
│   └── Leitor.cs
│
├── Data/
│   ├── BibliotecaContext.cs       # DbContext EF Core
│   └── DbSeeder.cs                # Dados iniciais automáticos
│
├── ViewModels/
│   ├── LoginViewModel.cs
│   ├── LivroViewModel.cs
│   ├── EmprestimoViewModel.cs
│   └── PaginatedList.cs           # Paginação genérica
│
├── Services/
│   ├── AuthService.cs             # Autenticação via Session
│   └── UsuarioService.cs
│
├── Views/
│   ├── Auth/          # Login
│   ├── Home/          # Dashboard
│   ├── Livro/         # Index, Create, Edit, Details, Delete
│   ├── Categoria/     # Index, Create, Edit, Details, Delete
│   ├── Emprestimo/    # Index, Create, Details, Devolver
│   ├── Leitor/        # Index, Create, Edit, Details, Delete
│   ├── Usuario/       # Index, Create, Edit, Details, Delete
│   └── Shared/        # _Layout, _Paginacao, _ValidationScripts
│
├── wwwroot/
│   ├── css/site.css   # Estilos globais (~1500 linhas)
│   ├── css/login.css
│   ├── js/site.js
│   └── favicon.svg    # Ícone de livro
│
├── Properties/
│   └── launchSettings.json
├── appsettings.json
├── Program.cs
└── Biblioteca.csproj
```

---

## 🗄️ Modelo de Dados

```
Usuario
  Id | Nome | Email | SenhaHash | Role | FotoUrl

Categoria
  Id | Nome

Livro
  Id | Titulo | Autor | AnoPublicacao | CategoriaId (FK) | ImagemUrl

Exemplar
  Id | LivroId (FK) | Tombo | Disponivel | Situacao | DataAquisicao

Leitor
  Id | Nome | Cpf | Email | Telefone | Endereco | DataCadastro | Ativo

Emprestimo
  Id | LivroId (FK) | ExemplarId (FK) | LeitorId (FK)
     | DataEmprestimo | DataPrevistaDevolucao | DataDevolucao | Status
```

### Relacionamentos

```
Categoria  (1) ──── (N)  Livro
Livro      (1) ──── (N)  Exemplar
Exemplar   (1) ──── (N)  Emprestimo
Leitor     (1) ──── (N)  Emprestimo
```

---

## 🚀 Como Executar

### Pré-requisitos

- [.NET SDK 9.0+](https://dotnet.microsoft.com/download)
- SQL Server Express ou LocalDB
- Visual Studio 2022 ou VS Code

### Passo a Passo

```bash
# 1. Clone ou extraia o projeto
cd Biblioteca

# 2. Restaure os pacotes
dotnet restore

# 3. Execute — o banco é criado automaticamente
dotnet run
```

Acesse: **http://localhost:5000**

> O banco de dados é criado automaticamente na primeira execução via `EnsureCreated()` + `DbSeeder`, populando categorias, livros, exemplares, leitores e empréstimos de exemplo.

---

## 👥 Controle de Acesso

| Funcionalidade | Administrador | Funcionário |
|---|:---:|:---:|
| Dashboard | ✅ | ✅ |
| Livros (CRUD) | ✅ | ✅ |
| Categorias (CRUD) | ✅ | ✅ |
| Leitores (CRUD) | ✅ | ✅ |
| Empréstimos | ✅ | ✅ |
| Devoluções | ✅ | ✅ |
| Usuários do sistema | ✅ | ❌ |

---

## 📋 Regras de Negócio

| Regra | Descrição |
|---|---|
| RN01 | Apenas usuários autenticados acessam o sistema |
| RN02 | Apenas Administradores gerenciam usuários do sistema |
| RN03 | Só é possível emprestar um exemplar **disponível** |
| RN04 | O sistema seleciona **automaticamente** o primeiro exemplar disponível (menor tombo) |
| RN05 | Ao registrar o empréstimo, o exemplar é marcado como **indisponível** |
| RN06 | A devolução restaura automaticamente o exemplar para **disponível** |
| RN07 | Empréstimos com prazo vencido são marcados como **Atrasado** automaticamente |
| RN08 | Exemplar marcado como **Perdido** fica indisponível permanentemente |
| RN09 | Categorias com livros vinculados **não podem ser excluídas** |
| RN10 | Leitores com empréstimos ativos/atrasados **não podem ser excluídos** |

---

## 🛠️ Tecnologias Utilizadas

| Tecnologia | Uso |
|---|---|
| **C# 13** | Linguagem principal |
| **ASP.NET Core MVC 9** | Framework web |
| **Entity Framework Core 9** | ORM e mapeamento do banco |
| **SQL Server / LocalDB** | Banco de dados |
| **Razor Views** | Renderização de HTML no servidor |
| **BCrypt.Net** | Hash seguro de senhas |
| **ASP.NET Session** | Controle de autenticação |
| **Data Protection API** | Criptografia de cookies de sessão |
| **Google Books API** | Capas dos livros via ISBN |
| **HTML5 + CSS3 puro** | Interface sem frameworks externos |

---

## 📦 Pacotes NuGet

```xml
<PackageReference Include="Microsoft.EntityFrameworkCore.SqlServer" Version="9.0.0" />
<PackageReference Include="Microsoft.EntityFrameworkCore.Tools" Version="9.0.0" />
<PackageReference Include="Microsoft.EntityFrameworkCore.Design" Version="9.0.0" />
<PackageReference Include="BCrypt.Net-Next" Version="4.0.3" />
```

---

## 🔌 Endpoints Principais

| Método | Rota | Descrição |
|---|---|---|
| GET/POST | `/Auth/Login` | Autenticação |
| GET | `/Home` | Dashboard |
| GET | `/Livro` | Listar livros (grid/lista) |
| GET | `/Livro/Details/{id}` | Detalhes + exemplares + histórico |
| POST | `/Livro/AdicionarExemplar` | Adicionar exemplar ao livro |
| POST | `/Livro/AtualizarExemplar` | Atualizar situação do exemplar |
| GET | `/Leitor` | Listar leitores |
| GET | `/Leitor/Details/{id}` | Perfil + histórico do leitor |
| GET | `/Emprestimo` | Listar empréstimos (com filtros) |
| POST | `/Emprestimo/Create` | Registrar empréstimo |
| POST | `/Emprestimo/Devolver/{id}` | Registrar devolução |
| GET | `/Usuario` | Gerenciar usuários (Admin) |

---

## 🎨 Interface

- Layout com **sidebar fixa** e topbar com foto do usuário
- **Modo grid** (cards com capas) e **modo lista** para livros
- Paginação inteligente com reticências (`1 … 4 5 6 … 10`)
- Filtros rápidos por status nos empréstimos
- Formulários com seções, validação em português e pré-visualização de imagens
- Telas de exclusão centralizadas com proteção contra erros
- Máscaras automáticas de CPF e telefone
- Favicon personalizado (ícone de livro aberto)
- Totalmente responsivo

---

## 📅 Dados de Exemplo (Seed)

Ao executar pela primeira vez, o sistema popula automaticamente:

- **3 usuários** (2 administradores + 1 funcionário)
- **12 categorias** literárias
- **52 livros** com capas via Google Books API
- **~75 exemplares** com tombos gerados automaticamente
- **18 leitores** cadastrados
- **29 empréstimos** com histórico variado:
  - 16 devolvidos (alguns com atraso na devolução)
  - 10 ativos (dentro do prazo)
  - 3 atrasados

---

*Desenvolvido por Erick Santana — Instituto Federal de Sergipe · Programação WEB II*
