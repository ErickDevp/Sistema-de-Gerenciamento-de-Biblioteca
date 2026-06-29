using BibliotecaMVC.Data;
using BibliotecaMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BibliotecaMVC.Controllers
{
    public class CategoriaController : BaseController
    {
        private readonly BibliotecaContext _context;

        public CategoriaController(BibliotecaContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var categorias = _context.Categorias
                .Include(c => c.Livros)
                .OrderBy(c => c.Nome)
                .ToList();
            return View(categorias);
        }

        public IActionResult Details(int id)
        {
            var categoria = _context.Categorias
                .Include(c => c.Livros)
                .FirstOrDefault(c => c.Id == id);
            if (categoria == null) return NotFound();
            return View(categoria);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Categoria categoria)
        {
            if (_context.Categorias.Any(c => c.Nome == categoria.Nome))
                ModelState.AddModelError("Nome", "Esta categoria já existe.");

            if (!ModelState.IsValid)
                return View(categoria);

            _context.Categorias.Add(categoria);
            _context.SaveChanges();
            TempData["Sucesso"] = "Categoria cadastrada com sucesso!";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var categoria = _context.Categorias.Find(id);
            if (categoria == null) return NotFound();
            return View(categoria);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Categoria categoria)
        {
            if (id != categoria.Id) return BadRequest();

            if (_context.Categorias.Any(c => c.Nome == categoria.Nome && c.Id != id))
                ModelState.AddModelError("Nome", "Já existe uma categoria com este nome.");

            if (!ModelState.IsValid)
                return View(categoria);

            _context.Categorias.Update(categoria);
            _context.SaveChanges();
            TempData["Sucesso"] = "Categoria atualizada com sucesso!";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var categoria = _context.Categorias
                .Include(c => c.Livros)
                .FirstOrDefault(c => c.Id == id);
            if (categoria == null) return NotFound();
            return View(categoria);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var categoria = _context.Categorias.Include(c => c.Livros).FirstOrDefault(c => c.Id == id);
            if (categoria == null) return NotFound();

            if (categoria.Livros.Any())
            {
                TempData["Erro"] = "Não é possível excluir uma categoria com livros associados.";
                return RedirectToAction(nameof(Index));
            }

            _context.Categorias.Remove(categoria);
            _context.SaveChanges();
            TempData["Sucesso"] = "Categoria excluída com sucesso!";
            return RedirectToAction(nameof(Index));
        }
    }
}
