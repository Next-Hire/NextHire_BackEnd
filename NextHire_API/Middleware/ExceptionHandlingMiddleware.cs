using NextHire_API.Exceptions;

namespace NextHire_API.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (DomainException ex)
            {
                await EscreverAsync(context, StatusCodes.Status400BadRequest, "requisicao_invalida", ex.Message);
            }
            catch (KeyNotFoundException ex)
            {
                await EscreverAsync(context, StatusCodes.Status404NotFound, "nao_encontrado", ex.Message);
            }
            catch (UnauthorizedAccessException ex)
            {
                await EscreverAsync(context, StatusCodes.Status401Unauthorized, "nao_autorizado", ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro não tratado em {Metodo} {Caminho}", context.Request.Method, context.Request.Path);
                await EscreverAsync(context, StatusCodes.Status500InternalServerError, "erro_interno", "Ocorreu um erro interno ao processar a requisição.");
            }
        }

        private static async Task EscreverAsync(HttpContext context, int status, string codigo, string mensagem)
        {
            if (context.Response.HasStarted)
                return;

            context.Response.Clear();
            context.Response.StatusCode = status;
            context.Response.ContentType = "application/problem+json";

            await context.Response.WriteAsJsonAsync(new
            {
                type = $"https://httpstatuses.com/{status}",
                title = codigo,
                status,
                detail = mensagem,
                traceId = context.TraceIdentifier
            });
        }
    }
}
