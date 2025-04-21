using ChallengeAtmApi.Application.Services;
using ChallengeAtmApi.Application.Services.Interfaces;
using ChallengeAtmApi.Infrastructure.Context;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Security.Cryptography;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

var host = Environment.GetEnvironmentVariable("DB_HOST");
var port = Environment.GetEnvironmentVariable("DB_PORT");
var database = Environment.GetEnvironmentVariable("DB_NAME");
var username = Environment.GetEnvironmentVariable("DB_USER");
var password = Environment.GetEnvironmentVariable("DB_PASSWORD");
var connectionString = $"Host={host};Port={port};Database={database};Username={username};Password={password};SSL Mode=Require;Trust Server Certificate=true";


// Agregar los servicios:
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    //Codigo generado en Gemini para la configuración de requerimiento de token.
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "ATM API", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Encabezado de autorización JWT usando el esquema Bearer.",
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
    options.UseNpgsql(connectionString));

//Agregar los servicios que se crean para cada Controller:
builder.Services.AddScoped<ITransactionTypeService, TransactionTypeService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IFailedLoginAttemptService, FailedLoginAttemptService>();
builder.Services.AddScoped<ICardInformationService, CardInformationService>();
builder.Services.AddScoped<ICustomerInformationService, CustomerInformationService>();
builder.Services.AddScoped<ITransactionHistoryService, TransactionHistoryService>();

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