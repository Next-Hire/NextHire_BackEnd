
using DotNetEnv;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using NextHire_API.Contexts;
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
using NextHireAPI.Applications.Auth;

Env.Load();
System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

var builder = WebApplication.CreateBuilder(args);

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

builder.Services.AddSingleton<IConnectionMultiplexer>(_ =>
    ConnectionMultiplexer.Connect(redisConnection)
);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
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
dataSourceBuilder.MapEnum<ModeloTrabaloEnum>("modelo_trabalo_enum");
dataSourceBuilder.MapEnum<NivelPrioridadeEnum>("nivel_prioridade_enum");
dataSourceBuilder.MapEnum<SituacaoProfissionalEnum>("situacao_profissional_enum");
dataSourceBuilder.MapEnum<StatusRelacaoEnum>("status_relacao_enum");
dataSourceBuilder.MapEnum<TipoQuestaoEnum>("tipo_questao_enum");
dataSourceBuilder.MapEnum<TipoUsuarioEnum>("tipo_usuario_enum");

var dataSource = dataSourceBuilder.Build();

// Registar o DbContext usando a fonte de dados com os enums configurados

builder.Services.AddDbContext<NextHireContext>(options =>
    options.UseNpgsql(dataSource, npgsql =>
    {
        npgsql.MapEnum<ClassificacaoQuestaoEnum>("classificacao_questao_enum");
        npgsql.MapEnum<EscalaTrabalhoEnum>("escala_trabalho_enum");
        npgsql.MapEnum<ModeloTrabaloEnum>("modelo_trabalo_enum");
        npgsql.MapEnum<NivelPrioridadeEnum>("nivel_prioridade_enum");
        npgsql.MapEnum<SituacaoProfissionalEnum>("situacao_profissional_enum");
        npgsql.MapEnum<StatusRelacaoEnum>("status_relacao_enum");
        npgsql.MapEnum<TipoQuestaoEnum>("tipo_questao_enum");
        npgsql.MapEnum<TipoUsuarioEnum>("tipo_usuario_enum");
    }));

//? JWT CONFIG

var issuer = builder.Configuration["Jwt:Issuer"];
var audience = builder.Configuration["Jwt:Audience"];

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

builder.Services.AddAuthorization();

// Redis
builder.Services.AddScoped<RedisService>();

// Auth
builder.Services.AddScoped<AutenticacaoService>();
builder.Services.AddScoped<TokenJWT>();

// Area Ocupacao
builder.Services.AddScoped<IAreaOcupacaoRepository, AreaOcupacaoRepository>();
builder.Services.AddScoped<AreaOcupacaoService>();

builder.Services.AddScoped<IAnotacaoRepository, AnotacaoRepository>();
builder.Services.AddScoped<AnotacaoService>();


builder.Services.AddScoped<IHabilidadesRepository, HabilidadeRepositiory>();
builder.Services.AddScoped<HabilidadesService>();

// Experiencia profissional
builder.Services.AddScoped<IExperienciaProfissionalRepository, ExperienciaProfissionalRepository>();
builder.Services.AddScoped<ExperienciaProfissionalService>();

// Usuario
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<UsuarioService>();

// Comentario
builder.Services.AddScoped<IComentarioRepository, ComentarioRepository>();
builder.Services.AddScoped<ComentarioService>();

// Vaga
builder.Services.AddScoped<IVagaRepository, VagaRepository>();
builder.Services.AddScoped<VagaService>();

builder.Services.AddScoped<IInformacaoCandidatoRepository, InformacaoCandidatoRepository>();
builder.Services.AddScoped<InformacaoCandidatoService>();

builder.Services.AddScoped<IAvaliacaoHabilidadeRepository, AvaliacaoHabilidadesRepository>();
builder.Services.AddScoped<AvaliacaoHabilidadeService>();

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
