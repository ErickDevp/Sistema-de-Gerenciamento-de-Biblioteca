using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Biblioteca.Controllers
{
    public class BaseController : Controller
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            try
            {
                var session = context.HttpContext.Session;
                var usuarioId = session.GetInt32("UsuarioId");

                if (usuarioId == null)
                {
                    context.Result = new RedirectToActionResult("Login", "Auth", null);
                    return;
                }

                ViewBag.UsuarioNome = session.GetString("UsuarioNome");
                ViewBag.UsuarioRole = session.GetString("UsuarioRole");

                base.OnActionExecuting(context);
            }
            catch
            {
                context.Result = new RedirectToActionResult("Login", "Auth", null);
            }
        }

        protected bool EhAdministrador()
        {
            return HttpContext.Session.GetString("UsuarioRole") == "Administrador";
        }

        protected IActionResult AcessoNegado()
        {
            TempData["Erro"] = "Acesso negado. Apenas administradores podem realizar esta ação.";
            return RedirectToAction("Index", "Home");
        }
    }
}
