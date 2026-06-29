using BibliotecaMVC.Data;
using BibliotecaMVC.Models;

namespace BibliotecaMVC.Services
{
    public class AuthService
    {
        private readonly BibliotecaContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthService(BibliotecaContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public Usuario? Autenticar(string email, string senha)
        {
            var usuario = _context.Usuarios.FirstOrDefault(u => u.Email == email);
            if (usuario == null) return null;
            if (!BCrypt.Net.BCrypt.Verify(senha, usuario.SenhaHash)) return null;
            return usuario;
        }

        public void IniciarSessao(Usuario usuario)
        {
            var session = _httpContextAccessor.HttpContext?.Session;
            if (session != null)
            {
                session.SetInt32("UsuarioId", usuario.Id);
                session.SetString("UsuarioNome", usuario.Nome);
                session.SetString("UsuarioEmail", usuario.Email);
                session.SetString("UsuarioRole", usuario.Role);
            }
        }

        public void EncerrarSessao()
        {
            _httpContextAccessor.HttpContext?.Session.Clear();
        }

        public bool EstaAutenticado()
        {
            return _httpContextAccessor.HttpContext?.Session.GetInt32("UsuarioId") != null;
        }

        public string? ObterRole()
        {
            return _httpContextAccessor.HttpContext?.Session.GetString("UsuarioRole");
        }

        public bool EhAdministrador()
        {
            return ObterRole() == "Administrador";
        }
    }
}
