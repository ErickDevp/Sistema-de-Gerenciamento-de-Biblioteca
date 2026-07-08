using Biblioteca.Data;
using Biblioteca.Models;
using Biblioteca.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Biblioteca.Controllers
{
    public class LivroController : BaseController
    {
        private readonly BibliotecaContext _context;

        public LivroController(BibliotecaContext context)
        {
            _context = context;
        }

        public IActionResult Index(string? busca, int? categoriaId, int pagina = 1)
        {
            const int tamanhoPagina = 12;

            var query = _context.Livros
                .Include(l => l.Categoria)
                .Include(l => l.Exemplares)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(busca))
                query = query.Where(l => l.Titulo.Contains(busca) || l.Autor.Contains(busca));

            if (categoriaId.HasValue)
                query = query.Where(l => l.CategoriaId == categoriaId.Value);

            var vm = new LivroViewModel
            {
                Livros = PaginatedList<Livro>.Criar(query.OrderBy(l => l.Titulo), pagina, tamanhoPagina),
                FiltroBusca = busca,
                FiltroCategoria = categoriaId,
                Categorias = _context.Categorias
                    .OrderBy(c => c.Nome)
                    .Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Nome })
                    .ToList()
            };

            return View(vm);
        }

        public IActionResult Details(int id)
        {
            var livro = _context.Livros
                .Include(l => l.Categoria)
                .Include(l => l.Exemplares)
                    .ThenInclude(ex => ex.Emprestimos)
                        .ThenInclude(e => e.Leitor)
                .FirstOrDefault(l => l.Id == id);

            if (livro == null) return NotFound();
            return View(livro);
        }

        [HttpGet]
        public IActionResult Create()
        {
            PopularCategorias();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Livro livro, int QuantidadeExemplares = 1)
        {
            ModelState.Remove("Exemplares");

            if (!ModelState.IsValid)
            {
                PopularCategorias();
                return View(livro);
            }

            _context.Livros.Add(livro);
            _context.SaveChanges();

            // Gera tombo: 3 primeiras letras do título + número sequencial
            var prefixo = GerarPrefixo(livro.Titulo);
            var qtd = Math.Max(1, Math.Min(99, QuantidadeExemplares));

            for (int i = 1; i <= qtd; i++)
            {
                var tombo = $"{prefixo}-{i:D3}";
                // Garante tombo único se já existir
                var contador = i;
                while (_context.Exemplares.Any(ex => ex.Tombo == tombo))
                {
                    contador++;
                    tombo = $"{prefixo}-{contador:D3}";
                }

                _context.Exemplares.Add(new Exemplar
                {
                    LivroId = livro.Id,
                    Tombo = tombo,
                    Disponivel = true,
                    Situacao = "Bom estado",
                    DataAquisicao = DateTime.Now
                });
            }

            _context.SaveChanges();
            TempData["Sucesso"] = $"Livro cadastrado com {qtd} exemplar(es)!";
            return RedirectToAction(nameof(Details), new { id = livro.Id });
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var livro = _context.Livros.Find(id);
            if (livro == null) return NotFound();
            PopularCategorias();
            return View(livro);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Livro livro)
        {
            if (id != livro.Id) return BadRequest();

            ModelState.Remove("Exemplares");

            if (!ModelState.IsValid)
            {
                PopularCategorias();
                return View(livro);
            }

            var existente = _context.Livros.Find(id);
            if (existente == null) return NotFound();

            existente.Titulo = livro.Titulo;
            existente.Autor = livro.Autor;
            existente.AnoPublicacao = livro.AnoPublicacao;
            existente.CategoriaId = livro.CategoriaId;
            existente.ImagemUrl = livro.ImagemUrl;

            _context.SaveChanges();
            TempData["Sucesso"] = "Livro atualizado com sucesso!";
            return RedirectToAction(nameof(Details), new { id });
        }

        // Adicionar exemplar avulso
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AdicionarExemplar(int livroId, int quantidade = 1)
        {
            var livro = _context.Livros.Find(livroId);
            if (livro == null) return NotFound();

            var prefixo = GerarPrefixo(livro.Titulo);
            var existentes = _context.Exemplares.Count(ex => ex.LivroId == livroId);
            var qtd = Math.Max(1, Math.Min(10, quantidade));

            for (int i = 1; i <= qtd; i++)
            {
                var seq = existentes + i;
                var tombo = $"{prefixo}-{seq:D3}";
                while (_context.Exemplares.Any(ex => ex.Tombo == tombo))
                {
                    seq++;
                    tombo = $"{prefixo}-{seq:D3}";
                }

                _context.Exemplares.Add(new Exemplar
                {
                    LivroId = livroId,
                    Tombo = tombo,
                    Disponivel = true,
                    Situacao = "Bom estado",
                    DataAquisicao = DateTime.Now
                });
            }

            _context.SaveChanges();
            TempData["Sucesso"] = $"{qtd} exemplar(es) adicionado(s) com sucesso!";
            return RedirectToAction(nameof(Details), new { id = livroId });
        }

        // Atualizar situação de um exemplar
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AtualizarExemplar(int exemplarId, string situacao)
        {
            var exemplar = _context.Exemplares.Find(exemplarId);
            if (exemplar == null) return NotFound();

            exemplar.Situacao = situacao;
            // Apenas "Perdido" torna o exemplar indisponível permanentemente.
            // "Danificado" mantém disponível (pode ser emprestado com ressalva).
            // "Bom estado" restaura a disponibilidade se estava marcado como perdido.
            if (situacao == "Perdido")
                exemplar.Disponivel = false;
            else if (situacao == "Bom estado")
                exemplar.Disponivel = true;
            // "Danificado" não altera o campo Disponivel

            _context.SaveChanges();
            TempData["Sucesso"] = $"Exemplar {exemplar.Tombo} atualizado!";
            return RedirectToAction(nameof(Details), new { id = exemplar.LivroId });
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var livro = _context.Livros
                .Include(l => l.Categoria)
                .Include(l => l.Exemplares)
                .FirstOrDefault(l => l.Id == id);
            if (livro == null) return NotFound();
            return View(livro);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var livro = _context.Livros
                .Include(l => l.Exemplares)
                    .ThenInclude(ex => ex.Emprestimos)
                .FirstOrDefault(l => l.Id == id);

            if (livro == null) return NotFound();

            if (livro.Exemplares.Any(ex => ex.Emprestimos.Any(e => e.Status == "Ativo")))
            {
                TempData["Erro"] = "Não é possível excluir um livro com empréstimos ativos.";
                return RedirectToAction(nameof(Index));
            }

            _context.Livros.Remove(livro);
            _context.SaveChanges();
            TempData["Sucesso"] = "Livro excluído com sucesso!";
            return RedirectToAction(nameof(Index));
        }

        private void PopularCategorias(int? selectedId = null)
        {
            ViewBag.Categorias = _context.Categorias
                .OrderBy(c => c.Nome)
                .Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Nome,
                    Selected = c.Id == selectedId
                }).ToList();
        }

        private static string GerarPrefixo(string titulo)
        {
            var limpo = new string(titulo
                .ToUpper()
                .Where(char.IsLetter)
                .ToArray());
            return limpo.Length >= 3 ? limpo[..3] : limpo.PadRight(3, 'X');
        }
    }
}
