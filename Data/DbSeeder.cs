using Biblioteca.Models;

namespace Biblioteca.Data
{
    public static class DbSeeder
    {
        static string Capa(string isbn) =>
            $"https://books.google.com/books/content?vid=ISBN{isbn}&printsec=frontcover&img=1&zoom=1";

        public static void Seed(BibliotecaContext context)
        {
            context.Database.EnsureCreated();

            // ── USUÁRIOS ──────────────────────────────────────────────
            if (!context.Usuarios.Any())
            {
                context.Usuarios.AddRange(
                    new Usuario
                    {
                        Nome = "Fabrício Gregório",
                        Email = "fabricio@biblioteca.com",
                        SenhaHash = BCrypt.Net.BCrypt.HashPassword("senha123"),
                        Role = "Administrador",
                        FotoUrl = "https://i.postimg.cc/BngkV3v1/image.png"
                    },
                    new Usuario
                    {
                        Nome = "Erick Santana",
                        Email = "erick@biblioteca.com",
                        SenhaHash = BCrypt.Net.BCrypt.HashPassword("senha123"),
                        Role = "Administrador",
                        FotoUrl = "https://i.postimg.cc/QtZ6vSHP/igm.jpg"
                    },
                    new Usuario
                    {
                        Nome = "Funcionário Padrão",
                        Email = "funcionario@biblioteca.com",
                        SenhaHash = BCrypt.Net.BCrypt.HashPassword("Func@123"),
                        Role = "Funcionario"
                    }
                );
                context.SaveChanges();
            }

            // ── CATEGORIAS ────────────────────────────────────────────
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

            // ── LIVROS ────────────────────────────────────────────────
            if (!context.Livros.Any())
            {
                var cats = context.Categorias.ToList();
                int C(string nome) => cats.First(c => c.Nome == nome).Id;

                context.Livros.AddRange(
                    // Romance
                    new Livro { Titulo = "Dom Casmurro", Autor = "Machado de Assis", AnoPublicacao = 1899, CategoriaId = C("Romance"), ImagemUrl = Capa("9788535910663") },
                    new Livro { Titulo = "A Moreninha", Autor = "Joaquim Manuel de Macedo", AnoPublicacao = 1844, CategoriaId = C("Romance"), ImagemUrl = Capa("9788508008971") },
                    new Livro { Titulo = "Memórias Póstumas de Brás Cubas", Autor = "Machado de Assis", AnoPublicacao = 1881, CategoriaId = C("Romance"), ImagemUrl = Capa("9788535910957") },
                    new Livro { Titulo = "Orgulho e Preconceito", Autor = "Jane Austen", AnoPublicacao = 1813, CategoriaId = C("Romance"), ImagemUrl = Capa("9780141439518") },
                    new Livro { Titulo = "O Pequeno Príncipe", Autor = "Antoine de Saint-Exupéry", AnoPublicacao = 1943, CategoriaId = C("Romance"), ImagemUrl = Capa("9780156012195") },
                    // Ficção Científica
                    new Livro { Titulo = "1984", Autor = "George Orwell", AnoPublicacao = 1949, CategoriaId = C("Ficção Científica"), ImagemUrl = Capa("9780451524935") },
                    new Livro { Titulo = "Admirável Mundo Novo", Autor = "Aldous Huxley", AnoPublicacao = 1932, CategoriaId = C("Ficção Científica"), ImagemUrl = Capa("9780060850524") },
                    new Livro { Titulo = "Fundação", Autor = "Isaac Asimov", AnoPublicacao = 1951, CategoriaId = C("Ficção Científica"), ImagemUrl = Capa("9780553293357") },
                    new Livro { Titulo = "Duna", Autor = "Frank Herbert", AnoPublicacao = 1965, CategoriaId = C("Ficção Científica"), ImagemUrl = Capa("9780441013593") },
                    new Livro { Titulo = "O Guia do Mochileiro das Galáxias", Autor = "Douglas Adams", AnoPublicacao = 1979, CategoriaId = C("Ficção Científica"), ImagemUrl = Capa("9780345391803") },
                    new Livro { Titulo = "Neuromancer", Autor = "William Gibson", AnoPublicacao = 1984, CategoriaId = C("Ficção Científica"), ImagemUrl = Capa("9780441569595") },
                    // Terror
                    new Livro { Titulo = "It — A Coisa", Autor = "Stephen King", AnoPublicacao = 1986, CategoriaId = C("Terror"), ImagemUrl = Capa("9781501156700") },
                    new Livro { Titulo = "O Iluminado", Autor = "Stephen King", AnoPublicacao = 1977, CategoriaId = C("Terror"), ImagemUrl = Capa("9780307743657") },
                    new Livro { Titulo = "Drácula", Autor = "Bram Stoker", AnoPublicacao = 1897, CategoriaId = C("Terror"), ImagemUrl = Capa("9780141439846") },
                    new Livro { Titulo = "Frankenstein", Autor = "Mary Shelley", AnoPublicacao = 1818, CategoriaId = C("Terror"), ImagemUrl = Capa("9780141439471") },
                    // História
                    new Livro { Titulo = "Sapiens", Autor = "Yuval Noah Harari", AnoPublicacao = 2011, CategoriaId = C("História"), ImagemUrl = Capa("9780062316097") },
                    new Livro { Titulo = "Homo Deus", Autor = "Yuval Noah Harari", AnoPublicacao = 2015, CategoriaId = C("História"), ImagemUrl = Capa("9780062464316") },
                    new Livro { Titulo = "21 Lições para o Século 21", Autor = "Yuval Noah Harari", AnoPublicacao = 2018, CategoriaId = C("História"), ImagemUrl = Capa("9780525512172") },
                    new Livro { Titulo = "A Arte da Guerra", Autor = "Sun Tzu", AnoPublicacao = 500, CategoriaId = C("História"), ImagemUrl = Capa("9780140455526") },
                    new Livro { Titulo = "O Mundo Assombrado pelos Demônios", Autor = "Carl Sagan", AnoPublicacao = 1995, CategoriaId = C("História"), ImagemUrl = Capa("9780345409461") },
                    // Tecnologia
                    new Livro { Titulo = "Clean Code", Autor = "Robert C. Martin", AnoPublicacao = 2008, CategoriaId = C("Tecnologia"), ImagemUrl = Capa("9780132350884") },
                    new Livro { Titulo = "O Programador Pragmático", Autor = "Andrew Hunt e David Thomas", AnoPublicacao = 1999, CategoriaId = C("Tecnologia"), ImagemUrl = Capa("9780201616224") },
                    new Livro { Titulo = "Design Patterns", Autor = "Gang of Four", AnoPublicacao = 1994, CategoriaId = C("Tecnologia"), ImagemUrl = Capa("9780201633610") },
                    new Livro { Titulo = "A Catedral e o Bazar", Autor = "Eric S. Raymond", AnoPublicacao = 1999, CategoriaId = C("Tecnologia"), ImagemUrl = Capa("9780596001087") },
                    new Livro { Titulo = "Código Limpo em Arquitetura", Autor = "Robert C. Martin", AnoPublicacao = 2017, CategoriaId = C("Tecnologia"), ImagemUrl = Capa("9780134494166") },
                    // Filosofia
                    new Livro { Titulo = "A República", Autor = "Platão", AnoPublicacao = -380, CategoriaId = C("Filosofia"), ImagemUrl = Capa("9780140455113") },
                    new Livro { Titulo = "Assim Falou Zaratustra", Autor = "Friedrich Nietzsche", AnoPublicacao = 1883, CategoriaId = C("Filosofia"), ImagemUrl = Capa("9780140441185") },
                    new Livro { Titulo = "O Mundo de Sofia", Autor = "Jostein Gaarder", AnoPublicacao = 1991, CategoriaId = C("Filosofia"), ImagemUrl = Capa("9780374525941") },
                    new Livro { Titulo = "Meditações", Autor = "Marco Aurélio", AnoPublicacao = 180, CategoriaId = C("Filosofia"), ImagemUrl = Capa("9780140449334") },
                    // Fantasia
                    new Livro { Titulo = "O Senhor dos Anéis", Autor = "J.R.R. Tolkien", AnoPublicacao = 1954, CategoriaId = C("Fantasia"), ImagemUrl = Capa("9780618640157") },
                    new Livro { Titulo = "Harry Potter e a Pedra Filosofal", Autor = "J.K. Rowling", AnoPublicacao = 1997, CategoriaId = C("Fantasia"), ImagemUrl = Capa("9788532511010") },
                    new Livro { Titulo = "O Nome do Vento", Autor = "Patrick Rothfuss", AnoPublicacao = 2007, CategoriaId = C("Fantasia"), ImagemUrl = Capa("9780756404741") },
                    new Livro { Titulo = "As Crônicas de Nárnia", Autor = "C.S. Lewis", AnoPublicacao = 1950, CategoriaId = C("Fantasia"), ImagemUrl = Capa("9780066238500") },
                    new Livro { Titulo = "O Hobbit", Autor = "J.R.R. Tolkien", AnoPublicacao = 1937, CategoriaId = C("Fantasia"), ImagemUrl = Capa("9780547928227") },
                    // Biografia
                    new Livro { Titulo = "Steve Jobs", Autor = "Walter Isaacson", AnoPublicacao = 2011, CategoriaId = C("Biografia"), ImagemUrl = Capa("9781451648539") },
                    new Livro { Titulo = "Elon Musk", Autor = "Walter Isaacson", AnoPublicacao = 2023, CategoriaId = C("Biografia"), ImagemUrl = Capa("9781982181284") },
                    new Livro { Titulo = "Leonardo da Vinci", Autor = "Walter Isaacson", AnoPublicacao = 2017, CategoriaId = C("Biografia"), ImagemUrl = Capa("9781501139154") },
                    new Livro { Titulo = "A Autobiografia de Malcolm X", Autor = "Malcolm X e Alex Haley", AnoPublicacao = 1965, CategoriaId = C("Biografia"), ImagemUrl = Capa("9780345350688") },
                    // Autoajuda
                    new Livro { Titulo = "Os 7 Hábitos das Pessoas Altamente Eficazes", Autor = "Stephen Covey", AnoPublicacao = 1989, CategoriaId = C("Autoajuda"), ImagemUrl = Capa("9781982137274") },
                    new Livro { Titulo = "O Poder do Hábito", Autor = "Charles Duhigg", AnoPublicacao = 2012, CategoriaId = C("Autoajuda"), ImagemUrl = Capa("9780812981605") },
                    new Livro { Titulo = "Mindset", Autor = "Carol S. Dweck", AnoPublicacao = 2006, CategoriaId = C("Autoajuda"), ImagemUrl = Capa("9780345472328") },
                    new Livro { Titulo = "Como Fazer Amigos e Influenciar Pessoas", Autor = "Dale Carnegie", AnoPublicacao = 1936, CategoriaId = C("Autoajuda"), ImagemUrl = Capa("9780671027032") },
                    // Clássicos
                    new Livro { Titulo = "Dom Quixote", Autor = "Miguel de Cervantes", AnoPublicacao = 1605, CategoriaId = C("Clássicos"), ImagemUrl = Capa("9780060934347") },
                    new Livro { Titulo = "Crime e Castigo", Autor = "Fiódor Dostoiévski", AnoPublicacao = 1866, CategoriaId = C("Clássicos"), ImagemUrl = Capa("9780143058144") },
                    new Livro { Titulo = "O Processo", Autor = "Franz Kafka", AnoPublicacao = 1925, CategoriaId = C("Clássicos"), ImagemUrl = Capa("9780805210408") },
                    new Livro { Titulo = "Odisseia", Autor = "Homero", AnoPublicacao = -800, CategoriaId = C("Clássicos"), ImagemUrl = Capa("9780140268867") },
                    new Livro { Titulo = "Guerra e Paz", Autor = "Lev Tolstói", AnoPublicacao = 1869, CategoriaId = C("Clássicos"), ImagemUrl = Capa("9780143039990") },
                    // Policial
                    new Livro { Titulo = "O Código Da Vinci", Autor = "Dan Brown", AnoPublicacao = 2003, CategoriaId = C("Policial"), ImagemUrl = Capa("9780385504201") },
                    new Livro { Titulo = "Assassinato no Expresso do Oriente", Autor = "Agatha Christie", AnoPublicacao = 1934, CategoriaId = C("Policial"), ImagemUrl = Capa("9780062693662") },
                    new Livro { Titulo = "Os Homens que Não Amavam as Mulheres", Autor = "Stieg Larsson", AnoPublicacao = 2005, CategoriaId = C("Policial"), ImagemUrl = Capa("9780307949486") },
                    new Livro { Titulo = "Sherlock Holmes — Estudo em Vermelho", Autor = "Arthur Conan Doyle", AnoPublicacao = 1887, CategoriaId = C("Policial"), ImagemUrl = Capa("9780140439083") },
                    // Ciência
                    new Livro { Titulo = "Uma Breve História do Tempo", Autor = "Stephen Hawking", AnoPublicacao = 1988, CategoriaId = C("Ciência"), ImagemUrl = Capa("9780553380163") },
                    new Livro { Titulo = "O Gene Egoísta", Autor = "Richard Dawkins", AnoPublicacao = 1976, CategoriaId = C("Ciência"), ImagemUrl = Capa("9780198788607") },
                    new Livro { Titulo = "A Origem das Espécies", Autor = "Charles Darwin", AnoPublicacao = 1859, CategoriaId = C("Ciência"), ImagemUrl = Capa("9780140432053") },
                    new Livro { Titulo = "Cosmos", Autor = "Carl Sagan", AnoPublicacao = 1980, CategoriaId = C("Ciência"), ImagemUrl = Capa("9780345539434") }
                );
                context.SaveChanges();
            }

            // ── EXEMPLARES ────────────────────────────────────────────
            if (!context.Exemplares.Any())
            {
                var livros = context.Livros.ToList();

                string Prefixo(string titulo)
                {
                    var limpo = new string(titulo.ToUpper().Where(char.IsLetter).ToArray());
                    return limpo.Length >= 3 ? limpo[..3] : limpo.PadRight(3, 'X');
                }

                var maisPopulares = new HashSet<string>
                {
                    "1984", "Dom Casmurro", "Sapiens", "O Senhor dos Anéis",
                    "Harry Potter e a Pedra Filosofal", "Clean Code", "O Código Da Vinci",
                    "It — A Coisa", "Duna", "Uma Breve História do Tempo", "O Hobbit",
                    "O Pequeno Príncipe", "Dom Quixote"
                };

                var tombosUsados = new HashSet<string>();
                var exemplares = new List<Exemplar>();
                var rng = new Random(42);

                foreach (var livro in livros)
                {
                    int qtd = maisPopulares.Contains(livro.Titulo) ? 3 : (livro.Id % 3 == 0 ? 2 : 1);
                    var prefixo = Prefixo(livro.Titulo);

                    for (int i = 1; i <= qtd; i++)
                    {
                        var seq = i;
                        var tombo = $"{prefixo}-{seq:D3}";
                        while (tombosUsados.Contains(tombo)) { seq++; tombo = $"{prefixo}-{seq:D3}"; }
                        tombosUsados.Add(tombo);

                        exemplares.Add(new Exemplar
                        {
                            LivroId = livro.Id,
                            Tombo = tombo,
                            Disponivel = true,
                            Situacao = "Bom estado",
                            DataAquisicao = DateTime.Now.AddDays(-rng.Next(60, 730))
                        });
                    }
                }

                context.Exemplares.AddRange(exemplares);
                context.SaveChanges();
            }

            // ── LEITORES ─────────────────────────────────────────────
            if (!context.Leitores.Any())
            {
                context.Leitores.AddRange(
                    new Leitor { Nome = "Ana Clara Souza",          Cpf = "111.222.333-01", Email = "ana.clara@email.com",      Telefone = "(79) 99801-1001", Endereco = "Rua das Flores, 10 - Aracaju/SE",          Ativo = true,  DataCadastro = DateTime.Now.AddDays(-300) },
                    new Leitor { Nome = "Bruno Ferreira Lima",       Cpf = "111.222.333-02", Email = "bruno.lima@email.com",      Telefone = "(79) 99801-1002", Endereco = "Av. Beira Mar, 200 - Aracaju/SE",         Ativo = true,  DataCadastro = DateTime.Now.AddDays(-280) },
                    new Leitor { Nome = "Carla Mendes Oliveira",     Cpf = "111.222.333-03", Email = "carla.mendes@email.com",    Telefone = "(79) 99801-1003", Endereco = "Rua do Sol, 35 - Lagarto/SE",             Ativo = true,  DataCadastro = DateTime.Now.AddDays(-260) },
                    new Leitor { Nome = "Diego Santana Costa",       Cpf = "111.222.333-04", Email = "diego.costa@email.com",     Telefone = "(79) 99801-1004", Endereco = "Rua Nova, 88 - Itabaiana/SE",             Ativo = true,  DataCadastro = DateTime.Now.AddDays(-240) },
                    new Leitor { Nome = "Eduarda Pereira Santos",    Cpf = "111.222.333-05", Email = "eduarda.santos@email.com",  Telefone = "(79) 99801-1005", Endereco = "Av. Central, 150 - Aracaju/SE",           Ativo = true,  DataCadastro = DateTime.Now.AddDays(-220) },
                    new Leitor { Nome = "Felipe Rodrigues Alves",    Cpf = "111.222.333-06", Email = "felipe.alves@email.com",    Telefone = "(79) 99801-1006", Endereco = "Rua das Palmeiras, 72 - São Cristóvão/SE",Ativo = true,  DataCadastro = DateTime.Now.AddDays(-200) },
                    new Leitor { Nome = "Gabriela Torres Nascimento",Cpf = "111.222.333-07", Email = "gabriela.torres@email.com", Telefone = "(79) 99801-1007", Endereco = "Rua do Comércio, 44 - Aracaju/SE",        Ativo = true,  DataCadastro = DateTime.Now.AddDays(-180) },
                    new Leitor { Nome = "Henrique Barbosa Moura",    Cpf = "111.222.333-08", Email = "henrique.moura@email.com",  Telefone = "(79) 99801-1008", Endereco = "Av. Getúlio Vargas, 99 - Lagarto/SE",    Ativo = true,  DataCadastro = DateTime.Now.AddDays(-160) },
                    new Leitor { Nome = "Isabela Cunha Ramos",       Cpf = "111.222.333-09", Email = "isabela.ramos@email.com",   Telefone = "(79) 99801-1009", Endereco = "Rua da Paz, 11 - Aracaju/SE",             Ativo = true,  DataCadastro = DateTime.Now.AddDays(-140) },
                    new Leitor { Nome = "João Victor Pires",         Cpf = "111.222.333-10", Email = "joao.pires@email.com",      Telefone = "(79) 99801-1010", Endereco = "Rua Sete de Setembro, 300 - Aracaju/SE",  Ativo = true,  DataCadastro = DateTime.Now.AddDays(-120) },
                    new Leitor { Nome = "Larissa Freitas Melo",      Cpf = "111.222.333-11", Email = "larissa.melo@email.com",    Telefone = "(79) 99801-1011", Endereco = "Rua Itabaianinha, 55 - Aracaju/SE",       Ativo = true,  DataCadastro = DateTime.Now.AddDays(-100) },
                    new Leitor { Nome = "Marcos Vinícius Teixeira",  Cpf = "111.222.333-12", Email = "marcos.teixeira@email.com", Telefone = "(79) 99801-1012", Endereco = "Av. Francisco Porto, 18 - Aracaju/SE",    Ativo = true,  DataCadastro = DateTime.Now.AddDays(-90)  },
                    new Leitor { Nome = "Natália Borges Lima",       Cpf = "111.222.333-13", Email = "natalia.borges@email.com",  Telefone = "(79) 99801-1013", Endereco = "Rua Lagarto, 66 - Itabaiana/SE",          Ativo = true,  DataCadastro = DateTime.Now.AddDays(-80)  },
                    new Leitor { Nome = "Pedro Augusto Vasconcelos", Cpf = "111.222.333-14", Email = "pedro.vasc@email.com",      Telefone = "(79) 99801-1014", Endereco = "Rua Dom Bosco, 22 - Aracaju/SE",          Ativo = true,  DataCadastro = DateTime.Now.AddDays(-70)  },
                    new Leitor { Nome = "Rafaela Cardoso Nunes",     Cpf = "111.222.333-15", Email = "rafaela.nunes@email.com",   Telefone = "(79) 99801-1015", Endereco = "Av. Tancredo Neves, 500 - Aracaju/SE",    Ativo = true,  DataCadastro = DateTime.Now.AddDays(-60)  },
                    new Leitor { Nome = "Samuel Diniz Araújo",       Cpf = "111.222.333-16", Email = "samuel.araujo@email.com",   Telefone = "(79) 99801-1016", Endereco = "Rua Pacatuba, 8 - Lagarto/SE",            Ativo = true,  DataCadastro = DateTime.Now.AddDays(-50)  },
                    new Leitor { Nome = "Tatiane Oliveira Cruz",     Cpf = "111.222.333-17", Email = "tatiane.cruz@email.com",    Telefone = "(79) 99801-1017", Endereco = "Rua Capitão Mor Galvão, 77 - Aracaju/SE", Ativo = true,  DataCadastro = DateTime.Now.AddDays(-40)  },
                    new Leitor { Nome = "Vinícius Souza Leal",       Cpf = "111.222.333-18", Email = "vinicius.leal@email.com",   Telefone = "(79) 99801-1018", Endereco = "Rua Santo Amaro, 34 - São Cristóvão/SE",  Ativo = false, DataCadastro = DateTime.Now.AddDays(-200) }
                );
                context.SaveChanges();
            }

            // ── EMPRÉSTIMOS ───────────────────────────────────────────
            if (!context.Emprestimos.Any())
            {
                var exemplares = context.Exemplares.ToList();
                var leitores   = context.Leitores.Where(l => l.Ativo).ToList();
                var hoje       = DateTime.Now.Date;

                // Seleciona exemplares para emprestar (pega os primeiros disponíveis de cada livro)
                // Para ter variedade vamos pegar exemplares de índices variados
                var selecionados = exemplares
                    .GroupBy(e => e.LivroId)
                    .SelectMany(g => g.Take(2))
                    .ToList();

                var emprestimos = new List<Emprestimo>();

                void Emprestar(int exIdx, int leitorIdx, int diasAtras, int prazo, bool devolvido, int? diasAtrasDevolucao = null)
                {
                    if (exIdx >= selecionados.Count || leitorIdx >= leitores.Count) return;
                    var ex = selecionados[exIdx];
                    var leitor = leitores[leitorIdx];
                    var dataEmp = hoje.AddDays(-diasAtras);
                    var dataPrev = dataEmp.AddDays(prazo);
                    DateTime? dataDev = null;
                    string status;

                    if (devolvido)
                    {
                        dataDev = dataEmp.AddDays(diasAtrasDevolucao ?? (prazo - 2));
                        status = "Devolvido";
                        // exemplar continua disponivel
                    }
                    else if (dataPrev < hoje)
                    {
                        status = "Atrasado";
                        ex.Disponivel = false;
                    }
                    else
                    {
                        status = "Ativo";
                        ex.Disponivel = false;
                    }

                    emprestimos.Add(new Emprestimo
                    {
                        LivroId = ex.LivroId,
                        ExemplarId = ex.Id,
                        LeitorId = leitor.Id,
                        DataEmprestimo = dataEmp,
                        DataPrevistaDevolucao = dataPrev,
                        DataDevolucao = dataDev,
                        Status = status
                    });
                }

                // ── Devolvidos (histórico variado) ──
                Emprestar(0,  0,  120, 14, true,  12);
                Emprestar(1,  1,  110, 14, true,  10);
                Emprestar(2,  2,  100, 14, true,  14);
                Emprestar(3,  3,   90, 14, true,   8);
                Emprestar(4,  4,   85, 14, true,  20); // devolveu com 6 dias de atraso
                Emprestar(5,  5,   80, 14, true,  13);
                Emprestar(6,  6,   75, 14, true,  11);
                Emprestar(7,  7,   70, 14, true,  14);
                Emprestar(8,  8,   65, 14, true,   9);
                Emprestar(9,  9,   60, 14, true,  16); // devolveu 2 dias depois do prazo
                Emprestar(10, 10,  55, 14, true,  12);
                Emprestar(11, 11,  50, 14, true,  13);
                Emprestar(12, 0,   45, 14, true,  10);
                Emprestar(13, 1,   40, 14, true,  14);
                Emprestar(14, 2,   35, 14, true,   7);
                Emprestar(15, 3,   30, 14, true,  11);

                // ── Ativos (dentro do prazo) ──
                Emprestar(16, 4,  10, 14, false);
                Emprestar(17, 5,   8, 14, false);
                Emprestar(18, 6,   5, 14, false);
                Emprestar(19, 7,   3, 14, false);
                Emprestar(20, 8,   2, 14, false);
                Emprestar(21, 9,   1, 14, false);
                Emprestar(22, 10,  7, 21, false);
                Emprestar(23, 11,  4, 14, false);
                Emprestar(24, 12,  6, 14, false);
                Emprestar(25, 13,  9, 21, false);

                // ── Atrasados ──
                Emprestar(26, 14, 30, 14, false); // 16 dias atrasado
                Emprestar(27, 15, 25, 14, false); // 11 dias atrasado
                Emprestar(28, 16, 20, 14, false); //  6 dias atrasado

                context.Emprestimos.AddRange(emprestimos);
                context.SaveChanges();
            }
        }
    }
}
