using NextHire_API.Contexts;
using NextHire_API.Domains;
using NextHire_API.Interfaces;

namespace NextHire_API.Repository
{
    public class InformacaoCandidatoRepository : IInformacaoCandidatoRepository
    {
        private readonly NextHireContext _context;

        public InformacaoCandidatoRepository(NextHireContext context)
        {
            _context = context;
        }

        public void Adicionar(InformacaoCandidato informacaoCandidato)
        {
            _context.InformacaoCandidato.Add(informacaoCandidato);
            _context.SaveChanges();
        }

        public void Atualizar(int id, InformacaoCandidato informacaoCandidato)
        {
            if (informacaoCandidato == null)
                throw new ArgumentNullException(nameof(informacaoCandidato));

            var informacaoExistente = _context.InformacaoCandidato.Find(id);
            if (informacaoExistente == null)
                throw new KeyNotFoundException("Informações do candidato não encontradas.");

            // Não altera InfoCandidatoId nem CandidatoId.
            informacaoExistente.DataNascimento = informacaoCandidato.DataNascimento;
            informacaoExistente.FormacaoAcademica = informacaoCandidato.FormacaoAcademica;
            informacaoExistente.Idioma = informacaoCandidato.Idioma;
            informacaoExistente.Resumo = informacaoCandidato.Resumo;
            informacaoExistente.Telefone = informacaoCandidato.Telefone;
            informacaoExistente.SituacaoProfissional = informacaoCandidato.SituacaoProfissional;

            _context.SaveChanges();
        }

        public InformacaoCandidato BuscarPorCandidato(int candidatoId)
        {
            return _context.InformacaoCandidato
                .FirstOrDefault(i => i.CandidatoId == candidatoId);
        }

        public InformacaoCandidato BuscarPorId(int id)
        {
            return _context.InformacaoCandidato.Find(id);
        }

        public List<InformacaoCandidato> Listar()
        {
            return _context.InformacaoCandidato.ToList();
        }

        public void Deletar(int id)
        {
            var informacao = _context.InformacaoCandidato.Find(id);
            if (informacao == null)
                throw new KeyNotFoundException("Informações do candidato não encontradas.");

            _context.InformacaoCandidato.Remove(informacao);
            _context.SaveChanges();
        }
    }
}
