using System.ComponentModel.DataAnnotations;

namespace BibliotecaMVC.Models
{
    public class Categoria
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O nome da categoria é obrigatório.")]
        [StringLength(100, ErrorMessage = "O nome deve ter no máximo 100 caracteres.")]
        [Display(Name = "Nome")]
        public string Nome { get; set; } = string.Empty;

        // Navigation
        public ICollection<Livro> Livros { get; set; } = new List<Livro>();
    }
}
