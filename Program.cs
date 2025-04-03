using ChallengeAtmApi.Context;
using ChallengeAtmApi.Services;
using ChallengeAtmApi.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Agregar los servicios:
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    //Codigo generado en Gemini para la configuraci�n de requerimiento de token.
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Tu API", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Encabezado de autorizaci�n JWT usando el esquema Bearer.",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
}); ;
builder.Services.AddDbContext<PostgresContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

//Agregar los servicios que se crean para cada Controller:
builder.Services.AddScoped<ITransactionTypeService, TransactionTypeService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IFailedLoginAttemptService, FailedLoginAttemptService>();

//Agrego el JWT package
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidAudience= "atm-api",
            ValidIssuer= "atm-api",
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("AX,cp+Gf7<])EhIt3?yKA;e]V0[9L30cdGUSjnf,k:tZ0|_M0|%&eKM0L+$wRHD"))
        };
    });
builder.Services.AddAuthorization();

//Buildear la app
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//Middlewares:
app.UseAuthentication();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();