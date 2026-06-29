using BibliotecaMVC.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace BibliotecaMVC.ViewModels
{
    public class EmprestimoViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Selecione um livro.")]
        [Display(Name = "Livro")]
        public int LivroId { get; set; }

        [Required(ErrorMessage = "O nome do leitor é obrigatório.")]
        [StringLength(150)]
        [Display(Name = "Nome do Leitor")]
        public string NomeLeitor { get; set; } = string.Empty;

        [Required(ErrorMessage = "A data prevista de devolução é obrigatória.")]
        [DataType(DataType.Date)]
        [Display(Name = "Previsão de Devolução")]
        public DateTime DataPrevistaDevolucao { get; set; } = DateTime.Now.AddDays(14);

        public IEnumerable<SelectListItem>? LivrosDisponiveis { get; set; }
    }
}
