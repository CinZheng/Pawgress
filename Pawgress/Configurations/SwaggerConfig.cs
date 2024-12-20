using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace Pawgress.Configurations
{
    public static class SwaggerConfig
    {
        public static IServiceCollection ConfigureSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                // basisinformatie swagger
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Pawgress API",
                    Version = "v1",
                    Description = "API documentatie voor Pawgress applicatie"
                });

                // JWT authenticatie
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Voer je JWT-token in!"
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
            });

            return services;
        }
    }
}
