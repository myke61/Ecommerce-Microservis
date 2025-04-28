using Ecommerce.Base;
using Order.Application;
using Order.Persistance;    
using Order.Infastructure.MassTransit;
using Order.API.Services.LoginService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Caching.Redis;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = "https://localhost:5001";
        options.Audience = "orderApi";
        options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        {
            RoleClaimType = "role",
            ValidateAudience = false
        };
    });

builder.Services.AddAuthorizationBuilder()
    .AddPolicy("orderApi", policy =>
    {
        policy.RequireAuthenticatedUser();
        policy.RequireClaim("scope", "orderApi");
    });

builder.Services.AddRedisCache();

// Add services to the container.
builder.Services.AddPersistenceServices();
builder.Services.AddApplicationServices();
builder.Services.AddMassTransitWithRabbitMQ();
builder.Services.AddBaseServices();

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ILoginService, LoginService>();

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
