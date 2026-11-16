using System.Net;
using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using DotNetEnv;
using Microsoft.EntityFrameworkCore;
using NextHire_API.Contexts;
using NextHire_API.DTOs.CurriculoS3DTO;
using System.Text.Json;
using NextHire_API.Exceptions;


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

        public CurriculoService()
        {
            var regiao = RegionEndpoint.GetBySystemName(regionName);
            var credenciais = new SessionAWSCredentials(access_key_id, secret_Key, session_token);
            _s3Client = new AmazonS3Client(credenciais, regiao);
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



        
    }
}
