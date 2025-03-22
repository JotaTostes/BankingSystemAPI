using Banking.Application.Interfaces;
using Banking.Application.Services;
using Banking.Domain.Interfaces;
using Banking.Infrastructure.Context;
using Banking.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using System;

namespace BankingAPI.Configurations
{
    public static class DependencyInjectionConfig
    {
        public static void AddDependencyInjection(this IServiceCollection services, string connectionString)
        {
            // Configuração do banco de dados
            services.AddDbContext<BankingDbContext>(options =>
                options.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0, 25))));

            services.AddScoped<IContaBancariaRepository, ContaBancariaRepository>();
            services.AddScoped<IRegistroDesativacaoRepository, RegistroDesativacaoRepository>();
            services.AddScoped<ITransacoesRepository, TransacoesRepository>();

            services.AddScoped<IContaBancariaService, ContaBancariaService>();
            services.AddScoped<ITransacaoService, TransacaoService>();
        }
    }
}
