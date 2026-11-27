using System.Net;
using System.Text;

namespace NextHire_API.Applications.Email
{
    public static class EmailTemplateRenderer
    {
        public static (string Html, string Texto) Render(string raizTemplates, string template, IDictionary<string, string> dados)
        {
            string html = Carregar(raizTemplates, template + ".html", dados, htmlEncode: true);
            string texto = Carregar(raizTemplates, template + ".txt", dados, htmlEncode: false);

            if (!string.IsNullOrWhiteSpace(html))
            {
                string layout = Carregar(raizTemplates, "_Layout.html", new Dictionary<string, string>(dados), htmlEncode: true);
                if (!string.IsNullOrWhiteSpace(layout))
                    html = layout.Replace("{{conteudo}}", html);
            }

            if (string.IsNullOrWhiteSpace(texto))
                texto = RemoverTags(html);

            return (html, texto);
        }

        private static string Carregar(string raiz, string arquivo, IDictionary<string, string> dados, bool htmlEncode)
        {
            string caminho = Path.Combine(raiz, arquivo);
            if (!File.Exists(caminho))
                return string.Empty;

            string conteudo = File.ReadAllText(caminho, Encoding.UTF8);

            foreach (var (chave, valor) in dados)
            {
                string seguro = htmlEncode ? WebUtility.HtmlEncode(valor ?? string.Empty) : (valor ?? string.Empty);
                conteudo = conteudo.Replace("{{" + chave + "}}", seguro);
            }

            return conteudo;
        }

        private static string RemoverTags(string html)
            => System.Text.RegularExpressions.Regex.Replace(html, "<[^>]+>", string.Empty);
    }
}
