using System.ComponentModel.DataAnnotations;

namespace Biblioteca.Models
{
    public class Leitor
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O nome é obrigatório.")]
        [StringLength(150, ErrorMessage = "O nome deve ter no máximo 150 caracteres.")]
        [Display(Name = "Nome Completo")]
        public string Nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "O CPF é obrigatório.")]
        [StringLength(14, ErrorMessage = "CPF inválido.")]
        [Display(Name = "CPF")]
        public string Cpf { get; set; } = string.Empty;

        [Required(ErrorMessage = "O e-mail é obrigatório.")]
        [EmailAddress(ErrorMessage = "Informe um e-mail válido.")]
        [StringLength(150)]
        [Display(Name = "E-mail")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "O telefone é obrigatório.")]
        [StringLength(20)]
        [Display(Name = "Telefone")]
        public string Telefone { get; set; } = string.Empty;

        [StringLength(200)]
        [Display(Name = "Endereço")]
        public string? Endereco { get; set; }

        [Display(Name = "Data de Cadastro")]
        public DateTime DataCadastro { get; set; } = DateTime.Now;

        [Display(Name = "Ativo")]
        public bool Ativo { get; set; } = true;

        // Navigation
        public ICollection<Emprestimo> Emprestimos { get; set; } = new List<Emprestimo>();

        // Computed
        public int TotalEmprestimos => Emprestimos.Count;
        public int EmprestimosAtivos => Emprestimos.Count(e => e.Status == "Ativo");
        public int EmprestimosAtrasados => Emprestimos.Count(e => e.Status == "Atrasado");
    }
}
