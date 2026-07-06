using Biblioteca.Data;
using Biblioteca.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Biblioteca.Controllers
{
    public class HomeController : BaseController
    {
        private readonly BibliotecaContext _context;

        public HomeController(BibliotecaContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var hoje = DateTime.Now.Date;

            // ── Cards de estatísticas (mantidos via ViewBag) ──────────────────────
            ViewBag.TotalLivros = _context.Livros.Count();
            ViewBag.LivrosDisponiveis = _context.Livros.Count(l => l.Disponivel);
            ViewBag.LivrosEmprestados = _context.Livros.Count(l => !l.Disponivel);
            ViewBag.TotalCategorias = _context.Categorias.Count();
            ViewBag.TotalUsuarios = _context.Usuarios.Count();
            ViewBag.TotalLeitores = _context.Leitores.Count(l => l.Ativo);
            ViewBag.EmprestimosAtivos = _context.Emprestimos.Count(e => e.Status == "Ativo");
            ViewBag.EmprestimosAtrasados = _context.Emprestimos
                .Count(e => e.Status == "Atrasado");

            // ── Últimos empréstimos (mantido igual) ───────────────────────────────
            var emprestimosRecentes = _context.Emprestimos
                .Include(e => e.Livro)
                .Include(e => e.Leitor)
                .OrderByDescending(e => e.DataEmprestimo)
                .Take(5)
                .ToList();

            // ── Gráfico de barras: Empréstimos x Devoluções por mês ──────────────
            // Materializa em memória para montar os últimos 6 meses com segurança
            var dataInicio = new DateTime(hoje.Year, hoje.Month, 1).AddMonths(-5);

            var todosEmprestimos = _context.Emprestimos
                .Select(e => new { e.DataEmprestimo, e.DataDevolucao })
                .Where(e => e.DataEmprestimo >= dataInicio)
                .ToList();

            // Constrói a série dos últimos 6 meses (garantindo que meses sem dados = 0)
            var meses = new List<string>();
            var emprestimosPorMes = new List<int>();
            var devolucoesPorMes = new List<int>();

            for (int i = 0; i < 6; i++)
            {
                var mesRef = dataInicio.AddMonths(i);
                var ano = mesRef.Year;
                var mes = mesRef.Month;

                meses.Add(mesRef.ToString("MMM/yy",
                    new System.Globalization.CultureInfo("pt-BR")));

                emprestimosPorMes.Add(
                    todosEmprestimos.Count(e =>
                        e.DataEmprestimo.Year == ano &&
                        e.DataEmprestimo.Month == mes));

                devolucoesPorMes.Add(
                    todosEmprestimos.Count(e =>
                        e.DataDevolucao.HasValue &&
                        e.DataDevolucao.Value.Year == ano &&
                        e.DataDevolucao.Value.Month == mes));
            }

            // ── Gráfico de rosca: Livros por Categoria ────────────────────────────
            var livrosPorCategoria = _context.Livros
                .Where(l => l.Categoria != null)
                .GroupBy(l => l.Categoria!.Nome)
                .Select(g => new { Categoria = g.Key, Total = g.Count() })
                .OrderByDescending(x => x.Total)
                .ToList();

            var viewModel = new DashboardViewModel
            {
                EmprestimosRecentes = emprestimosRecentes,
                Meses = meses,
                EmprestimosPorMes = emprestimosPorMes,
                DevolucoesPorMes = devolucoesPorMes,
                Categorias = livrosPorCategoria.Select(x => x.Categoria).ToList(),
                LivrosPorCategoria = livrosPorCategoria.Select(x => x.Total).ToList()
            };

            return View(viewModel);
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
