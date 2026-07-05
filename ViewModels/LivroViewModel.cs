using Biblioteca.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Biblioteca.ViewModels
{
    public class LivroViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O título é obrigatório.")]
        [StringLength(200)]
        [Display(Name = "Título")]
        public string Titulo { get; set; } = string.Empty;

        [Required(ErrorMessage = "O autor é obrigatório.")]
        [StringLength(150)]
        [Display(Name = "Autor")]
        public string Autor { get; set; } = string.Empty;

        [Required(ErrorMessage = "O ano de publicação é obrigatório.")]
        [Range(1000, 9999, ErrorMessage = "Informe um ano válido.")]
        [Display(Name = "Ano de Publicação")]
        public int AnoPublicacao { get; set; }

        [Required(ErrorMessage = "Selecione uma categoria.")]
        [Display(Name = "Categoria")]
        public int CategoriaId { get; set; }

        [Display(Name = "Disponível")]
        public bool Disponivel { get; set; } = true;

        public IEnumerable<SelectListItem>? Categorias { get; set; }

        // Filtros
        public string? FiltroBusca { get; set; }
        public int? FiltroCategoria { get; set; }

        // Paginação
        public PaginatedList<Livro>? Livros { get; set; }
    }
}
