using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using PrintMe.Server.Models.Authentication;

namespace PrintMe.Server.Logic.Authentication;

public static class DIExtensions
{
    public static IServiceCollection ConfigureAuthentication(this IServiceCollection collection, ConfigurationManager manager)
    {
        var secureString = manager["PrivateJWTKey"].ToReadonlySecureString();

        var authOptions = new Options()
        {
            PrivateSecureKey = secureString
        };
        
        collection.AddSingleton(authOptions)
            .AddSingleton<TokenGenerator>();

        collection.AddAuthentication(builder =>
        {
            builder.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            builder.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(builder =>
        {
            builder.RequireHttpsMetadata = false;
            builder.SaveToken = true;
            builder.TokenValidationParameters = new()
            {
                IssuerSigningKey =
                    new SymmetricSecurityKey(
                        Encoding.ASCII.GetBytes(authOptions.SecureBase64Span.ToArray())),
                ValidateIssuer = false,
                ValidateAudience = false,
            };

            builder.Events = new JwtBearerEvents()
            {
                OnMessageReceived = context =>
                {
                    var accessToken = context.Request.Query["access_token"];
                    
                    if (!string.IsNullOrEmpty(accessToken))
                    {
                        context.Token = accessToken;
                    }
                    
                    return Task.CompletedTask;
                }
            };
        });
        
        collection.AddAuthorization();

        var securityScheme = new OpenApiSecurityScheme
        {
            Name = "JWT Authentication",
            Description = "Enter your JWT token in this field",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.Http,
            Scheme = "bearer",
            BearerFormat = "JWT"
        };

        var securityRequirement = new OpenApiSecurityRequirement
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
                []
            }
        };

        
        collection.AddSwaggerGen(options =>
        {
            options.AddSecurityDefinition("Bearer", securityScheme);
            options.AddSecurityRequirement(securityRequirement);
        });
        
        return collection;
    }
}