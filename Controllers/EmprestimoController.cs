using BibliotecaMVC.Data;
using BibliotecaMVC.Models;
using BibliotecaMVC.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BibliotecaMVC.Controllers
{
    public class EmprestimoController : BaseController
    {
        private readonly BibliotecaContext _context;

        public EmprestimoController(BibliotecaContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var hoje = DateTime.Now.Date;

            // Atualiza status de atrasados
            var atrasados = _context.Emprestimos
                .Where(e => e.Status == "Ativo" && e.DataPrevistaDevolucao.Date < hoje)
                .ToList();

            foreach (var e in atrasados)
                e.Status = "Atrasado";

            if (atrasados.Any())
                _context.SaveChanges();

            var emprestimos = _context.Emprestimos
                .Include(e => e.Livro)
                .OrderByDescending(e => e.DataEmprestimo)
                .ToList();

            return View(emprestimos);
        }

        public IActionResult Details(int id)
        {
            var emprestimo = _context.Emprestimos
                .Include(e => e.Livro)
                .ThenInclude(l => l!.Categoria)
                .FirstOrDefault(e => e.Id == id);

            if (emprestimo == null) return NotFound();
            return View(emprestimo);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var vm = new EmprestimoViewModel
            {
                DataPrevistaDevolucao = DateTime.Now.AddDays(14),
                LivrosDisponiveis = _context.Livros
                    .Where(l => l.Disponivel)
                    .OrderBy(l => l.Titulo)
                    .Select(l => new SelectListItem
                    {
                        Value = l.Id.ToString(),
                        Text = $"{l.Titulo} - {l.Autor}"
                    }).ToList()
            };
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(EmprestimoViewModel vm)
        {
            if (vm.DataPrevistaDevolucao.Date <= DateTime.Now.Date)
                ModelState.AddModelError("DataPrevistaDevolucao", "A data de devolução deve ser posterior à data atual.");

            if (!ModelState.IsValid)
            {
                vm.LivrosDisponiveis = _context.Livros
                    .Where(l => l.Disponivel)
                    .OrderBy(l => l.Titulo)
                    .Select(l => new SelectListItem
                    {
                        Value = l.Id.ToString(),
                        Text = $"{l.Titulo} - {l.Autor}"
                    }).ToList();
                return View(vm);
            }

            var livro = _context.Livros.Find(vm.LivroId);
            if (livro == null || !livro.Disponivel)
            {
                TempData["Erro"] = "O livro selecionado não está disponível para empréstimo.";
                return RedirectToAction(nameof(Create));
            }

            // RN08 - marca como indisponível
            livro.Disponivel = false;

            var emprestimo = new Emprestimo
            {
                LivroId = vm.LivroId,
                NomeLeitor = vm.NomeLeitor,
                DataEmprestimo = DateTime.Now,
                DataPrevistaDevolucao = vm.DataPrevistaDevolucao,
                Status = "Ativo"
            };

            _context.Emprestimos.Add(emprestimo);
            _context.SaveChanges();
            TempData["Sucesso"] = "Empréstimo registrado com sucesso!";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Devolver(int id)
        {
            var emprestimo = _context.Emprestimos
                .Include(e => e.Livro)
                .FirstOrDefault(e => e.Id == id);

            if (emprestimo == null) return NotFound();

            if (emprestimo.DataDevolucao != null)
            {
                TempData["Erro"] = "Este empréstimo já foi devolvido.";
                return RedirectToAction(nameof(Index));
            }

            return View(emprestimo);
        }

        [HttpPost, ActionName("Devolver")]
        [ValidateAntiForgeryToken]
        public IActionResult DevolverConfirmed(int id)
        {
            var emprestimo = _context.Emprestimos
                .Include(e => e.Livro)
                .FirstOrDefault(e => e.Id == id);

            if (emprestimo == null) return NotFound();

            // RN05 - devolução
            emprestimo.DataDevolucao = DateTime.Now;
            emprestimo.Status = "Devolvido";

            if (emprestimo.Livro != null)
                emprestimo.Livro.Disponivel = true;

            _context.SaveChanges();
            TempData["Sucesso"] = "Devolução registrada com sucesso!";
            return RedirectToAction(nameof(Index));
        }
    }
}
