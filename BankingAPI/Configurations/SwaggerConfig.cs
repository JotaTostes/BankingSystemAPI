﻿using Microsoft.OpenApi.Models;
using Asp.Versioning.ApiExplorer;

namespace BankingAPI.Configurations
{
    public static class SwaggerConfig
    {
        /// <summary>
        /// Configuração do Swagger
        /// </summary>
        /// <param name="services"></param>
        public static void AddSwaggerConfiguration(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Documentação API Banking",
                    Description = "API do Sistema de Caixa de Banco, permitindo o cadastro de contas e a transferência de valores.",
                    Contact = new OpenApiContact
                    {
                        Name = "João Guilherme",
                        Email = "joao_tostes17@hotmail.com"
                    }
                });

                var xmlPath = Path.Combine(AppContext.BaseDirectory, "ApiBanking.xml");
                c.IncludeXmlComments(xmlPath);
            });
            
        }

        /// <summary>
        /// Utilização do Swagger
        /// </summary>
        /// <param name="app"></param>
        public static void UseSwaggerConfiguration(this WebApplication app)
        {
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
                    foreach (var description in provider.ApiVersionDescriptions)
                    {
                        c.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
                    }
                });
            }
        }
    }
}
