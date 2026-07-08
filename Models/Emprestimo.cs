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

        [Required(ErrorMessage = "O exemplar é obrigatório.")]
        [Display(Name = "Exemplar")]
        public int ExemplarId { get; set; }

        [Required(ErrorMessage = "O leitor é obrigatório.")]
        [Display(Name = "Leitor")]
        public int LeitorId { get; set; }

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

        [ForeignKey("ExemplarId")]
        public Exemplar? Exemplar { get; set; }

        [ForeignKey("LeitorId")]
        public Leitor? Leitor { get; set; }

        [NotMapped]
        public bool EstaAtrasado =>
            DataDevolucao == null && DateTime.Now.Date > DataPrevistaDevolucao.Date;
    }
}
