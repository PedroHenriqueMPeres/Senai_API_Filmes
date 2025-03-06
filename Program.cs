using api_filmes_senai.Context;
using api_filmes_senai.Interfaces;
using api_filmes_senai.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Adiciona o contexto do banco de dados (exemplo com SQL Server)
builder.Services.AddDbContext<Filmes_Context>(options => 
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


// adicionar o repositorio e a interface ao container de injecao de dependencia
builder.Services.AddScoped<IGeneroRepository, GeneroRepository>();
builder.Services.AddScoped<IFilmeRepository, FilmeRepository>();
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();



// adicionar o serviço de Controllers 
builder.Services.AddControllers();

// adicionar serviço de JWT Bearer
builder.Services.AddAuthentication(options =>
{
    options.DefaultChallengeScheme = "JwtBearer";
    options.DefaultAuthenticateScheme = "JwtBearer";
})
.AddJwtBearer("JwtBearer", options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,

        ValidateAudience = true,

        ValidateLifetime = true,

        IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes("filmes-chave-autenticacao-webapi-dev")),

        ClockSkew = TimeSpan.FromMinutes(5),

        ValidIssuer = "api_filmes_senai",

        ValidAudience = "api_filmes_senai"

    };
});


var app = builder.Build();

// adicionar o mapeamento dos controllers
app.MapControllers();

// adicionar a autorização
app.UseAuthorization();

// adicionar a autenticação
app.UseAuthentication();

app.Run();



