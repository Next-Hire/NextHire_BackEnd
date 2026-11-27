
using DotNetEnv;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using NextHire_API.Contexts;
using NextHire_API.Applications.Services;
using NextHire_API.Interfaces;
using NextHire_API.Repository;
using NextHire_API.Applications.Services;
using Npgsql;
using NextHire_API.Domains;
using NextHireAPI.Interfaces;
using NextHireAPI.Repository;
using NextHireAPI.Applications.Services;
using StackExchange.Redis;
using System.Text;
using Amazon.Runtime;
using Amazon.S3;
using NextHire_API.Applications.S3;
using NextHire_API.Applications.Integrations;
using NextHire_API.Exceptions;
using NextHire_API.Applications.Auth;
using Microsoft.EntityFrameworkCore.Diagnostics; // adicione no topo do Program.cs, se necessário

Env.Load();
System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

var builder = WebApplication.CreateBuilder(args);
var acessKeyAws = Environment.GetEnvironmentVariable("AWS_ACCESS_KEY_ID");
var secretKeyAws = Environment.GetEnvironmentVariable("AWS_SECRET_ACCESS_KEY");
var regionName = Environment.GetEnvironmentVariable("AWS_REGION");

// if (string.IsNullOrWhiteSpace(acessKeyAws) || string.IsNullOrWhiteSpace(secretKeyAws) || string.IsNullOrWhiteSpace(regionName))
//     throw new DomainException("Credenciais da AWS não configuradas.");

// var credenciais = new BasicAWSCredentials(acessKeyAws, secretKeyAws);
// var regiao = RegionEndpoint.GetBySystemName(regionName);

// builder.Services.AddSingleton<IAmazonS3>(varAux => new AmazonS3Client(credenciais, regiao));

var conexaoBanco = Environment.GetEnvironmentVariable("CONNECTION_STRING");


var redisConnection = Environment.GetEnvironmentVariable("REDIS_CONNECTION");
var jwtKey = Environment.GetEnvironmentVariable("JWT_KEY");

if (string.IsNullOrWhiteSpace(conexaoBanco))
    throw new Exception("CONNECTION_STRING não configurada.");

if (string.IsNullOrWhiteSpace(redisConnection))
    throw new Exception("REDIS_CONNECTION não configurada.");

if (string.IsNullOrWhiteSpace(jwtKey))
    throw new Exception("JWT_KEY não configurada.");

if (Encoding.UTF8.GetByteCount(jwtKey) < 32)
    throw new Exception("JWT_KEY precisa ter no mínimo 32 bytes.");

var rbosfBaseUrl = Environment.GetEnvironmentVariable("RBOSF_API_BASE_URL")
    ?? builder.Configuration["Rbosf:BaseUrl"]
    ?? "http://localhost:5116/escoapi/v1/";

if (!Uri.TryCreate($"{rbosfBaseUrl.TrimEnd('/')}/", UriKind.Absolute, out Uri? rbosfUri))
    throw new Exception("RBOSF_API_BASE_URL inválida.");

builder.Services.AddSingleton<IConnectionMultiplexer>(_ =>
    ConnectionMultiplexer.Connect(redisConnection)
);

builder.Services.AddControllers();


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    //? Habilita os comentarios XML (summary) nos endpoints do Swagger
    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
        options.IncludeXmlComments(xmlPath, includeControllerXmlComments: true);

    //? Habilita as anotacoes do Swashbuckle ([SwaggerOperation], [SwaggerResponse] etc.)
    options.EnableAnnotations();

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Insira o token JWT desta forma: Bearer {seu_token}"
    });

    options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
    {
        [new OpenApiSecuritySchemeReference("Bearer", document)] = []
    });
});

// ==============================================================================
// Criar a fonte de dados e mapear os ENUMs do PostgreSQL

var dataSourceBuilder = new NpgsqlDataSourceBuilder(conexaoBanco);

dataSourceBuilder.MapEnum<ClassificacaoQuestaoEnum>("classificacao_questao_enum");
dataSourceBuilder.MapEnum<EscalaTrabalhoEnum>("escala_trabalho_enum");
dataSourceBuilder.MapEnum<ModeloTrabaloEnum>("modelo_trabalho_enum");
dataSourceBuilder.MapEnum<NivelPrioridadeEnum>("nivel_prioridade_enum");
dataSourceBuilder.MapEnum<SituacaoProfissionalEnum>("situacao_profissional_enum");
dataSourceBuilder.MapEnum<StatusRelacaoEnum>("status_relacao_enum");
dataSourceBuilder.MapEnum<TipoQuestaoEnum>("tipo_questao_enum");
dataSourceBuilder.MapEnum<TipoUsuarioEnum>("tipo_usuario_enum");
dataSourceBuilder.MapEnum<TipoVagaEnum>("tipo_vaga_enum");

var dataSource = dataSourceBuilder.Build();

// Registar o DbContext usando a fonte de dados com os enums configurados

builder.Services.AddDbContext<NextHireContext>(options =>
    options.UseNpgsql(dataSource, npgsql =>
    {
        npgsql.MapEnum<ClassificacaoQuestaoEnum>("classificacao_questao_enum");
        npgsql.MapEnum<EscalaTrabalhoEnum>("escala_trabalho_enum");
        npgsql.MapEnum<ModeloTrabaloEnum>("modelo_trabalho_enum");
        npgsql.MapEnum<NivelPrioridadeEnum>("nivel_prioridade_enum");
        npgsql.MapEnum<SituacaoProfissionalEnum>("situacao_profissional_enum");
        npgsql.MapEnum<StatusRelacaoEnum>("status_relacao_enum");
        npgsql.MapEnum<TipoQuestaoEnum>("tipo_questao_enum");
        npgsql.MapEnum<TipoUsuarioEnum>("tipo_usuario_enum");
        npgsql.MapEnum<TipoVagaEnum>("tipo_vaga_enum");
    }));

//? JWT CONFIG

//var googleAPI = builder.Configuration.GetSection("");

var issuer = Environment.GetEnvironmentVariable("Jwt__Issuer");
var audience = Environment.GetEnvironmentVariable("Jwt__Audience");



if (string.IsNullOrWhiteSpace(issuer))
    throw new Exception("Jwt:Issuer não configurado.");

if (string.IsNullOrWhiteSpace(audience))
    throw new Exception("Jwt:Audience não configurado.");

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = issuer,
            ValidAudience = audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
            ClockSkew = TimeSpan.Zero
        };
    });

builder.Services.AddAuthorization(options =>
{
    //? Política: somente o próprio usuário (via token) ou um Administrador pode executar a ação
    options.AddPolicy("MesmoUsuarioOuAdm", policy =>
        policy.Requirements.Add(new NextHireAPI.Applications.Validations.SameUserRequirement()));

    //? Política: EXCLUSIVAMENTE o próprio usuário (via token) pode executar a ação
    options.AddPolicy("SomenteProprioUsuario", policy =>
        policy.Requirements.Add(new NextHireAPI.Applications.Validations.OnlySameUserRequirement()));
});

//? Handlers que avaliam as políticas acima
builder.Services.AddSingleton<Microsoft.AspNetCore.Authorization.IAuthorizationHandler,
    NextHireAPI.Applications.Validations.SameUserOrAdmHandler>();
builder.Services.AddSingleton<Microsoft.AspNetCore.Authorization.IAuthorizationHandler,
    NextHireAPI.Applications.Validations.OnlySameUserHandler>();

// Redis
builder.Services.AddScoped<RedisService>();

// Auth
builder.Services.AddHttpClient<AutenticacaoService>();
builder.Services.AddScoped<AutenticacaoService>();
builder.Services.AddScoped<TokenJWT>();

// Area Ocupacao
builder.Services.AddScoped<IAreaOcupacaoRepository, AreaOcupacaoRepository>();
builder.Services.AddScoped<AreaOcupacaoService>();

builder.Services.AddScoped<IAnotacaoRepository, AnotacaoRepository>();
builder.Services.AddScoped<AnotacaoService>();

// Curriculo Vaga
builder.Services.AddScoped<ICurriculoVagaRepository, CurriculoVagaRepository>();
builder.Services.AddScoped<CurriculoVagaService>();

builder.Services.AddScoped<EmpresaService>();
builder.Services.AddScoped<IEmpresaRepository, EmpresaRepository>();


builder.Services.AddScoped<IHabilidadesRepository, HabilidadeRepositiory>();
builder.Services.AddScoped<HabilidadesService>();
builder.Services.AddHttpClient<IRbosfHabilidadeClient, RbosfHabilidadeClient>(client =>
{
    client.BaseAddress = rbosfUri;
    client.Timeout = TimeSpan.FromSeconds(10);
});

// Experiencia profissional
builder.Services.AddScoped<IExperienciaProfissionalRepository, ExperienciaProfissionalRepository>();
builder.Services.AddScoped<ExperienciaProfissionalService>();

// SetorOcupacao
builder.Services.AddScoped<ISetor_OcupacaoRepository, Setor_OcupacaoRepository>();
builder.Services.AddScoped<Setor_OcupacaoService>();

// CandidatoQuestão
builder.Services.AddScoped<ICandidato_QuestaoRepository, CandidatoQuestaoRepository>();
builder.Services.AddScoped<CandidatoQuestaoService>();


// Usuario
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<UsuarioService>();

// Comentario
builder.Services.AddScoped<IComentarioRepository, ComentarioRepository>();
builder.Services.AddScoped<ComentarioService>();

// Entrevistas
builder.Services.AddScoped<IEntrevistaRepository, EntrevistaRepository>();
builder.Services.AddScoped<EntrevistaService>();

//Procesos Seletivos
builder.Services.AddScoped<IProcessoSeletivoRepository, ProcessoSeletivoRepository>();
builder.Services.AddScoped<ProcessoSeletivoService>();

builder.Services.AddScoped<IProcessoSeletivoCandidatoRepository, ProcessoSeletivoCandidatoRepository>();
builder.Services.AddScoped<CandidatoProcessoSeletivoService>();

// Vaga
builder.Services.AddScoped<IVagaRepository, VagaRepository>();
builder.Services.AddScoped<VagaService>();

// Curriculo
builder.Services.AddScoped<CurriculoService>();
builder.Services.AddScoped<ICurriculoCandidatoRepository, CurriculoCandidatoRepository>();
builder.Services.AddHttpClient<GeminiCurriculoService>();
builder.Services.AddHttpClient<GroqCurriculoService>();

builder.Services.AddScoped<IInformacaoCandidatoRepository, InformacaoCandidatoRepository>();
builder.Services.AddScoped<InformacaoCandidatoService>();

builder.Services.AddScoped<IAvaliacaoHabilidadeRepository, AvaliacaoHabilidadesRepository>();
builder.Services.AddScoped<AvaliacaoHabilidadeService>();

// Processo seletivo
builder.Services.AddScoped<IProcessoSeletivoRepository, ProcessoSeletivoRepository>();
builder.Services.AddScoped<ProcessoSeletivoService>();

// Questao (Next Agent)
builder.Services.AddScoped<IQuestaoRepository, QuestaoRepository>();
builder.Services.AddScoped<QuestaoService>();
builder.Services.AddHttpClient<NextAgentService>(client =>
{
    var url = Environment.GetEnvironmentVariable("LMSTUDIO_URL") ?? "http://localhost:1235";
    client.BaseAddress = new Uri(url);
    // Geração de 10 questões com grammar estruturada no LM Studio pode levar ~2 min
    client.Timeout = TimeSpan.FromMinutes(5);
});

// Avaliação de candidaturas pelo Next Agent (nota + comentários)
builder.Services.AddScoped<IAvaliacaoAgenteRepository, AvaliacaoAgenteRepository>();
builder.Services.AddScoped<AvaliacaoAgenteService>();

// cors btw

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
 
// Configure the HTTP request pipeline.

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//if (!app.Environment.IsDevelopment())
//    app.UseHttpsRedirection();

// Mapeia a pasta 'wwwroot' ou 'uploads' para o caminho web '/uploads'

string caminhoUploads = Path.Combine(builder.Environment.ContentRootPath, "wwwroot", "uploads");

if (!Directory.Exists(caminhoUploads))
{
    Directory.CreateDirectory(caminhoUploads);
}

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new Microsoft.Extensions.FileProviders.PhysicalFileProvider(caminhoUploads),
    RequestPath = "/uploads"
});

// Mantém o suporte a arquivos estáticos padrão de wwwroot

app.UseStaticFiles();

app.UseCors("CorsPolicy");

// ATENÇÃO: Autenticação SEMPRE ANTES da Autorização

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();


// dotnet ef migrations add SyncMergeModel --ignore-changes