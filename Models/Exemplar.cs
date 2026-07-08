using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Biblioteca.Models
{
    public class Exemplar
    {
        public int Id { get; set; }

        [Required]
        public int LivroId { get; set; }

        [Required]
        [StringLength(20)]
        [Display(Name = "Tombo")]
        public string Tombo { get; set; } = string.Empty;

        [Display(Name = "Disponível")]
        public bool Disponivel { get; set; } = true;

        [Display(Name = "Situação")]
        [StringLength(30)]
        public string Situacao { get; set; } = "Bom estado"; // Bom estado, Danificado, Perdido

        [Display(Name = "Data de Aquisição")]
        public DateTime DataAquisicao { get; set; } = DateTime.Now;

        // Navigation
        [ForeignKey("LivroId")]
        public Livro? Livro { get; set; }

        public ICollection<Emprestimo> Emprestimos { get; set; } = new List<Emprestimo>();
    }
}
