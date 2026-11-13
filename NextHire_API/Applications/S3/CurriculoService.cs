using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using DotNetEnv;
using Microsoft.EntityFrameworkCore;
using NextHire_API.Contexts;
using NextHire_API.DTOs.CurriculoS3DTO;
using System.Text.Json;
    

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

        public CurriculoService(NextHireContext ctx)
        {
            var credentials = new SessionAWSCredentials(access_key_id, secret_Key, session_token);
            _s3Client = new AmazonS3Client(credentials, RegionEndpoint.USEast1);
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
                    ContentType = "image/png"
                }
            };

            string url = await _s3Client.GetPreSignedURLAsync(request);
            return url;
        }



        
    }
}
