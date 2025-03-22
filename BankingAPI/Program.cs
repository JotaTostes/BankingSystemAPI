using Banking.Infrastructure.Context;
using BankingAPI.Configurations;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDependencyInjection(builder.Configuration.GetConnectionString("DefaultConnection"));

// Adicionando Controllers
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
