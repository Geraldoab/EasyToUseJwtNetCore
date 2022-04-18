using EasyToUseJwt;
using EasyToUseJwt.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class EasyToUseDependencyInjection
    {
        public static void AddScopedCustomToken(this IServiceCollection services)
        {
            services.AddScoped<ICustomToken, CustomToken>();
        }

        public static void AddTransientCustomToken(this IServiceCollection services)
        {
            services.AddTransient<ICustomToken, CustomToken>();
        }

        public static void AddSingletonCustomToken(this IServiceCollection services)
        {
            services.AddSingleton<ICustomToken, CustomToken>();
        }

        public static void AddEasyToUseJwt(this IServiceCollection services, Action<JwtConfiguration> setup)
        {
            var jwtConfiguration = new JwtConfiguration();
            setup(jwtConfiguration);

            byte[]? key = null;

            if (string.IsNullOrWhiteSpace(jwtConfiguration.SecretKey))
                throw new ArgumentNullException(nameof(jwtConfiguration.SecretKey));
            else
                key = Encoding.ASCII.GetBytes(jwtConfiguration.SecretKey);

            services.AddAuthentication(auth =>
            {
                auth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                auth.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(setup =>
            {
                setup.RequireHttpsMetadata = jwtConfiguration.RequireHttpsMetadata;
                setup.SaveToken = jwtConfiguration.SaveToken;
                setup.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });
        }

        public static void AddSwaggerJwtConfiguration(this IServiceCollection services)
        {
            services.AddSwaggerGen(s =>
            {
                s.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = @"JWT Authorization header using the Bearer scheme. \r\n\r\n 
                      Enter 'Bearer' [space] and then your token in the text input below.
                      \r\n\r\nExample: 'Bearer 12345abcdef'",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                s.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header,
                        },
                        new List<string>()
                    }
                });
            });
        }
    }
}