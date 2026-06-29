using BibliotecaMVC.Data;
using BibliotecaMVC.Models;
using BibliotecaMVC.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BibliotecaMVC.Controllers
{
    public class LivroController : BaseController
    {
        private readonly BibliotecaContext _context;

        public LivroController(BibliotecaContext context)
        {
            _context = context;
        }

        public IActionResult Index(string? busca, int? categoriaId)
        {
            var query = _context.Livros
                .Include(l => l.Categoria)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(busca))
                query = query.Where(l => l.Titulo.Contains(busca) || l.Autor.Contains(busca));

            if (categoriaId.HasValue)
                query = query.Where(l => l.CategoriaId == categoriaId.Value);

            var vm = new LivroViewModel
            {
                Livros = query.OrderBy(l => l.Titulo).ToList(),
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
                .Include(l => l.Emprestimos)
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
        public IActionResult Create(Livro livro)
        {
            if (!ModelState.IsValid)
            {
                PopularCategorias();
                return View(livro);
            }

            livro.Disponivel = true;
            _context.Livros.Add(livro);
            _context.SaveChanges();
            TempData["Sucesso"] = "Livro cadastrado com sucesso!";
            return RedirectToAction(nameof(Index));
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

            if (!ModelState.IsValid)
            {
                PopularCategorias();
                return View(livro);
            }

            _context.Livros.Update(livro);
            _context.SaveChanges();
            TempData["Sucesso"] = "Livro atualizado com sucesso!";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var livro = _context.Livros.Include(l => l.Categoria).FirstOrDefault(l => l.Id == id);
            if (livro == null) return NotFound();
            return View(livro);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var livro = _context.Livros.Include(l => l.Emprestimos).FirstOrDefault(l => l.Id == id);
            if (livro == null) return NotFound();

            if (livro.Emprestimos.Any(e => e.Status == "Ativo"))
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
    }
}
