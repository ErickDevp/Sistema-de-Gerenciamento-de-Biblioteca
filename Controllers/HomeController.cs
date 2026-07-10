using Biblioteca.Data;
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

            ViewBag.TotalLivros = _context.Livros.Count();
            ViewBag.TotalExemplares = _context.Exemplares.Count();
            ViewBag.LivrosDisponiveis = _context.Exemplares.Count(e => e.Disponivel);
            ViewBag.LivrosEmprestados = _context.Exemplares.Count(e => !e.Disponivel);
            ViewBag.TotalCategorias = _context.Categorias.Count();
            ViewBag.TotalUsuarios = _context.Usuarios.Count();
            ViewBag.TotalLeitores = _context.Leitores.Count(l => l.Ativo);
            ViewBag.EmprestimosAtivos = _context.Emprestimos.Count(e => e.Status == "Ativo");
            ViewBag.EmprestimosAtrasados = _context.Emprestimos
                .Count(e => e.Status == "Atrasado");

            var emprestimosRecentes = _context.Emprestimos
                .Include(e => e.Livro)
                .Include(e => e.Leitor)
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
