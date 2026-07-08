using Biblioteca.Data;
using Biblioteca.Models;
using Biblioteca.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Biblioteca.Controllers
{
    public class EmprestimoController : BaseController
    {
        private readonly BibliotecaContext _context;

        public EmprestimoController(BibliotecaContext context)
        {
            _context = context;
        }

        public IActionResult Index(string? filtroStatus, int pagina = 1)
        {
            const int tamanhoPagina = 10;
            var hoje = DateTime.Now.Date;

            var atrasados = _context.Emprestimos
                .Where(e => e.Status == "Ativo" && e.DataPrevistaDevolucao.Date < hoje)
                .ToList();
            foreach (var e in atrasados) e.Status = "Atrasado";
            if (atrasados.Any()) _context.SaveChanges();

            var query = _context.Emprestimos
                .Include(e => e.Livro)
                .Include(e => e.Exemplar)
                .Include(e => e.Leitor)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(filtroStatus))
                query = query.Where(e => e.Status == filtroStatus);

            var paginado = PaginatedList<Emprestimo>.Criar(
                query.OrderByDescending(e => e.DataEmprestimo), pagina, tamanhoPagina);

            ViewBag.FiltroStatus = filtroStatus;
            ViewBag.TotalAtrasados = _context.Emprestimos.Count(e => e.Status == "Atrasado");

            return View(paginado);
        }

        public IActionResult Details(int id)
        {
            var emprestimo = _context.Emprestimos
                .Include(e => e.Livro).ThenInclude(l => l!.Categoria)
                .Include(e => e.Exemplar)
                .Include(e => e.Leitor)
                .FirstOrDefault(e => e.Id == id);

            if (emprestimo == null) return NotFound();
            return View(emprestimo);
        }

        [HttpGet]
        public IActionResult Create(int? leitorId)
        {
            // Lista livros que têm ao menos 1 exemplar disponível
            var livrosDisponiveis = _context.Livros
                .Include(l => l.Exemplares)
                .Where(l => l.Exemplares.Any(ex => ex.Disponivel))
                .OrderBy(l => l.Titulo)
                .Select(l => new SelectListItem
                {
                    Value = l.Id.ToString(),
                    Text = $"{l.Titulo} — {l.Autor} ({l.Exemplares.Count(ex => ex.Disponivel)} disponível/eis)"
                }).ToList();

            var vm = new EmprestimoViewModel
            {
                DataPrevistaDevolucao = DateTime.Now.AddDays(14),
                LeitorId = leitorId ?? 0,
                LivrosDisponiveis = livrosDisponiveis,
                Leitores = _context.Leitores
                    .Where(l => l.Ativo)
                    .OrderBy(l => l.Nome)
                    .Select(l => new SelectListItem
                    {
                        Value = l.Id.ToString(),
                        Text = $"{l.Nome} — {l.Cpf}"
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
                    .Include(l => l.Exemplares)
                    .Where(l => l.Exemplares.Any(ex => ex.Disponivel))
                    .OrderBy(l => l.Titulo)
                    .Select(l => new SelectListItem
                    {
                        Value = l.Id.ToString(),
                        Text = $"{l.Titulo} — {l.Autor} ({l.Exemplares.Count(ex => ex.Disponivel)} disponível/eis)"
                    }).ToList();
                vm.Leitores = _context.Leitores
                    .Where(l => l.Ativo).OrderBy(l => l.Nome)
                    .Select(l => new SelectListItem { Value = l.Id.ToString(), Text = $"{l.Nome} — {l.Cpf}" }).ToList();
                return View(vm);
            }

            // Seleciona automaticamente o primeiro exemplar disponível
            var exemplar = _context.Exemplares
                .Where(ex => ex.LivroId == vm.LivroId && ex.Disponivel)
                .OrderBy(ex => ex.Tombo)
                .FirstOrDefault();

            if (exemplar == null)
            {
                TempData["Erro"] = "Não há exemplares disponíveis para este livro.";
                return RedirectToAction(nameof(Create));
            }

            exemplar.Disponivel = false;

            var emprestimo = new Emprestimo
            {
                LivroId = vm.LivroId,
                ExemplarId = exemplar.Id,
                LeitorId = vm.LeitorId,
                DataEmprestimo = DateTime.Now,
                DataPrevistaDevolucao = vm.DataPrevistaDevolucao,
                Status = "Ativo"
            };

            _context.Emprestimos.Add(emprestimo);
            _context.SaveChanges();
            TempData["Sucesso"] = $"Empréstimo registrado! Exemplar <strong>{exemplar.Tombo}</strong> separado.";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Devolver(int id)
        {
            var emprestimo = _context.Emprestimos
                .Include(e => e.Livro)
                .Include(e => e.Exemplar)
                .Include(e => e.Leitor)
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
                .Include(e => e.Exemplar)
                .FirstOrDefault(e => e.Id == id);

            if (emprestimo == null) return NotFound();

            emprestimo.DataDevolucao = DateTime.Now;
            emprestimo.Status = "Devolvido";

            if (emprestimo.Exemplar != null)
                emprestimo.Exemplar.Disponivel = true;

            _context.SaveChanges();
            TempData["Sucesso"] = "Devolução registrada com sucesso!";
            return RedirectToAction(nameof(Index));
        }
    }
}
