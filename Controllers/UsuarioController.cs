using Biblioteca.Models;
using Biblioteca.Services;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Biblioteca.Controllers
{
    public class UsuarioController : BaseController
    {
        private readonly UsuarioService _usuarioService;

        public UsuarioController(UsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        public IActionResult Index()
        {
            if (!EhAdministrador()) return AcessoNegado();
            var usuarios = _usuarioService.ListarTodos();
            return View(usuarios);
        }

        public IActionResult Details(int id)
        {
            if (!EhAdministrador()) return AcessoNegado();
            var usuario = _usuarioService.ObterPorId(id);
            if (usuario == null) return NotFound();
            return View(usuario);
        }

        [HttpGet]
        public IActionResult Create()
        {
            if (!EhAdministrador()) return AcessoNegado();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Usuario usuario, string Senha, string ConfirmarSenha)
        {
            if (!EhAdministrador()) return AcessoNegado();

            if (string.IsNullOrWhiteSpace(Senha))
                ModelState.AddModelError("Senha", "A senha é obrigatória.");

            if (Senha != ConfirmarSenha)
                ModelState.AddModelError("ConfirmarSenha", "As senhas não coincidem.");

            if (_usuarioService.EmailJaExiste(usuario.Email))
                ModelState.AddModelError("Email", "Este e-mail já está cadastrado.");

            ModelState.Remove("SenhaHash");

            if (!ModelState.IsValid)
                return View(usuario);

            _usuarioService.Criar(usuario, Senha);
            TempData["Sucesso"] = "Usuário cadastrado com sucesso!";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            if (!EhAdministrador()) return AcessoNegado();
            var usuario = _usuarioService.ObterPorId(id);
            if (usuario == null) return NotFound();
            return View(usuario);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Usuario usuario, string? NovaSenha, string? ConfirmarSenha)
        {
            if (!EhAdministrador()) return AcessoNegado();

            if (id != usuario.Id) return BadRequest();

            if (!string.IsNullOrWhiteSpace(NovaSenha) && NovaSenha != ConfirmarSenha)
                ModelState.AddModelError("ConfirmarSenha", "As senhas não coincidem.");

            if (_usuarioService.EmailJaExiste(usuario.Email, usuario.Id))
                ModelState.AddModelError("Email", "Este e-mail já está em uso por outro usuário.");

            ModelState.Remove("SenhaHash");

            if (!ModelState.IsValid)
                return View(usuario);

            var existente = _usuarioService.ObterPorId(id);
            if (existente == null) return NotFound();

            existente.Nome = usuario.Nome;
            existente.Email = usuario.Email;
            existente.Role = usuario.Role;
            existente.FotoUrl = usuario.FotoUrl;

            _usuarioService.Atualizar(existente, NovaSenha);

            // Se o usuário editou o próprio perfil, atualiza a sessão imediatamente
            var sessaoId = HttpContext.Session.GetInt32("UsuarioId");
            if (sessaoId == id)
            {
                HttpContext.Session.SetString("UsuarioNome", existente.Nome);
                HttpContext.Session.SetString("UsuarioRole", existente.Role);
                if (!string.IsNullOrEmpty(existente.FotoUrl))
                    HttpContext.Session.SetString("UsuarioFoto", existente.FotoUrl);
                else
                    HttpContext.Session.Remove("UsuarioFoto");
            }

            TempData["Sucesso"] = "Usuário atualizado com sucesso!";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            if (!EhAdministrador()) return AcessoNegado();
            var usuario = _usuarioService.ObterPorId(id);
            if (usuario == null) return NotFound();
            return View(usuario);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            if (!EhAdministrador()) return AcessoNegado();

            var sessaoId = HttpContext.Session.GetInt32("UsuarioId");
            if (sessaoId == id)
            {
                TempData["Erro"] = "Você não pode excluir o próprio usuário logado.";
                return RedirectToAction(nameof(Index));
            }

            _usuarioService.Excluir(id);
            TempData["Sucesso"] = "Usuário excluído com sucesso!";
            return RedirectToAction(nameof(Index));
        }
    }
}
