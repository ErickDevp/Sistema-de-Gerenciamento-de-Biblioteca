using BibliotecaMVC.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BibliotecaMVC.Controllers
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

            ViewBag.TotalLivros = _context.Livros.Count();
            ViewBag.LivrosDisponiveis = _context.Livros.Count(l => l.Disponivel);
            ViewBag.LivrosEmprestados = _context.Livros.Count(l => !l.Disponivel);
            ViewBag.TotalCategorias = _context.Categorias.Count();
            ViewBag.TotalUsuarios = _context.Usuarios.Count();
            ViewBag.EmprestimosAtivos = _context.Emprestimos.Count(e => e.Status == "Ativo");
            ViewBag.EmprestimosAtrasados = _context.Emprestimos
                .Count(e => e.Status == "Ativo" && e.DataPrevistaDevolucao.Date < hoje);

            var emprestimosRecentes = _context.Emprestimos
                .Include(e => e.Livro)
                .OrderByDescending(e => e.DataEmprestimo)
                .Take(5)
                .ToList();

            return View(emprestimosRecentes);
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
