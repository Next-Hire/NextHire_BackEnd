using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.EntityFrameworkCore;
using NextHire_API.Contexts;
using NextHire_API.DTOs.CurriculoS3DTO;

namespace NextHire_API.Applications.S3
{
    public class CurriculoService
    {
        private readonly NextHireContext _ctx;
        private readonly IAmazonS3 _s3Client;

        public CurriculoService(NextHireContext ctx)
        {
            _ctx = ctx;
            _s3Client = new AmazonS3Client("SUA_ACCESS_KEY", "SUA_SECRET_KEY", RegionEndpoint.SAEast1);
                }

        public async Task ImportarCurriculoDoS3ParaBancoAsync(string bucketName, string objectKey)
        {
            // 1. Prepara a requisição para buscar o objeto no S3
            var request = new GetObjectRequest
            {
                BucketName = bucketName,
                Key = objectKey // Ex: "curriculos/meu-curriculo.pdf"
            };

            byte[] pdfBytes;

            // 2. Faz o download do arquivo do S3 para a memória
            using (GetObjectResponse response = await _s3Client.GetObjectAsync(request))
            using (Stream responseStream = response.ResponseStream)
            using (MemoryStream memoryStream = new MemoryStream())
            {
                await responseStream.CopyToAsync(memoryStream);
                pdfBytes = memoryStream.ToArray();
            }

            // 3. Cria a entidade para o Entity Framework
            var novoCurriculo = new CurriculoDTOS3
            {
                NomeArquivo = Path.GetFileName(objectKey),
                ContentType = "application/pdf",
                ArquivoPdf = pdfBytes,
                //DataUpload = DateTime.UtcNow
            };

            // 4. Salva no banco de dados usando EF Core
            await _ctx.CurriculoCandidato.AddAsync(novoCurriculo); // JSONB 
            await _ctx.SaveChangesAsync();
        }
    }
}
