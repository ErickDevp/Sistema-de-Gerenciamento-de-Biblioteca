using Biblioteca.Models;

namespace Biblioteca.Data
{
    public static class DbSeeder
    {
        // Google Books API retorna capas pelo ISBN com alta precisão
        static string Capa(string isbn) =>
            $"https://books.google.com/books/content?vid=ISBN{isbn}&printsec=frontcover&img=1&zoom=1";

        public static void Seed(BibliotecaContext context)
        {
            context.Database.EnsureCreated();

            if (!context.Usuarios.Any())
            {
                context.Usuarios.AddRange(
                    new Usuario { Nome = "Administrador", Email = "admin@biblioteca.com", SenhaHash = BCrypt.Net.BCrypt.HashPassword("Admin@123"), Role = "Administrador" },
                    new Usuario { Nome = "Funcionário Padrão", Email = "funcionario@biblioteca.com", SenhaHash = BCrypt.Net.BCrypt.HashPassword("Func@123"), Role = "Funcionario" }
                );
                context.SaveChanges();
            }

            if (!context.Categorias.Any())
            {
                context.Categorias.AddRange(
                    new Categoria { Nome = "Romance" },
                    new Categoria { Nome = "Ficção Científica" },
                    new Categoria { Nome = "Terror" },
                    new Categoria { Nome = "História" },
                    new Categoria { Nome = "Tecnologia" },
                    new Categoria { Nome = "Filosofia" },
                    new Categoria { Nome = "Fantasia" },
                    new Categoria { Nome = "Biografia" },
                    new Categoria { Nome = "Autoajuda" },
                    new Categoria { Nome = "Clássicos" },
                    new Categoria { Nome = "Policial" },
                    new Categoria { Nome = "Ciência" }
                );
                context.SaveChanges();
            }

            if (!context.Livros.Any())
            {
                var cats = context.Categorias.ToList();
                int Id(string nome) => cats.First(c => c.Nome == nome).Id;

                context.Livros.AddRange(
                    // Romance
                    new Livro { Titulo = "Dom Casmurro", Autor = "Machado de Assis", AnoPublicacao = 1899, CategoriaId = Id("Romance"), Disponivel = true,
                        ImagemUrl = Capa("9788535910663") },
                    new Livro { Titulo = "A Moreninha", Autor = "Joaquim Manuel de Macedo", AnoPublicacao = 1844, CategoriaId = Id("Romance"), Disponivel = true,
                        ImagemUrl = Capa("9788508008971") },
                    new Livro { Titulo = "Memórias Póstumas de Brás Cubas", Autor = "Machado de Assis", AnoPublicacao = 1881, CategoriaId = Id("Romance"), Disponivel = true,
                        ImagemUrl = Capa("9788535910957") },
                    new Livro { Titulo = "Orgulho e Preconceito", Autor = "Jane Austen", AnoPublicacao = 1813, CategoriaId = Id("Romance"), Disponivel = true,
                        ImagemUrl = Capa("9780141439518") },

                    // Ficção Científica
                    new Livro { Titulo = "1984", Autor = "George Orwell", AnoPublicacao = 1949, CategoriaId = Id("Ficção Científica"), Disponivel = true,
                        ImagemUrl = Capa("9780451524935") },
                    new Livro { Titulo = "Admirável Mundo Novo", Autor = "Aldous Huxley", AnoPublicacao = 1932, CategoriaId = Id("Ficção Científica"), Disponivel = true,
                        ImagemUrl = Capa("9780060850524") },
                    new Livro { Titulo = "Fundação", Autor = "Isaac Asimov", AnoPublicacao = 1951, CategoriaId = Id("Ficção Científica"), Disponivel = true,
                        ImagemUrl = Capa("9780553293357") },
                    new Livro { Titulo = "Duna", Autor = "Frank Herbert", AnoPublicacao = 1965, CategoriaId = Id("Ficção Científica"), Disponivel = true,
                        ImagemUrl = Capa("9780441013593") },

                    // Terror
                    new Livro { Titulo = "It — A Coisa", Autor = "Stephen King", AnoPublicacao = 1986, CategoriaId = Id("Terror"), Disponivel = true,
                        ImagemUrl = Capa("9781501156700") },
                    new Livro { Titulo = "O Iluminado", Autor = "Stephen King", AnoPublicacao = 1977, CategoriaId = Id("Terror"), Disponivel = true,
                        ImagemUrl = Capa("9780307743657") },
                    new Livro { Titulo = "Drácula", Autor = "Bram Stoker", AnoPublicacao = 1897, CategoriaId = Id("Terror"), Disponivel = true,
                        ImagemUrl = Capa("9780141439846") },

                    // História
                    new Livro { Titulo = "Sapiens", Autor = "Yuval Noah Harari", AnoPublicacao = 2011, CategoriaId = Id("História"), Disponivel = true,
                        ImagemUrl = Capa("9780062316097") },
                    new Livro { Titulo = "Homo Deus", Autor = "Yuval Noah Harari", AnoPublicacao = 2015, CategoriaId = Id("História"), Disponivel = true,
                        ImagemUrl = Capa("9780062464316") },
                    new Livro { Titulo = "21 Lições para o Século 21", Autor = "Yuval Noah Harari", AnoPublicacao = 2018, CategoriaId = Id("História"), Disponivel = true,
                        ImagemUrl = Capa("9780525512172") },
                    new Livro { Titulo = "A Arte da Guerra", Autor = "Sun Tzu", AnoPublicacao = 500, CategoriaId = Id("História"), Disponivel = true,
                        ImagemUrl = Capa("9780140455526") },

                    // Tecnologia
                    new Livro { Titulo = "Clean Code", Autor = "Robert C. Martin", AnoPublicacao = 2008, CategoriaId = Id("Tecnologia"), Disponivel = true,
                        ImagemUrl = Capa("9780132350884") },
                    new Livro { Titulo = "O Programador Pragmático", Autor = "Andrew Hunt e David Thomas", AnoPublicacao = 1999, CategoriaId = Id("Tecnologia"), Disponivel = true,
                        ImagemUrl = Capa("9780201616224") },
                    new Livro { Titulo = "Design Patterns", Autor = "Gang of Four", AnoPublicacao = 1994, CategoriaId = Id("Tecnologia"), Disponivel = true,
                        ImagemUrl = Capa("9780201633610") },
                    new Livro { Titulo = "A Catedral e o Bazar", Autor = "Eric S. Raymond", AnoPublicacao = 1999, CategoriaId = Id("Tecnologia"), Disponivel = true,
                        ImagemUrl = Capa("9780596001087") },

                    // Filosofia
                    new Livro { Titulo = "A República", Autor = "Platão", AnoPublicacao = -380, CategoriaId = Id("Filosofia"), Disponivel = true,
                        ImagemUrl = Capa("9780140455113") },
                    new Livro { Titulo = "Assim Falou Zaratustra", Autor = "Friedrich Nietzsche", AnoPublicacao = 1883, CategoriaId = Id("Filosofia"), Disponivel = true,
                        ImagemUrl = Capa("9780140441185") },
                    new Livro { Titulo = "O Mundo de Sofia", Autor = "Jostein Gaarder", AnoPublicacao = 1991, CategoriaId = Id("Filosofia"), Disponivel = true,
                        ImagemUrl = Capa("9780374525941") },

                    // Fantasia
                    new Livro { Titulo = "O Senhor dos Anéis", Autor = "J.R.R. Tolkien", AnoPublicacao = 1954, CategoriaId = Id("Fantasia"), Disponivel = true,
                        ImagemUrl = Capa("9780618640157") },
                    new Livro { Titulo = "Harry Potter e a Pedra Filosofal", Autor = "J.K. Rowling", AnoPublicacao = 1997, CategoriaId = Id("Fantasia"), Disponivel = true,
                        ImagemUrl = Capa("9788532511010") },
                    new Livro { Titulo = "O Nome do Vento", Autor = "Patrick Rothfuss", AnoPublicacao = 2007, CategoriaId = Id("Fantasia"), Disponivel = true,
                        ImagemUrl = Capa("9780756404741") },
                    new Livro { Titulo = "As Crônicas de Nárnia", Autor = "C.S. Lewis", AnoPublicacao = 1950, CategoriaId = Id("Fantasia"), Disponivel = true,
                        ImagemUrl = Capa("9780066238500") },

                    // Biografia
                    new Livro { Titulo = "Steve Jobs", Autor = "Walter Isaacson", AnoPublicacao = 2011, CategoriaId = Id("Biografia"), Disponivel = true,
                        ImagemUrl = Capa("9781451648539") },
                    new Livro { Titulo = "Elon Musk", Autor = "Walter Isaacson", AnoPublicacao = 2023, CategoriaId = Id("Biografia"), Disponivel = true,
                        ImagemUrl = Capa("9781982181284") },
                    new Livro { Titulo = "Leonardo da Vinci", Autor = "Walter Isaacson", AnoPublicacao = 2017, CategoriaId = Id("Biografia"), Disponivel = true,
                        ImagemUrl = Capa("9781501139154") },

                    // Autoajuda
                    new Livro { Titulo = "Os 7 Hábitos das Pessoas Altamente Eficazes", Autor = "Stephen Covey", AnoPublicacao = 1989, CategoriaId = Id("Autoajuda"), Disponivel = true,
                        ImagemUrl = Capa("9781982137274") },
                    new Livro { Titulo = "O Poder do Hábito", Autor = "Charles Duhigg", AnoPublicacao = 2012, CategoriaId = Id("Autoajuda"), Disponivel = true,
                        ImagemUrl = Capa("9780812981605") },
                    new Livro { Titulo = "Mindset", Autor = "Carol S. Dweck", AnoPublicacao = 2006, CategoriaId = Id("Autoajuda"), Disponivel = true,
                        ImagemUrl = Capa("9780345472328") },

                    // Clássicos
                    new Livro { Titulo = "Dom Quixote", Autor = "Miguel de Cervantes", AnoPublicacao = 1605, CategoriaId = Id("Clássicos"), Disponivel = true,
                        ImagemUrl = Capa("9780060934347") },
                    new Livro { Titulo = "Crime e Castigo", Autor = "Fiódor Dostoiévski", AnoPublicacao = 1866, CategoriaId = Id("Clássicos"), Disponivel = true,
                        ImagemUrl = Capa("9780143058144") },
                    new Livro { Titulo = "O Processo", Autor = "Franz Kafka", AnoPublicacao = 1925, CategoriaId = Id("Clássicos"), Disponivel = true,
                        ImagemUrl = Capa("9780805210408") },
                    new Livro { Titulo = "Odisseia", Autor = "Homero", AnoPublicacao = -800, CategoriaId = Id("Clássicos"), Disponivel = true,
                        ImagemUrl = Capa("9780140268867") },

                    // Policial
                    new Livro { Titulo = "O Código Da Vinci", Autor = "Dan Brown", AnoPublicacao = 2003, CategoriaId = Id("Policial"), Disponivel = true,
                        ImagemUrl = Capa("9780385504201") },
                    new Livro { Titulo = "Assassinato no Expresso do Oriente", Autor = "Agatha Christie", AnoPublicacao = 1934, CategoriaId = Id("Policial"), Disponivel = true,
                        ImagemUrl = Capa("9780062693662") },
                    new Livro { Titulo = "Os Homens que Não Amavam as Mulheres", Autor = "Stieg Larsson", AnoPublicacao = 2005, CategoriaId = Id("Policial"), Disponivel = true,
                        ImagemUrl = Capa("9780307949486") },

                    // Ciência
                    new Livro { Titulo = "Uma Breve História do Tempo", Autor = "Stephen Hawking", AnoPublicacao = 1988, CategoriaId = Id("Ciência"), Disponivel = true,
                        ImagemUrl = Capa("9780553380163") },
                    new Livro { Titulo = "O Gene Egoísta", Autor = "Richard Dawkins", AnoPublicacao = 1976, CategoriaId = Id("Ciência"), Disponivel = true,
                        ImagemUrl = Capa("9780198788607") },
                    new Livro { Titulo = "A Origem das Espécies", Autor = "Charles Darwin", AnoPublicacao = 1859, CategoriaId = Id("Ciência"), Disponivel = true,
                        ImagemUrl = Capa("9780140432053") }
                );
                context.SaveChanges();
            }
        }
    }
}
