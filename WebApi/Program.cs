using System.Text;
using Application.Interfaces;
using Application.Services;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using WebApi.Filters;

namespace WebApi
{
    public static class Program
    {
        private const string Issuer = "https://auth.mycompany.local";
        private const string Audience = "my-api";
        private const string SigningKey = "a2cf3be4dbdbed59d07d7ae4f22680d92584cfb5447864feb0868c4343985cdf";

        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();

            builder.Services.AddTransient<ICardRepository, CardRepository>();
            builder.Services.AddTransient<IPermissionService, PermissionService>();
            builder.Services.AddTransient<IActionService, ActionService>();
            builder.Services.AddTransient<CorrelationLoggingFilterAttribute>();
            
            AddJwtAuth(builder);
            AddSwagger(builder);
            
            builder.Services.AddEndpointsApiExplorer();
            
            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            
            app.UseAuthorization();
            app.UseAuthentication();
            
            app.MapControllers();
            
            app.Run();
        }

        private static void AddJwtAuth(WebApplicationBuilder builder)
        {
            builder.Services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = false,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = Issuer,
                        ValidAudience = Audience,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SigningKey)),
                    };
                });
        }

        private static void AddSwagger(WebApplicationBuilder builder)
        {
            builder.Services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                    Description = "Enter: {your JWT token}"
                });

                options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
                {
                    {
                        new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                        {
                            Reference = new Microsoft.OpenApi.Models.OpenApiReference
                            {
                                Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });
        }
    }
}