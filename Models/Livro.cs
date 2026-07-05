using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Biblioteca.Models
{
    public class Livro
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O título é obrigatório.")]
        [StringLength(200, ErrorMessage = "O título deve ter no máximo 200 caracteres.")]
        [Display(Name = "Título")]
        public string Titulo { get; set; } = string.Empty;

        [Required(ErrorMessage = "O autor é obrigatório.")]
        [StringLength(150, ErrorMessage = "O autor deve ter no máximo 150 caracteres.")]
        [Display(Name = "Autor")]
        public string Autor { get; set; } = string.Empty;

        [Required(ErrorMessage = "O ano de publicação é obrigatório.")]
        [Range(1000, 9999, ErrorMessage = "Informe um ano válido.")]
        [Display(Name = "Ano de Publicação")]
        public int AnoPublicacao { get; set; }

        [Required(ErrorMessage = "A categoria é obrigatória.")]
        [Display(Name = "Categoria")]
        public int CategoriaId { get; set; }

        [Display(Name = "Disponível")]
        public bool Disponivel { get; set; } = true;

        [StringLength(500)]
        [Display(Name = "URL da Capa")]
        public string? ImagemUrl { get; set; }

        // Navigation
        [ForeignKey("CategoriaId")]
        public Categoria? Categoria { get; set; }

        public ICollection<Emprestimo> Emprestimos { get; set; } = new List<Emprestimo>();
    }
}
