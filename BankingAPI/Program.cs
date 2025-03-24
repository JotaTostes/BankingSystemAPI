using Banking.Infrastructure.Context;
using BankingAPI.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDependencyInjection(builder.Configuration.GetConnectionString("DefaultConnection"));

// Adicionando Controllers
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(
    c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo
        {
            Title = "Documentação API Banking",
            Description = "Api do Sistema de Caixa de Banco, permitindo o cadastro de contas e a transferência de valores.",
            Contact = new OpenApiContact() { Name = "João Guilherme", Email = "joao_tostes17@hotmail.com" },
        });
        c.IncludeXmlComments(Path.Combine(System.AppContext.BaseDirectory, "ApiBanking.xml"));
    }
);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Banking API V1");
        c.RoutePrefix = string.Empty;
    });

    using (var scope = app.Services.CreateScope())
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<BankingDbContext>();
        dbContext.Database.Migrate();
    }
}

app.UseRouting();
app.UseAuthorization();
app.MapControllers();
app.Run();
