namespace NextHire_API.Applications.Pagination
{
    public static class PaginationExtensions
    {
        public static List<T> Paginar<T>(this IEnumerable<T> origem, int? pagina, int? tamanhoPagina)
        {
            if (pagina is null && tamanhoPagina is null)
                return origem.ToList();

            int paginaAtual = pagina.GetValueOrDefault(1);
            if (paginaAtual < 1) paginaAtual = 1;

            int tamanho = tamanhoPagina.GetValueOrDefault(20);
            if (tamanho <= 0) tamanho = 20;
            if (tamanho > 100) tamanho = 100;

            return origem.Skip((paginaAtual - 1) * tamanho).Take(tamanho).ToList();
        }
    }
}
