using Caching.Redis;
using Ecommerce.Base;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Hosting;
using Microsoft.OpenApi.Models;
using Product.Application;
using Product.Persistence;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = "https://localhost:5001";
        options.Audience = "productApi";
        options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        {
            RoleClaimType = "role",
            ValidateAudience = false
        };
    });

builder.Services.AddAuthorizationBuilder()
    .AddPolicy("productApi", policy =>
    {
        policy.RequireAuthenticatedUser();
        policy.RequireClaim("scope", "productApi");
    })
    .AddPolicy("AdminPolicy", policy => policy.RequireClaim("Admin"));

// Add services to the container.

builder.Services.AddRedisCache();
builder.Services.AddPersistenceServices();
builder.Services.AddApplicationServices();
builder.Services.AddBaseServices();

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen(options =>
{
    // Scheme Definition 
    options.AddSecurityDefinition("oauth2", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.OAuth2,
        Flows = new OpenApiOAuthFlows
        {
            AuthorizationCode = new OpenApiOAuthFlow
            {
                AuthorizationUrl = new Uri("https://localhost:5001/connect/authorize"),
                TokenUrl = new Uri("https://localhost:5001/connect/token"),
                Scopes = new Dictionary<string, string>
                            {
                                {"productApi", "Product API"}
                            }
            },
        }
    });
    // Apply Scheme globally
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "oauth2" }
            },
            new[] { "productApi" }
        }
    });
});

builder.Services.AddLogging(options =>
{
    options.AddConsole();
    options.AddDebug();
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.OAuthUsePkce();
    options.SwaggerEndpoint($"./v1/swagger.json", "SapWeb v1");
});

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
