using System.Net;
using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using DotNetEnv;
using Microsoft.EntityFrameworkCore;
using NextHire_API.Contexts;

using System.Text.Json;
using NextHire_API.Exceptions;
using NextHire_API.DTOs.CurriculoAWS;
using NextHire_API.Interfaces;
using NextHire_API.Applications.Convertions.DomainToDto;
using NextHire_API.Domains;
using NextHireAPI.Interfaces;


namespace NextHire_API.Applications.S3
{

    public class CurriculoService
    {
        //private readonly EnvironmentVariableTarget;
        private readonly IAmazonS3 _s3Client;

        private readonly string access_key_id = Environment.GetEnvironmentVariable("AWS_ACCESS_KEY_ID");
        private readonly string secret_Key = Environment.GetEnvironmentVariable("AWS_SECRET_ACCESS_KEY");
        private readonly string session_token = Environment.GetEnvironmentVariable("AWS_SESSION_TOKEN");
        private readonly string nome_bucket = Environment.GetEnvironmentVariable("bucket_name");
        private readonly string regionName = Environment.GetEnvironmentVariable("AWS_REGION");
        private readonly ICurriculoCandidatoRepository _rep;
        private readonly IUsuarioRepository _usuarioRep;
        private readonly GroqCurriculoService _groqService;

        // Lista de regras dos tipos permitidos
        private static readonly HashSet<string> tiposPermitidos = new(StringComparer.OrdinalIgnoreCase)
        {
            "application/pdf",
            "image/png",
            "image/jpeg"
        };

        private static readonly HashSet<string> extensoesPermitidas = new(StringComparer.OrdinalIgnoreCase)
        {
            ".pdf",
            ".png",
            ".jpeg",
            ".jpg"
        };

        public CurriculoService(ICurriculoCandidatoRepository rep, IUsuarioRepository usuarioRep, GroqCurriculoService groqService)
        {
            var regiao = RegionEndpoint.GetBySystemName(regionName);
            var credenciais = new SessionAWSCredentials(access_key_id, secret_Key, session_token);
            _s3Client = new AmazonS3Client(credenciais, regiao);

            _rep = rep;
            _usuarioRep = usuarioRep;
            _groqService = groqService;
        }
        //


        public async Task<string> GerarLinkVisualizacaoCurriculo(string objectKey)
        {
            var request = new GetPreSignedUrlRequest
            {
                BucketName = nome_bucket,
                Key = objectKey, // Ex: "curriculos/123/guia-unico.pdf"
                Expires = DateTime.UtcNow.AddMinutes(50),
                Verb = HttpVerb.GET,
                ResponseHeaderOverrides = new ResponseHeaderOverrides
                {
                    ContentDisposition = "inline",
                    // ContentType = "image/png"
                }
            };



            string url = await _s3Client.GetPreSignedURLAsync(request);
            return url;
        }

        public async Task<string> UploadCurriculoAsync(IFormFile arquivo, CancellationToken ct = default)
        {
            var extensao = Path.GetExtension(arquivo.FileName);
            if (!extensoesPermitidas.Contains(extensao) || !tiposPermitidos.Contains(arquivo.ContentType))
                throw new DomainException("Formato inválido. Apenas arquivos como PDF, JPEG e PNG são aceitos");
            var key = $"curriculos/{Guid.NewGuid()}{extensao}";

            using var memStream = new MemoryStream();
            await arquivo.CopyToAsync(memStream, ct);
            memStream.Position = 0; // Garantindo a leitura desde o comeco do código

            var request = new PutObjectRequest
            {
                BucketName = nome_bucket,
                Key = key,
                InputStream = memStream,
                ContentType = arquivo.ContentType
            };

            var response = await _s3Client.PutObjectAsync(request, ct);
            if (response.HttpStatusCode != System.Net.HttpStatusCode.OK)
                throw new DomainException("Falha ao enviar o arquivo para o Amazon S3");

            return key;
        }


        public async Task<List<LerCurriculoCandidatoDTO?>> Listar()
        {

            var queryList = await _rep.ListarCurriculos();
            if (queryList == null)
                throw new DomainException("Curriculos não encontrados");

            return queryList.Select(varAux => CurriculoCandidatoToDto.LerCurriculoToDTO(varAux)).ToList();
        }

        public async Task<LerCurriculoCandidatoDTO> BuscarPorCurriculoId(int curriculoId)
        {
            var query = await _rep.BuscarCurriculoPorId(curriculoId)
                ?? throw new DomainException("Curriculo não encontrado");

            return CurriculoCandidatoToDto.LerCurriculoToDTO(query);
        }

        public async Task<LerCurriculoCandidatoDTO> BuscarPorCandidatoId(int candidatoId)
        {
            var query = await _rep.BuscarCurriculoPorCandidatoId(candidatoId)
                ?? throw new DomainException("Curriculo não encontrado");

            return CurriculoCandidatoToDto.LerCurriculoToDTO(query);

        }

        public async Task<LerCurriculoCandidatoDTO> AdicionarCurriculoJSON(int candidatoId, CriarCurriculoDTO curriculoDTO)
        {
            if (curriculoDTO == null)
                throw new DomainException("Curriculo é obrigatorio");
            Usuario? usuarioQuery = await _rep.buscarCandidatoId(candidatoId);

            if (usuarioQuery.TipoUsuario != TipoUsuarioEnum.Candidato)
                throw new DomainException("O usuário encontrado não é um candidato");

            CurriculoCandidato? curriculoQuery = await _rep.BuscarCurriculoPorCandidatoId(usuarioQuery.UsuarioId);
             
            if (curriculoQuery != null)
                throw new DomainException("Usuário já possui curriculo. ");
            

            CurriculoCandidato curriculoPOST = new CurriculoCandidato
            {
                CandidatoId = usuarioQuery.UsuarioId,
                CurriculoConvertido = JsonSerializer.Serialize(curriculoDTO.curriculoConvertido)
            };

            curriculoPOST.Candidato = usuarioQuery;

            await _rep.AdicionarCurriculo(curriculoPOST);

            return CurriculoCandidatoToDto.LerCurriculoToDTO(curriculoPOST);
        }

        public async Task<LerCurriculoCandidatoDTO> AdicionarCurriculoArquivo(int candidatoId, IFormFile arquivo)
        {
            if (arquivo == null || arquivo.Length == 0)
                throw new DomainException("O arquivo do currículo é obrigatório.");
 
            Usuario? usuarioQuery = await _rep.buscarCandidatoId(candidatoId);
            if (usuarioQuery == null)
                throw new DomainException("Candidato não encontrado.");
            
            if (usuarioQuery.TipoUsuario != TipoUsuarioEnum.Candidato)
                throw new DomainException("O usuário encontrado não é um candidato.");

            CurriculoCandidato? curriculoQuery = await _rep.BuscarCurriculoPorCandidatoId(usuarioQuery.UsuarioId);
            if (curriculoQuery != null)
                throw new DomainException("Usuário já possui currículo.");
 
            CurriculoDTO? extraido = await _groqService.ExtrairCurriculoArquivoAsync(arquivo);
            if (extraido == null)
                throw new DomainException("Não foi possível extrair os dados do currículo enviado.");

            // Monta o DTO padrão e salva

            CurriculoCandidato curriculoPOST = new CurriculoCandidato
            {
                CandidatoId = usuarioQuery.UsuarioId,
                CurriculoConvertido = JsonSerializer.Serialize(extraido)
            };
            
            curriculoPOST.Candidato = usuarioQuery;
            
            await _rep.AdicionarCurriculo(curriculoPOST);
            return CurriculoCandidatoToDto.LerCurriculoToDTO(curriculoPOST);
        }


        public async Task<LerCurriculoCandidatoDTO> RemoverCurriculo(int curriculoId)
        {
            CurriculoCandidato? curriculoQuery = await _rep.BuscarCurriculoPorId(curriculoId)
                ?? throw new DomainException("Curriculo não encontrado");

            await _rep.RemoverCurriculo(curriculoQuery.CurriculoCandidatoId);

            return CurriculoCandidatoToDto.LerCurriculoToDTO(curriculoQuery);

            
        }



        
    }
}
