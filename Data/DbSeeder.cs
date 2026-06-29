using BibliotecaMVC.Models;

namespace BibliotecaMVC.Data
{
    public static class DbSeeder
    {
        public static void Seed(BibliotecaContext context)
        {
            context.Database.EnsureCreated();

            // Seed admin user
            if (!context.Usuarios.Any())
            {
                context.Usuarios.Add(new Usuario
                {
                    Nome = "Administrador",
                    Email = "admin@biblioteca.com",
                    SenhaHash = BCrypt.Net.BCrypt.HashPassword("Admin@123"),
                    Role = "Administrador"
                });

                context.Usuarios.Add(new Usuario
                {
                    Nome = "Funcionário Padrão",
                    Email = "funcionario@biblioteca.com",
                    SenhaHash = BCrypt.Net.BCrypt.HashPassword("Func@123"),
                    Role = "Funcionario"
                });

                context.SaveChanges();
            }

            // Seed categorias
            if (!context.Categorias.Any())
            {
                context.Categorias.AddRange(
                    new Categoria { Nome = "Romance" },
                    new Categoria { Nome = "Ficção Científica" },
                    new Categoria { Nome = "Terror" },
                    new Categoria { Nome = "História" },
                    new Categoria { Nome = "Tecnologia" },
                    new Categoria { Nome = "Filosofia" }
                );
                context.SaveChanges();
            }

            // Seed livros
            if (!context.Livros.Any())
            {
                var categorias = context.Categorias.ToList();
                var romance = categorias.First(c => c.Nome == "Romance");
                var ficcao = categorias.First(c => c.Nome == "Ficção Científica");
                var historia = categorias.First(c => c.Nome == "História");

                context.Livros.AddRange(
                    new Livro { Titulo = "Dom Casmurro", Autor = "Machado de Assis", AnoPublicacao = 1899, CategoriaId = romance.Id, Disponivel = true },
                    new Livro { Titulo = "1984", Autor = "George Orwell", AnoPublicacao = 1949, CategoriaId = ficcao.Id, Disponivel = true },
                    new Livro { Titulo = "O Senhor dos Anéis", Autor = "J.R.R. Tolkien", AnoPublicacao = 1954, CategoriaId = romance.Id, Disponivel = true },
                    new Livro { Titulo = "Sapiens", Autor = "Yuval Noah Harari", AnoPublicacao = 2011, CategoriaId = historia.Id, Disponivel = true }
                );
                context.SaveChanges();
            }
        }
    }
}
