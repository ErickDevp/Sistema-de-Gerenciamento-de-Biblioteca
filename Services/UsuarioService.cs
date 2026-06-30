using Biblioteca.Data;
using Biblioteca.Models;
using Microsoft.EntityFrameworkCore;

namespace Biblioteca.Services
{
    public class UsuarioService
    {
        private readonly BibliotecaContext _context;

        public UsuarioService(BibliotecaContext context)
        {
            _context = context;
        }

        public IEnumerable<Usuario> ListarTodos()
        {
            return _context.Usuarios.OrderBy(u => u.Nome).ToList();
        }

        public Usuario? ObterPorId(int id)
        {
            return _context.Usuarios.Find(id);
        }

        public bool EmailJaExiste(string email, int? ignorarId = null)
        {
            return _context.Usuarios.Any(u => u.Email == email && u.Id != ignorarId);
        }

        public void Criar(Usuario usuario, string senha)
        {
            usuario.SenhaHash = BCrypt.Net.BCrypt.HashPassword(senha);
            _context.Usuarios.Add(usuario);
            _context.SaveChanges();
        }

        public void Atualizar(Usuario usuario, string? novaSenha = null)
        {
            if (!string.IsNullOrWhiteSpace(novaSenha))
                usuario.SenhaHash = BCrypt.Net.BCrypt.HashPassword(novaSenha);

            _context.Usuarios.Update(usuario);
            _context.SaveChanges();
        }

        public void Excluir(int id)
        {
            var usuario = _context.Usuarios.Find(id);
            if (usuario != null)
            {
                _context.Usuarios.Remove(usuario);
                _context.SaveChanges();
            }
        }
    }
}
