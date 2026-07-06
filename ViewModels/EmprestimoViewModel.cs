using Biblioteca.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Biblioteca.ViewModels
{
    public class EmprestimoViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Selecione um livro.")]
        [Display(Name = "Livro")]
        public int LivroId { get; set; }

        [Required(ErrorMessage = "Selecione um leitor.")]
        [Display(Name = "Leitor")]
        public int LeitorId { get; set; }

        [Required(ErrorMessage = "A data prevista de devolução é obrigatória.")]
        [DataType(DataType.Date)]
        [Display(Name = "Previsão de Devolução")]
        public DateTime DataPrevistaDevolucao { get; set; } = DateTime.Now.AddDays(14);

        public IEnumerable<SelectListItem>? LivrosDisponiveis { get; set; }
        public IEnumerable<SelectListItem>? Leitores { get; set; }
    }
}
