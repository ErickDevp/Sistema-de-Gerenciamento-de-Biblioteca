using Biblioteca.Services;
using Biblioteca.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Biblioteca.Controllers
{
    public class AuthController : Controller
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            if (_authService.EstaAutenticado())
                return RedirectToAction("Index", "Home");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var usuario = _authService.Autenticar(model.Email, model.Senha);

            if (usuario == null)
            {
                ModelState.AddModelError("", "E-mail ou senha inválidos.");
                return View(model);
            }

            _authService.IniciarSessao(usuario);
            TempData["Sucesso"] = $"Bem-vindo, {usuario.Nome}!";
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Logout()
        {
            _authService.EncerrarSessao();
            return RedirectToAction("Login");
        }
    }
}
