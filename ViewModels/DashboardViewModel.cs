using Biblioteca.Models;

namespace Biblioteca.ViewModels
{
    public class DashboardViewModel
    {
        // Empréstimos recentes para a tabela
        public IEnumerable<Emprestimo> EmprestimosRecentes { get; set; } = Enumerable.Empty<Emprestimo>();

        // Dados para o gráfico de barras: Empréstimos x Devoluções por mês
        public List<string> Meses { get; set; } = new();
        public List<int> EmprestimosPorMes { get; set; } = new();
        public List<int> DevolucoesPorMes { get; set; } = new();

        // Dados para o gráfico de rosca: Livros por Categoria
        public List<string> Categorias { get; set; } = new();
        public List<int> LivrosPorCategoria { get; set; } = new();
    }
}
