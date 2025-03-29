using ChallengeAtmApi.Context;
using ChallengeAtmApi.Services;
using ChallengeAtmApi.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Agregar los servicios:
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<PostgresContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

//Agregar los servicios que se crean para cada Controller:
builder.Services.AddScoped<ITransactionTypeService, TransactionTypeService>();



//Buildear la app
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
// TODO Revisar porque se cuelga el swagger al hacer 1 request...
    app.UseSwaggerUI();
}

//Middlewares:
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
