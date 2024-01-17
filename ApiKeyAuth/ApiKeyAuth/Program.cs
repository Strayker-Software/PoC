using ApiKeyAuth.Authentication;
using ApiKeyAuth.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.OpenApi.Models;

namespace ApiKeyAuth
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddScoped<UserService>();

            builder.Services.AddAuthentication("ApiKey")
                .AddScheme<AuthenticationSchemeOptions, ApiKeyAuthHandler>("ApiKey", x => { });
            builder.Services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("apiKey", new OpenApiSecurityScheme
                {
                    Name = "MY-APP-AUTH-TOKEN",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "apiKey",
                    In = ParameterLocation.Header,
                });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "apiKey"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}