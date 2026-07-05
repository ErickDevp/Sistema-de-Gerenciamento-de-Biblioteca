namespace Biblioteca.ViewModels
{
    public class PaginatedList<T>
    {
        public List<T> Itens { get; }
        public int PaginaAtual { get; }
        public int TotalPaginas { get; }
        public int TotalItens { get; }
        public int TamanhoPagina { get; }

        public bool TemPaginaAnterior => PaginaAtual > 1;
        public bool TemProximaPagina => PaginaAtual < TotalPaginas;

        public PaginatedList(List<T> itens, int totalItens, int paginaAtual, int tamanhoPagina)
        {
            Itens = itens;
            TotalItens = totalItens;
            PaginaAtual = paginaAtual;
            TamanhoPagina = tamanhoPagina;
            TotalPaginas = (int)Math.Ceiling(totalItens / (double)tamanhoPagina);
        }

        public static PaginatedList<T> Criar(IQueryable<T> source, int paginaAtual, int tamanhoPagina)
        {
            var totalItens = source.Count();
            var pagina = Math.Max(1, paginaAtual);
            var itens = source
                .Skip((pagina - 1) * tamanhoPagina)
                .Take(tamanhoPagina)
                .ToList();

            return new PaginatedList<T>(itens, totalItens, pagina, tamanhoPagina);
        }

        // Gera os números de página a exibir (ex: 1 ... 4 5 6 ... 10)
        public List<int?> PaginasVisiveis()
        {
            var paginas = new List<int?>();
            if (TotalPaginas <= 7)
            {
                for (int i = 1; i <= TotalPaginas; i++) paginas.Add(i);
                return paginas;
            }

            paginas.Add(1);
            if (PaginaAtual > 3) paginas.Add(null); // ellipsis

            for (int i = Math.Max(2, PaginaAtual - 1); i <= Math.Min(TotalPaginas - 1, PaginaAtual + 1); i++)
                paginas.Add(i);

            if (PaginaAtual < TotalPaginas - 2) paginas.Add(null); // ellipsis
            paginas.Add(TotalPaginas);

            return paginas;
        }
    }
}
