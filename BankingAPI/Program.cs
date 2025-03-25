using Banking.Infrastructure.Context;
using BankingAPI.Configurations;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDependencyInjection(builder.Configuration.GetConnectionString("DefaultConnection"));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerConfiguration();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    using (var scope = app.Services.CreateScope())
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<BankingDbContext>();
        dbContext.Database.Migrate();
    }
}

app.UseSwaggerConfiguration();
app.UseRouting();
app.UseAuthorization();
app.MapControllers();
app.Run();
