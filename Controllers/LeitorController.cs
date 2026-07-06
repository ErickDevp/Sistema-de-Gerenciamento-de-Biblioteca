using Biblioteca.Data;
using Biblioteca.Models;
using Biblioteca.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Biblioteca.Controllers
{
    public class LeitorController : BaseController
    {
        private readonly BibliotecaContext _context;

        public LeitorController(BibliotecaContext context)
        {
            _context = context;
        }

        public IActionResult Index(string? busca, string? filtroStatus, int pagina = 1)
        {
            const int tamanhoPagina = 10;

            var query = _context.Leitores.AsQueryable();

            if (!string.IsNullOrWhiteSpace(busca))
                query = query.Where(l => l.Nome.Contains(busca) || l.Cpf.Contains(busca) || l.Email.Contains(busca));

            if (filtroStatus == "ativo")   query = query.Where(l => l.Ativo);
            if (filtroStatus == "inativo") query = query.Where(l => !l.Ativo);

            var paginado = PaginatedList<Leitor>.Criar(query.OrderBy(l => l.Nome), pagina, tamanhoPagina);

            ViewBag.Busca = busca;
            ViewBag.FiltroStatus = filtroStatus;

            return View(paginado);
        }

        public IActionResult Details(int id)
        {
            var leitor = _context.Leitores
                .Include(l => l.Emprestimos)
                    .ThenInclude(e => e.Livro)
                .FirstOrDefault(l => l.Id == id);

            if (leitor == null) return NotFound();
            return View(leitor);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Leitor leitor)
        {
            if (_context.Leitores.Any(l => l.Cpf == leitor.Cpf))
                ModelState.AddModelError("Cpf", "Já existe um leitor com este CPF.");

            if (_context.Leitores.Any(l => l.Email == leitor.Email))
                ModelState.AddModelError("Email", "Já existe um leitor com este e-mail.");

            if (!ModelState.IsValid) return View(leitor);

            leitor.DataCadastro = DateTime.Now;
            leitor.Ativo = true;
            _context.Leitores.Add(leitor);
            _context.SaveChanges();
            TempData["Sucesso"] = "Leitor cadastrado com sucesso!";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var leitor = _context.Leitores.Find(id);
            if (leitor == null) return NotFound();
            return View(leitor);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Leitor leitor)
        {
            if (id != leitor.Id) return BadRequest();

            if (_context.Leitores.Any(l => l.Cpf == leitor.Cpf && l.Id != id))
                ModelState.AddModelError("Cpf", "Já existe outro leitor com este CPF.");

            if (_context.Leitores.Any(l => l.Email == leitor.Email && l.Id != id))
                ModelState.AddModelError("Email", "Já existe outro leitor com este e-mail.");

            if (!ModelState.IsValid) return View(leitor);

            var existente = _context.Leitores.Find(id);
            if (existente == null) return NotFound();

            existente.Nome = leitor.Nome;
            existente.Cpf = leitor.Cpf;
            existente.Email = leitor.Email;
            existente.Telefone = leitor.Telefone;
            existente.Endereco = leitor.Endereco;
            existente.Ativo = leitor.Ativo;

            _context.SaveChanges();
            TempData["Sucesso"] = "Leitor atualizado com sucesso!";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var leitor = _context.Leitores
                .Include(l => l.Emprestimos)
                .FirstOrDefault(l => l.Id == id);
            if (leitor == null) return NotFound();
            return View(leitor);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var leitor = _context.Leitores
                .Include(l => l.Emprestimos)
                .FirstOrDefault(l => l.Id == id);
            if (leitor == null) return NotFound();

            if (leitor.Emprestimos.Any(e => e.Status == "Ativo" || e.Status == "Atrasado"))
            {
                TempData["Erro"] = "Não é possível excluir um leitor com empréstimos ativos ou atrasados.";
                return RedirectToAction(nameof(Index));
            }

            _context.Emprestimos.RemoveRange(leitor.Emprestimos);
            _context.Leitores.Remove(leitor);
            _context.SaveChanges();
            TempData["Sucesso"] = "Leitor excluído com sucesso!";
            return RedirectToAction(nameof(Index));
        }
    }
}
