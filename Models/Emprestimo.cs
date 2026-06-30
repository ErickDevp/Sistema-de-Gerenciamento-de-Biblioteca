using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Biblioteca.Models
{
    public class Emprestimo
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O livro é obrigatório.")]
        [Display(Name = "Livro")]
        public int LivroId { get; set; }

        [Required(ErrorMessage = "O nome do leitor é obrigatório.")]
        [StringLength(150, ErrorMessage = "O nome deve ter no máximo 150 caracteres.")]
        [Display(Name = "Nome do Leitor")]
        public string NomeLeitor { get; set; } = string.Empty;

        [Display(Name = "Data do Empréstimo")]
        public DateTime DataEmprestimo { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "A data prevista de devolução é obrigatória.")]
        [Display(Name = "Previsão de Devolução")]
        public DateTime DataPrevistaDevolucao { get; set; }

        [Display(Name = "Data de Devolução")]
        public DateTime? DataDevolucao { get; set; }

        [Display(Name = "Status")]
        public string Status { get; set; } = "Ativo"; // Ativo, Devolvido, Atrasado

        // Navigation
        [ForeignKey("LivroId")]
        public Livro? Livro { get; set; }

        [NotMapped]
        public bool EstaAtrasado =>
            DataDevolucao == null && DateTime.Now.Date > DataPrevistaDevolucao.Date;
    }
}
